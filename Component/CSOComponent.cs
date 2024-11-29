using System;
using CSOEngine.Object;
using CSOEngine.Proxy;

namespace CSOEngine.Component
{
    public abstract class CSOComponent:CSOBaseComponent
    {
        [Obsolete("请使用cso代替cso_obsolete")]
        protected CObject cso_obsolete => owner;
                
                
        private CSOProxy cso_proxy;
        protected CSOProxy cso
        {
            get
            {
                if (cso_proxy == null)
                {
                    cso_proxy = new CSOProxy(owner);
                }
                return cso_proxy;
            }   
        }
    }
}