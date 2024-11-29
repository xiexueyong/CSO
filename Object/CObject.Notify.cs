using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
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