namespace CSOEngine.Utils.Pool 
{
    public interface ICSOPoolObject
    {
        

        // 当对象被重新激活时调用
        void OnPOActivate();
        //被回收
        void OnPODeactivate();

        // 检查对象是否正在使用
        bool InPool { get; set; }
        bool UseDefaultReset { get; }
    }

}