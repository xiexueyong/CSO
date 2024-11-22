using CSOEngine.Object;

namespace CSOEngine.Component
{
    public abstract class CSOComponent:CSOBaseComponent
    {
        protected CObject cso => owner;
    }
}