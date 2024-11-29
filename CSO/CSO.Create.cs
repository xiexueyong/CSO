using System;
using System.Collections.Generic;
using CSOEngine.Object;
using CSOEngine.Proxy;
using CSOEngine.Utils.Pool;
using UnityEngine;

namespace CSOEngine
{
    /// <summary>
    /// CSObject工厂类
    /// </summary>
    public static partial class CSO
    {
        private static readonly List<CObject> csObjects = new ();
        public static CObject Create(string name = null)
        {
            Init();
            var c = CObject.createObj();
            csObjects.Add(c);
            createCSOGameobject(c, null, name, null);
            return c;
        }

        /// <summary>
        /// 构造无父节点的CSO
        /// </summary>
        /// <param name="args">可变参数列表</param>
        /// <typeparam name="T">构造器泛型类</typeparam>
        /// <returns></returns>
        public static CObject Create<T>(string name, params object[] args) where T : class, ICSOBuilder, new()
        {
            Init();
            var c = CObject.createObj();
            csObjects.Add(c);
            createCSOGameobject(c, null, name, typeof(T));
            BuildCSO<T>(c, args);
            //处理Components的通知
            c.triggerComponentLifecyle();

            //处理Ability 、 View的通知
            c.triggerAbilityLifecyle();
            c.triggerViewLifecyle();
           
            return c;
        }


        /// <summary>
        /// 构造有父节点的CSO
        /// </summary>
        /// <param name="parent">父亲cso对象</param>
        /// <param name="args">可变参数列表</param>
        /// <typeparam name="T">构造器泛型类</typeparam>
        /// <returns></returns>
        public static CObject CreateWithParent<T>(CObject parent, string csoName, params object[] args)
            where T : class, ICSOBuilder, new()
        {
            var child = parent.createChildObj();
            createCSOGameobject(child, parent, csoName, typeof(T));
            BuildCSO<T>(child, args);

            //处理Components的通知
            child.triggerComponentLifecyle();

            //处理Ability、View的通知
            child.triggerAbilityLifecyle();
            child.triggerViewLifecyle();

            //继承Parent的Active
            if (parent.Activated && !parent.Destroyed)
            {
                child.Active(true);
            }

            //添加到Collection
            parent.ChildrenGroup.Add(child, true);
            parent.ChildrenGroup.TryAddUniqKey(child);
            return child;
        }
        public static CObject CreateWithParent<T>(CSOProxy parentProxy, string csoName, params object[] args)
            where T : class, ICSOBuilder, new()
        {
            return CreateWithParent<T>(parentProxy.cso,csoName,args);
        }
        
        public static CObject CreateWithParent(CObject parent, string csoName)
        {
            var child = parent.createChildObj();
            createCSOGameobject(child, parent, csoName, null);
            //继承Parent的Active
            if (parent.Activated && !parent.Destroyed)
            {
                child.Active(true);
            }

            //添加到Collection
            parent.ChildrenGroup.Add(child, true);
            parent.ChildrenGroup.TryAddUniqKey(child);
            return child;
        }

        public static CObject CreateWithParent(CSOProxy parent, string csoName)
        {
            return CreateWithParent(parent.cso,csoName);
        }


        private static void BuildCSO<T>(CObject cso, object[] args) where T : class, ICSOBuilder, new()
        {
            var builder = CSOPool.Get<T>();
            cso.BuilderType = typeof(T);
            builder.OnBuild(cso, args);
            CSOPool.Release<T>(builder);
        }

        /// <summary>
        /// 创建cso的gameobject
        /// </summary>
        /// <param name="linkerCSObject"></param>
        /// <param name="parentCsObject"></param>
        /// <param name="name"></param>
        /// <param name="builderType"></param>
        private static void createCSOGameobject(CObject linkerCSObject, CObject parentCsObject, string name,
            Type builderType)
        {
            string templinkerName = null;
#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(name))
            {
                templinkerName = name;// $"{name}";
            }
            else
            {
                if (builderType != null)
                {
                    templinkerName = builderType.Name;//$"{builderType.Name}";
                }
                else
                {
                    templinkerName = "CSO";
                }

            }
#endif
            //create cso linker gameobject
            var csoLinkerGameobject = new GameObject(templinkerName);
            var linker = csoLinkerGameobject.AddComponent<CSOForceLinker>();
            linker.SetCObject(linkerCSObject);
            linkerCSObject.LinkerTransform = csoLinkerGameobject.transform;

            if (parentCsObject == null)
            {
                csoLinkerGameobject.transform.parent = CSOLinkerRootTransform;
            }
            else
            {
                csoLinkerGameobject.transform.parent = parentCsObject.LinkerTransform;
            }
            
            
            //create cso view gameobject
            string tempCSOViewName = templinkerName != null ? $"{templinkerName}-gameobject" : null;
            var csoViweGameobject = new GameObject(tempCSOViewName);
            linker.ViewGameobject = csoViweGameobject;
            linkerCSObject.Transform = csoViweGameobject.transform;

            if (parentCsObject == null)
            {
                csoViweGameobject.transform.parent = CSOViewRootTransform;
            }
            else
            {
                csoViweGameobject.transform.parent = parentCsObject.Transform;
            }
            
            
            
            
            
            
        }
    }
}