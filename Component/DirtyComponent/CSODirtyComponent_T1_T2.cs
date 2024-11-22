namespace CSOEngine.Component
{   
    public abstract class CSODirtyComponent<T1,T2> : CSODirtyComponent 
    {
        #region value1

        private T1 _value1;

        public T1 Value1
        {
            get => _value1;
            set
            {
                bool equal = _value1 != null && _value1.Equals(value);
                if (equal)
                    return;

                _value1 = value;
                Dirty = true; 
            }
        }

        #endregion
        
        #region value2
        
        
     
        private T2 _value2;

        public T2 Value2
        {
            get => _value2;
            set
            {
                bool equal = _value2 != null && _value2.Equals(value);
                if (equal)
                    return;

                _value2 = value;
                Dirty = true; 
            }
        }
        #endregion
        public virtual void SetValue(T1 val1,T2 val2)
        {
            this.Value1 = val1;
            this.Value2 = val2;
        }
        
        public virtual void SetValue(T1 val1)
        {
            this.Value1 = val1;
            this.Value2 = default;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{_value1} , {_value2}";
        }

    }
}