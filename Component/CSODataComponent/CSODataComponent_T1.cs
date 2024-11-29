namespace CSOEngine.Component
{   
    public abstract class CSODataComponent<T> : CSODataComponent 
    {
        public T Value;
        public virtual void SetValue(T val)
        {
            this.Value = val;
        }
    }
}