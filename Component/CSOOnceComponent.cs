using System;
using System.Reflection;


namespace CSOEngine.Component
{
    public abstract class CSOOnceComponent<T> : CSODirtyComponent
    {
        private T _defaultValue;
        private T _oneTimeValue;
        public CSOOnceComponent(T defaultValue)
        {
            _defaultValue = defaultValue;
            _oneTimeValue = defaultValue;
        }
        public T OneTimeValue
        {
            get
            {
                var value = _oneTimeValue;
                _oneTimeValue = _defaultValue;
                Dirty = false;
                return value;
            }
            set
            {
                Dirty = true;
                _oneTimeValue = value;
            }
        }

    }
}