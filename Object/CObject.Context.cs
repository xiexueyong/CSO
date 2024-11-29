using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        private ComponentsContainer contextContainer;

        public T AddContext<T>()where T:CSOContextComponent,new()
        {
            if (!isRoot)
            {
                throw new Exception("只能在跟节点添加context");
            }
            var context = new T();
            return AddContext(context) as T;
        }

        public CSOContextComponent AddContext(CSOContextComponent contextComponent)
        {
            if (contextContainer == null)
            {
                contextContainer = new ComponentsContainer();
            }

            contextContainer.AddComp(contextComponent);
            return contextComponent;
        }

        public T GetContext<T>()where T:CSOContextComponent
        {
            var cso = this;
            while (cso != null)
            {
                T t = cso.contextContainer?.GetComp<T>();

                if (t != null)
                    return t;
                cso = parent;
            }

            return null;
        }
    }
}