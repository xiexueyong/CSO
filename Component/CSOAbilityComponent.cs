using CSOEngine.Object;
namespace CSOEngine.Component
{
    //在子类中实现IUpdate，即可每帧执行Update
    public abstract class CSOAbilityComponent:CSOEvtComponent
    {
        protected bool ViewHidden { get; set; }
        protected bool ViewDestroyed { get; set; }
        
        internal void Update()
        {
            OnUpdate();
        }
        
        internal void ShowView()
        {
            ViewHidden = false;
            OnShowView();
        }
        
        internal void HideView()
        {
            ViewHidden = true;
            OnHideView();
        }
        
        internal void DestroyView()
        {
            ViewDestroyed = true;
            OnDestroyView();
        }
        internal void RebuildView()
        {
            ViewDestroyed = false;
            OnRebuildView();
        }
        protected virtual void OnUpdate()
        {
            
        }
        //显示视图
        protected virtual void OnShowView()
        {
            
        }
        //隐藏视图
        protected virtual void OnHideView()
        {
            
        }
        //销毁视图
        protected virtual void OnDestroyView()
        {
            
        }
        //重建视图，销毁后再重建
        protected virtual void OnRebuildView()
        {
            
        }
      
    }
}