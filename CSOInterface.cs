namespace CSOEngine
{
    
    public interface IDestroyable
    {
        public void OnDestroy();
    }
    
    
    interface IDirty
    {
        public bool Dirty { get; }
    }
}