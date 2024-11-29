using System;
using CSOEngine.Component;
using CSOEngine.Object;
using CSOEngine.Proxy;

namespace CSOEngine.CSOGenerated
{
    public class CSOFasterViewBase
    {
        private CObject _cso;
        public CObject cso
        {
            set
            {
                _cso = value;
            }
        }

        public CSOProxy Proxy
        {
            set
            {
                _cso = value.cso;
            }
        }
        
        public CSOViewComponent ViewComponent
        {
            set
            {
                _cso = value.owner;
            }
        }

        protected T GetView<T>() where T : CSOViewComponent
        {
            if (_cso == null || _cso.Destroyed)
            {
                throw new Exception("cso为null,或者cso 已经被销毁,无法获取view");
            }
            return _cso.GetView<T>();
        }
    }
}