using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        internal ComponentsContainer viewContainer;
     
        
        public T AddView<T>(bool fromBuilder = true)where T:CSOViewComponent,new()
        {
            var view = CSOPool.Get<T>();
            return AddView(view,fromBuilder) as T;
        }
        
        public CSOViewComponent AddView(CSOViewComponent viewComponent,bool fromBuilder)
        {
            if (viewContainer==null)
            {
                viewContainer = new ComponentsContainer();
            }
            viewComponent.owner = this;
            viewContainer.AddComp(viewComponent);

            if (!fromBuilder)
            {
                triggerViewLifecyle(viewComponent);
                
                if (Activated)
                {
                    viewComponent.Active(Activated);
                }
            }
            
            OnAddView?.Invoke(viewComponent);
            return viewComponent;
        }

        internal void triggerViewLifecyle()
        {
            if (viewContainer != null && viewContainer.components != null)
            {
                foreach (var item in viewContainer.components)
                {
                    triggerViewLifecyle(item.Value as CSOViewComponent);
                }
            }
        }
        private void triggerViewLifecyle(CSOViewComponent viewComponent)
        {
            //触发OnStart
            viewComponent.Start();  
        }

        public CSOViewComponent RemoveView<T>() where T : CSOViewComponent
        {
            var component = viewContainer?.RemoveComp<T>();
            OnRemoveView?.Invoke(component);
            return component as T;
        }
        
        internal T GetView<T>()where T:CSOViewComponent
        {
            if (viewContainer==null)
            {
                return null;
            }
            return viewContainer.GetComp<T>() as T;
        }
        
        internal T GetViewByBaseClass<T>()where T:CSOViewComponent
        {
            if (viewContainer==null)
            {
                return null;
            }
            return viewContainer.GetCompByBaseClass<T>();
        }

        internal bool HasView<T>()
        {
            if (viewContainer==null)
            {
                return false;
            }
            return viewContainer.HasComp<T>();
        }
       
    }
}