namespace CSOEngine.Component
{   
    public abstract class CSOContextComponent<T1,T2> : CSOContextComponent 
    {
        public T1 Value1;
        public virtual void SetValue(T1 val)
        {
            this.Value1 = val;
        }
        
        public T2 Value2;
        public virtual void SetValue(T2 val)
        {
            this.Value2 = val;
        }
    }
}