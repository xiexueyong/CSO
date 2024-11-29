using System.Collections.Generic;

namespace CSOEngine.Component
{   
    public abstract class CSODictionaryDirtyComponent<TKey,TValue> : CSODirtyComponent<Dictionary<TKey,TValue>>
    {
        public virtual void Add(TKey key,TValue value)
        {
            Value.Add(key,value);
            Dirty = true;
            // todo:ValueNotify?.Invoke(ValueDirtyType.Add,t,1);
        }
    
        public virtual void Remove(TKey key)
        {
            Value.Remove(key);
            Dirty = true;
            // todo:ValueNotify?.Invoke(ValueDirtyType.Remove,t,1);
        }
        public virtual void Update(TKey key,TValue value)
        {
            if (Value.TryGetValue(key,out var v))
            {
                if (!v.Equals(value))
                {
                    Value[key] = value;
                    Dirty  = true;
                }
            }
            else
            {
                Add(key,value);
            }
            // todo:ValueNotify?.Invoke(ValueDirtyType.Update,t,1);
        }
        public virtual void SetValue(Dictionary<TKey,TValue> val)
        {
            this.Value = val;
        }
    }
}