using System;
using CSOEngine.Component;
using CSOEngine.Group;

namespace CSOEngine.Object
{
    public partial interface ICObject
    {
        public event Action OnDestroy;
        public event Action<bool> OnActive;
        public event Action<CSOBaseComponent> OnAddComp;
        public event Action<CSOBaseComponent> OnRemoveComp;
        
        
        public event Action<CSOBaseComponent> OnAddAbility;
        public event Action<CSOBaseComponent> OnRemoveAbility;
        
        public event Action<CSOBaseComponent> OnAddView;
        public event Action<CSOBaseComponent> OnRemoveView;

    }
}