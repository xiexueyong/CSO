namespace CSOEngine.Component
{   
    public abstract class CSODirtyComponent<T1,T2,T3> : CSODirtyComponent 
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
        
        
        #region value3
        private T3 _value3;
        public T3 Value3
        {
            get => _value3;
            set
            {
                bool equal = _value3 != null && _value3.Equals(value);
                if (equal)
                    return;

                _value3 = value;
                Dirty = true; 
            }
        }
        #endregion
        
        
        public virtual void SetValue(T1 val1,T2 val2,T3 val3)
        {
            this.Value1 = val1;
            this.Value2 = val2;
            this.Value3 = val3;
        }
        public virtual void SetValue(T1 val1,T2 val2)
        {
            this.Value1 = val1;
            this.Value2 = val2;
            this.Value3 = default;
        }
        
        public virtual void SetValue(T1 val1)
        {
            this.Value1 = val1;
            this.Value2 = default;
            this.Value3 = default;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}:{_value1} , {_value2} , {_value3}";
        }

    }
}