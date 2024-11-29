using System;
namespace CSOEngine.Component
{

    public abstract partial class CSODirtyComponent:CSODataComponent
    {
        public event Action<CSODirtyComponent> OnAdd;//自己new 一个component，使用AddComp(component)时可以用到。
        public event Action<CSODirtyComponent> OnRemove;
        public event Action<CSODirtyComponent> OnDirty;
        
        
        internal void DirtyNotify(DirtyType type)
        {
            if (!Started)
            {
                //未开始时，dirty 、remove都不通知
                if (type == DirtyType.Dirty || type == DirtyType.Removed)
                {
                    return;
                }
            }

            if (type == DirtyType.Added)
            {
                OnAdd?.Invoke(this);
            }else if (type == DirtyType.Removed)
            {
                 OnRemove?.Invoke(this);
            }else if (type == DirtyType.Dirty)
            {
                 OnDirty?.Invoke(this);
            }
        }
    }
}