namespace CSOEngine.Component
{   
    public abstract class CSOContextComponent<T1,T2,T3> : CSOContextComponent 
    {
        public T1 Value1;
        public T2 Value2;
        public T3 Value3;
        
        
        public virtual void SetValue(T1 val)
        {
            this.Value1 = val;
        }
       
        public virtual void SetValue(T2 val)
        {
            this.Value2 = val;
        }
    }
}