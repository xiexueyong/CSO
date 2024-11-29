using System;

namespace CSOEngine.Component
{   
    public enum ValueDirtyType  //可扩展自定义
    {
        None,
        Initialize, //初始化数据
        Remove, //删除数据
        Add, // 追加数据
        Update //修改数据
    }

    public abstract partial class CSODirtyComponent:CSODataComponent
    {
        public enum DirtyType
        {
            None,
            Added,
            Removed,
            Dirty
        }
        internal bool dirty;
        protected bool Dirty
        {
            set=> dirty = value;
        }
        public void ForceDirty()
        {
            Dirty = true;
        }
    }
}