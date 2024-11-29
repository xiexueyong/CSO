namespace CSOEngine.Component
{   
    public abstract class CSODataComponent<T1,T2> : CSODataComponent 
    {
        public T1 Value1;
        public T2 Value2;
        
        public virtual void SetValue(T1 val)
        {
            this.Value1 = val;
        }
    }
}