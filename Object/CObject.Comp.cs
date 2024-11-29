using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;
using UnityEngine;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        internal ComponentsContainer componentContainer;
        
        public T GetOrAddComp<T>(bool fromBuilder = false) where T : CSODataComponent,new()
        {
            var c = GetComp<T>();
            if (c == null)
            {
                return AddComp<T>(fromBuilder);
            }
            return c;
        }
        
        public T  AddComp<T>(bool fromBuilder = true)where T :CSODataComponent,new()
        {
            if (Destroyed)
            {
                Debug.LogError("cobject has been destroyd, cant add component");
                return null;
            }
            
            if (typeof(T) == typeof(UniqKeyComponent))
            {
                throw new Exception("UniqKeyComponent类型的组件必须先赋值，使用AddComp(CSOBaseComponent component)方法添加,因为添加时需要根据值计算hashcode");
            }
            
            var comp = CSOPool.Get<T>();
            AddComp(comp,fromBuilder);
            return comp;
        }
        /// <summary>
        /// 添加组件，如果fromBuilder，则在添加组件后，统一发送通知。如果不是，则立即发送通知。
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fromBuilder">是否从Factory.Builder添加组件，影响生命周期的调用。</param>
        /// <returns></returns>
        public CSODataComponent  AddComp(CSODataComponent component,bool fromBuilder = true)
        {
            if (Destroyed)
            {
                Debug.LogError("cobject has been destroyd, cant add component");
                return null;
            }

            if (componentContainer == null)
            {
                componentContainer = new ComponentsContainer();
            }
            
            component.owner = this;
            
            //添加并触发OnAdd
            componentContainer.AddComp(component,fromBuilder);

            if (!fromBuilder){
                triggerComponentLifecyle(component);
                
                //触发Active
                if (Activated)
                {
                    component.Active(Activated);
                } 
                
                //添加到Collection
                parent_group?.Add(this,false);
                if(component is UniqKeyComponent uniqKeyComponent)
                    parent_group?.TryAddUniqKey(this,uniqKeyComponent);
                //触发cso dirty.add通知
                tryTriggerDirtyNotifierFromAddOrRemove(component,CSODirtyComponent.DirtyType.Added);
            }

            OnAddComp?.Invoke(component);
            if (component is CSODirtyComponent dirtyComponent)
            {
                dirtyComponent.DirtyNotify(CSODirtyComponent.DirtyType.Added);
            }
            return component;
        }
        
        public  T GetComp<T>()where T:CSODataComponent
        {
            Type type = typeof(T);
            return GetComp(type) as T;
        }
        
        public CSODataComponent GetComp(Type type) 
        {
            if (componentContainer == null || componentContainer.components.Count == 0)
            {
                return null;
            }
            if (Destroyed)
            {
                Debug.LogError("cobject has been destroyd, cant get component");
                return null;
            }
            
            if (type.IsSubclassOf(typeof(CSOComponent)))
            {
                throw new Exception("ability component should use GetAbility to Get");
            }

            return componentContainer.GetComp(type) as CSODataComponent;
        }

        internal void triggerComponentLifecyle()
        {
            foreach (var item in componentContainer.components)
            {
                triggerComponentLifecyle(item.Value);
            }
            
        }

        private void triggerComponentLifecyle(CSOBaseComponent component)
        {
            component?.Start(); 
        }
        
        /// <summary>
        /// 根据基类获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  T GetCompByBaseClass<T>()where T: CSODataComponent
        {
            return componentContainer.GetCompByBaseClass<T>();
        }

        public void RemoveComp<T>()where T: CSODataComponent
        {
            var comp = GetComp<T>();
            RemoveComp(comp);
        }

        public void RemoveComp(CSODataComponent component)
        {
            if (component == null)
            {
                return;
            }
            if(component is UniqKeyComponent)
                parent_group?.TryRemoveUniqKey(this, component as UniqKeyComponent);
            
            parent_group?.TryRemoveFromMatcherCollection(this);
            
            //发送Removed通知
            if (component is CSODirtyComponent)
            {
                var dirtyComp = component as CSODirtyComponent;
                dirtyComp.DirtyNotify(CSODirtyComponent.DirtyType.Removed);
            }
            
            //触发cso dirty.remove通知
            tryTriggerDirtyNotifierFromAddOrRemove(component,CSODirtyComponent.DirtyType.Removed);
            
            
            OnRemoveComp?.Invoke(component);
            //移除，触发OnRemove
            componentContainer.RemoveComp(component);
           
        }
        
        public bool HasComp<T>()
        {
            Type type = typeof(T);
            return HasComp(type);
        }
        public bool HasComp(Type type)
        {
            if (componentContainer == null || componentContainer.components.Count == 0)
                return false;
            
            return componentContainer.HasComp(type);
        }
        
        public bool HasComps(HashSet<Type> types)
        {
            foreach (var type in types)
            {
                if (!HasComp(type))
                {
                    return false;
                }
            }
            return true;
        }
        
    }
}