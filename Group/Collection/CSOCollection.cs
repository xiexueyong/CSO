using System;
using System.Collections.Generic;
using System.Linq;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public class CSOCollection
    {
        private CSOMatcher matcher;
        private readonly HashSet<CObject> objects_hashset;
        private List<CObject> objects_list;
        private bool listNeedRefresh;

        public delegate void CollectionEvent(CObject obj);
        public event CollectionEvent OnAdd;
        public event CollectionEvent OnRemove;
        public event Action<CObject,CSOBaseComponent> OnDirty;

        private readonly bool ignoreDirty;
        public CSOCollection(bool ignoreDirty)
        {
            objects_hashset = new HashSet<CObject>();
            this.ignoreDirty = ignoreDirty;
        }
        public virtual void Add(CObject obj)
        {
            if (objects_hashset.Add(obj))
            {
                listNeedRefresh = true;
                OnAdd?.Invoke(obj);
                
                //只发送关注的type Dirty事件。all是一个特殊的Collection，不发送dirty事件
                if (!ignoreDirty && matcher != null)
                {
                    //添加事件
                    RegisterDirtyNotify(obj,matcher.allof);
                    RegisterDirtyNotify(obj,matcher.anyof);
                }
            }
        }
        
        public virtual void Remove(CObject obj)
        {
            if (objects_hashset.Remove(obj))
            {
                listNeedRefresh = true;
                OnRemove?.Invoke(obj);
                
                //移除事件
                if (!ignoreDirty && matcher != null)
                {
                    //添加事件
                    UnRegisterDirtyNotify(obj,matcher.allof);
                    UnRegisterDirtyNotify(obj,matcher.anyof);
                }
            }
        }

        private void RegisterDirtyNotify(CObject obj,HashSet<Type> componentTypes)
        {
            if (componentTypes != null && componentTypes.Count>0)
            {
                foreach (var type in componentTypes)
                {
                    var dirtyComp = obj.GetComp(type) as CSODirtyComponent;
                    if (dirtyComp != null)
                    {
                        dirtyComp.OnDirty += OnCompDirty;
                    }
                }
            }
        }
        private void UnRegisterDirtyNotify(CObject obj,HashSet<Type> componentTypes)
        {
            if (componentTypes != null && componentTypes.Count>0)
            {
                foreach (var type in componentTypes)
                {
                    var dirtyComp = obj.GetComp(type) as CSODirtyComponent;
                    if (dirtyComp != null)
                    {
                        dirtyComp.OnDirty -= OnCompDirty;
                    }
                }
            }
        }

        private void OnCompDirty(CSODirtyComponent comp)
        {
            //removed 类型不发送通知，因为会走Collection.OnRemove
            OnDirty?.Invoke(comp.owner,comp);
        }
      

        public HashSet<CObject> Objects
        {
            get
            {
                return objects_hashset;
            }
        }
        
        /// <summary>
        ///  优先使用ObjectsHashSet，尽量不要使用ObjectList，会有性能和内存消耗
        /// </summary>
        ///
        [Obsolete("This method is obsolete, use Objects instead.", false)]
        public List<CObject> ObjectList
        {
            get
            {
                if (listNeedRefresh || objects_list == null)
                {
                    listNeedRefresh = false;
                    objects_list = objects_hashset.ToList();
                }
                return objects_list;
            }
        }

        internal void SetMatcher(CSOMatcher matcher)
        {
            this.matcher = matcher;
        }
        
        internal bool sameMatcher(CSOMatcher matcher)
        {
            return matcher.Equals(this.matcher);
        }

        internal virtual bool TryAdd(CObject co,bool isSon)
        {
            if (matcher == null || matcher.Check(co,isSon))
            {
                Add(co);
                return true;
            }
            return false;
        }
        internal virtual bool TryRemvoe(CObject co)
        {
            if (matcher == null)
            {
                return false;
            }

            if (!matcher.Check(co,true))//onlyson 填true，表示不关心是否是son，因为添加时已经校验过，移除是不必再次校验
            {
                Remove(co);
                return true;
            }
            
            return false;
        }
        
        internal virtual void fillCollection(CSOCollection source,CObject parent)
        {
            if (matcher != null)
            {
                foreach (var item in source.Objects)
                {
                    if (matcher.Check(item,item.parent == parent))
                    {
                        Add(item);
                    }
                }
            }
        }
        
        public void Clear()
        {
            objects_hashset?.Clear();
        }
    }
}