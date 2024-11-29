using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;
using CSOEngine.Proxy;
using UnityEngine;

namespace CSOEngine
{
    [DisallowMultipleComponent]
    public abstract class CSOLinker:MonoBehaviour
    {
        [SerializeField]
        protected CObject csobject;
        [SerializeField]
        protected List<string> CompInfos;

        public void SetCObject(CObject csobject)
        {
            if (this.csobject != null)
            {
                throw new Exception("CSOLinker 重复设置csobject");
            }
            this.csobject = csobject;
            watchCSONotify();
            RefreshCompInfos();
        }
        public void SetCObject(CSOProxy proxy)
        {
            SetCObject(proxy.cso);
        }
        public CObject GetCObject()
        {
            return this.csobject;
        }
        
        protected void watchCSONotify()
        { 
            csobject.OnActive += OnActive;
            csobject.OnDestroy += OnCSObjectDestroy;
            csobject.OnAddComp += OnCSOAddComp;
            csobject.OnRemoveComp += OnCSORemoveComp;
            csobject.OnAddAbility += OnCSOAddAbility;
            csobject.OnRemoveAbility += OnCSORemoveAbility;
        }
        protected void clearCSOEvt()
        {
             csobject.OnActive -= OnActive;
            csobject.OnDestroy -= OnCSObjectDestroy;
            csobject.OnAddComp -= OnCSOAddComp;
            csobject.OnRemoveComp -= OnCSORemoveComp;
            csobject.OnAddAbility -= OnCSOAddAbility;
            csobject.OnRemoveAbility -= OnCSORemoveAbility;
        }

        protected virtual void OnActive(bool value)
        {
        }
        protected virtual void OnCSObjectDestroy()
        {
            clearCSOEvt();
            csobject = null;
        }
        protected virtual  void OnCSOAddComp(CSOBaseComponent component)
        {
            RefreshCompInfos();
        }
        protected virtual  void OnCSORemoveComp(CSOBaseComponent component)
        {
            RefreshCompInfos();
        }
        protected virtual  void OnCSOAddAbility(CSOBaseComponent component)
        {
            RefreshCompInfos();
        }
        protected virtual  void OnCSORemoveAbility(CSOBaseComponent component)
        {
            RefreshCompInfos();
        }
        
        
        protected void RefreshCompInfos()
        {
#if  UNITY_EDITOR
            CompInfos = new List<string>();
            CompInfos.Add("----components----");
            if (csobject != null && csobject.componentContainer != null && csobject.componentContainer.components != null && csobject.componentContainer.components.Count>0)
            {
                foreach (var comp in csobject.componentContainer.components)
                {
                    CompInfos.Add(comp.Value.ToString());
                }
            }  
            CompInfos.Add("----ability----");
            if (csobject != null && csobject.abilityContainer != null && csobject.abilityContainer.components != null && csobject.abilityContainer.components.Count>0)
            {
                foreach (var comp in csobject.abilityContainer.components)
                {
                    CompInfos.Add(comp.Value.ToString());
                }
            }
#endif
        }
    }
}