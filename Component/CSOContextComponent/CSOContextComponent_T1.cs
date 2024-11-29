namespace CSOEngine.Component
{   
    public abstract class CSOContextComponent<T> : CSOContextComponent 
    {
        public T Value;
        public virtual void SetValue(T val)
        {
            this.Value = val;
        }
    }
}