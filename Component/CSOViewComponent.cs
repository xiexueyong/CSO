using CSOEngine.Object;
using CSOEngine.Proxy;
using UnityEngine;

namespace CSOEngine.Component
{
    //在子类中实现IUpdate，即可每帧执行Update
    public abstract class CSOViewComponent:CSOEvtComponent
    {
        protected Transform RootTransform => cso.cso.RootTransform;
        protected Transform Transform => cso.cso.Transform;
        
        protected T GetView<T>()where T:CSOViewComponent
        {
            return owner.GetView<T>();
        } 
        
        protected T GetView<T>(CObject cObject)where T:CSOViewComponent
        {
            return cObject.GetView<T>();
        } 
        
        protected T GetView<T>(CSOProxy proxy)where T:CSOViewComponent
        {
            return proxy.cso.GetView<T>();
        } 
        
        
        
        
        
        
        
        
        
        
        
        
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