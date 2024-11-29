using System;
using CSOEngine.Component;
using CSOEngine.Group;
using CSOEngine.Group.DirtyMatcher;
using CSOEngine.Object;
using CSOEngine.State;
using UnityEngine;

namespace CSOEngine.Proxy
{
    public partial class CSOProxy:ICObject
    {
        public event Action OnDestroy
        {
            add { cso.OnDestroy += value; }
            remove { cso.OnDestroy -= value; }  
        }
        public event Action<bool> OnActive
        {
            add { cso.OnActive += value; }
            remove { cso.OnActive -= value; }  
        }
        public event Action<CSOBaseComponent> OnAddComp
        {
            add { cso.OnAddComp += value; }
            remove { cso.OnAddComp -= value; }
        }

        public event Action<CSOBaseComponent> OnRemoveComp
        {
            add { cso.OnRemoveComp += value; }
            remove { cso.OnRemoveComp -= value; }
        }
        
        public event Action<CSOBaseComponent> OnAddAbility
        {
            add { cso.OnAddAbility += value; }
            remove { cso.OnAddAbility -= value; }
        }

        public event Action<CSOBaseComponent> OnRemoveAbility
        {
            add { cso.OnRemoveAbility += value; }
            remove { cso.OnRemoveAbility -= value; }
        }
        
        public event Action<CSOBaseComponent> OnAddView
        {
            add { cso.OnAddView += value; }
            remove { cso.OnAddView -= value; }
        }

        public event Action<CSOBaseComponent> OnRemoveView
        {
            add { cso.OnRemoveView += value; }
            remove { cso.OnRemoveView -= value; }
        }

        public void AddDirtyListener(DirtyMatcher matcher,
            Action<CSODataComponent[], CSODirtyComponent.DirtyType> action)
        {
            cso.AddDirtyListener(matcher, action);
        }

        public void RemoveDirtyListener(DirtyMatcher matcher,
            Action<CSODataComponent[], CSODirtyComponent.DirtyType> action)
        {
            cso.RemoveDirtyListener(matcher, action);
        }
        
    }
}

