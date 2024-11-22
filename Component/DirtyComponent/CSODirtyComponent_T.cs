namespace CSOEngine.Component
{   
    public abstract class CSODirtyComponent<T> : CSODirtyComponent 
    {
        public event ValueDirtyNotify ValueNotify;
        public delegate void ValueDirtyNotify(ValueDirtyType dirtyType,T t);
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                bool equal = _value != null && _value.Equals(value);
                if (equal)
                    return;

                _value = value;
                Dirty = true; 
            }
        }
        public virtual void SetValue(T val)
        {
            this.Value = val;
        }

        protected  void SetValueNotify(ValueDirtyType dirtyType,T t) {
            ValueNotify?.Invoke(dirtyType,t);
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{_value}";
        }
    }
}