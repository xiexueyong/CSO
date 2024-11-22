using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Component
{
    public partial class ComponentsContainer
    {
        internal Dictionary<Type,CSOBaseComponent> components;

        public ComponentsContainer()
        {
            components = new Dictionary<Type, CSOBaseComponent>();
        }

        public bool HasComp<T>()
        {
            Type type = typeof(T);
            return HasComp(type);
        }
        public bool HasComp(Type type)
        {
            if (components == null || components.Count == 0)
                return false;
            
            return components.ContainsKey(type);
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
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  T GetComp<T>()where T:CSOBaseComponent
        {
            Type type = typeof(T);
            return GetComp(type) as T;
        }

     

        public virtual CSOBaseComponent GetComp(Type type)
        {
            if (components == null || components.Count == 0)
                return null;
            if (components.TryGetValue(type,out var comp))
            {
                return comp;
            }
            return null;
        }

        public virtual CSOBaseComponent AddComp(CSOBaseComponent component,bool fromBuilder = false)
        {
            if (components == null)
            {
                components = new Dictionary<Type, CSOBaseComponent>();
            }

            Type type = component.GetType();
            try
            {
                components.Add(type,component);
            }
            catch (Exception e)
            {
                throw new Exception("该类型的component已经存在："+type.ToString());
            }
            
            component.Add();
            return component;
        }

        public virtual T AddComp<T>(bool fromBuilder = false) where T :CSOBaseComponent,new()
        {
            var comp = CSOPool.Get<T>();
            return AddComp(comp,fromBuilder) as T;
        }

        public virtual CSOBaseComponent RemoveComp(CSOBaseComponent component)
        {
            if (components == null || component == null)
            {
                return null;
            }
            Type type = component.GetType();
            
            if (components.ContainsKey(type))
            {
                component.Remove();
                components.Remove(type);
            }

            return component;
        }
        public CSOBaseComponent RemoveComp<T>()where T:CSOBaseComponent
        {
            var c = GetComp<T>();
            RemoveComp(c);
            return c;
        }

        internal Dictionary<Type,CSOBaseComponent> GetComps()
        {
            return components;
        }

        internal void Destroy()
        {
            if (components != null && components.Count>0)
            {
                foreach (var component in components)
                {
                    component.Value.Remove();
                }
                components.Clear();
                components = null;
            }
        }
        
        internal void Active(bool value)
        {
            if (components != null && components.Count>0)
            {
                foreach (var component in components)
                {
                    component.Value.Active(value);
                }
            }
        }
    }
}