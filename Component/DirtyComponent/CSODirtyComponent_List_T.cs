using System;
using System.Collections;
using System.Collections.Generic;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Component
{
    public abstract class CSOListDirtyComponent<T> : CSODirtyComponent<List<T>>
    {
        public virtual void Add(T t)
        {
            Value.Add(t);
            CallValueNotify(ValueDirtyType.Add,t);
        }
    
        public virtual void Remove(T t)
        {
            Value.Remove(t);
            CallValueNotify(ValueDirtyType.Remove,t);
        }
        public virtual void Update(T t)
        {
            int index = Value.IndexOf(t);
            if (index == -1 || Value[index].Equals(t))
                return;

            Value[index] = t;
            CallValueNotify(ValueDirtyType.Update,t);
        }

        void CallValueNotify(ValueDirtyType dirtyType,T t) {
            var tmpList = CSOPool.Get<List<T>>();
            tmpList.Add(t);
            CallValueNotify(dirtyType,tmpList);
            CSOPool.Release(tmpList);
        }
        
        void CallValueNotify(ValueDirtyType dirtyType,List<T> ts) {
            SetValueNotify(dirtyType,ts );
        }

        public virtual void SetValue(List<T> val)
        {
            this.Value = val;
        }
    }
}