using CSOEngine.Component;
using CSOEngine.Object;
using CSOEngine.Proxy;

namespace CSOEngine.CSOGenerated
{
    public class CSOFasterBase
    {
        private CObject _cso;

        public CObject cso
        {
            set { _cso = value; }
        }

        public CSOProxy Proxy
        {
            set
            {
                _cso = value.cso;
            }
        }
        protected T GetComp<T>()where T:CSODataComponent
        {
            return _cso.GetComp<T>();
        }
        protected T GetAbility<T>()where T:CSOAbilityComponent
        {
            return _cso.GetAbility<T>();
        }
        protected T GetContext<T>()where T:CSOContextComponent
        {
            return _cso.GetContext<T>();
        }
    }
}