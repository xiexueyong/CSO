using System;
using System.Collections.Generic;
using System.Linq;
using CSOEngine.Component;
using CSOEngine.Object;
using JetBrains.Annotations;

namespace CSOEngine.Group
{
    public partial class CSOGroup
    {
        private Dictionary<int, CObject> _uniqKeys;
        internal Dictionary<int, CObject> UniqKeys
        {
            get
            {
                if (_uniqKeys == null)
                    _uniqKeys = new();
                return _uniqKeys;
            }
        }
        
        /// <summary>
        /// crate cso from builder时 尝试添加
        /// </summary>
        /// <param name="obj"></param>
        internal void TryAddUniqKey(CObject obj)
        {
            foreach (var item in obj.componentContainer.components)
            {
                if(item.Value is UniqKeyComponent component)
                    TryAddUniqKey(obj,component);
            }
        }
        /// <summary>
        /// AddComp 或者 create cso from builder时 添加UniqKey
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="component"></param>
        /// <exception cref="Exception"></exception>
        internal void TryAddUniqKey(CObject obj,UniqKeyComponent component)
        {
            int hashcode = component.GetHashCode();
            if (OwnerCSO.GetRootCSO().ChildrenGroup.UniqKeys.ContainsKey(hashcode))
            {
                throw new Exception($"uniqkey global repeated : {component.ToString()}");
            }
            else
            {
                OwnerCSO.GetRootCSO().ChildrenGroup.UniqKeys.Add(component.GetHashCode(),obj);
            }
        }

        internal void TryRemoveUniqKey(CObject obj, UniqKeyComponent component)
        {
            OwnerCSO.GetRootCSO().ChildrenGroup.UniqKeys.Remove(component.GetHashCode());
        }
        
        
        /// <summary>
        /// cso destroy时 遍历cso身上的所有uniqkey comp，从UniqKey中删除
        /// </summary>
        /// <param name="obj"></param>
        internal void TryRemoveUniqKey(CObject obj)
        {
            if (obj.componentContainer != null && obj.componentContainer.components.Count>0)
            {
                foreach (var item in obj.componentContainer.components)
                {
                    if(item.Value is UniqKeyComponent component)
                        TryRemoveUniqKey(obj,component);
                }
            }
        }

        internal CObject GetChildByHashCode(int hashcode)
        {
            if (OwnerCSO.GetRootCSO().ChildrenGroup.UniqKeys.TryGetValue(hashcode,out var cso))
            {
                return cso;
            } 
            return null;
        }
    }
}