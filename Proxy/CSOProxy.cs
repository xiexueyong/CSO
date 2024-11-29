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
        internal CObject cso;

        internal CSOProxy(CObject cso)
        {
            this.cso = cso;
        }

        public void Destroy()
        {
            cso.Destroy();
        }

        public void Active(bool value)
        {
            cso.Active(value);
        }

         [Obsolete("只能在状态内切状态，不可在Ability里切状态")]
        public Type GetCurState()
        {
            return cso.GetCurState();
        }

        public CObject AddState<T>(bool isDestroyState = false) where T : CSOState, new()
        {
            return cso.AddState<T>(isDestroyState);
        }

        public void SetFirstState<T>(params object[] args)
        {
            cso.SetFirstState<T>();
        }

        #region Property

        [Obsolete("请在ViewComponent内实现 view相关的功能，CSOViewComponent.Transform/RootTransform")]
        public Transform RootTransform => cso.RootTransform;
        
        [Obsolete("请在ViewComponent内实现 view相关的功能，CSOViewComponent.Transform/RootTransform")]
        public Transform Transform => cso.Transform;
        public bool Destroyed => cso.Destroyed;
        public bool Activated => cso.Activated;
        

        #endregion
        

        #region comp

        public T GetParentComp<T>() where T : CSODataComponent
        {
            return cso.GetParentComp<T>();
        }

        public T GetOrAddComp<T>(bool fromBuilder = false) where T : CSODataComponent, new()
        {
            return cso.GetOrAddComp<T>(fromBuilder);
        }

        public T AddComp<T>(bool fromBuilder = true) where T : CSODataComponent, new()
        {
            return cso.AddComp<T>(fromBuilder);
        }

        public CSODataComponent AddComp(CSODataComponent component, bool fromBuilder = true)
        {
            return cso.AddComp(component, fromBuilder);
        }

        public T GetComp<T>() where T : CSODataComponent
        {
            return cso.GetComp<T>();
        }

        public void RemoveComp<T>() where T : CSODataComponent
        {
            cso.RemoveComp<T>();
        }

        public void RemoveComp(CSODataComponent component)
        {
            cso.RemoveComp(component);
        }

        public bool HasComp<T>()
        {
            return cso.HasComp<T>();
        }

        public bool HasComp(Type type)
        {
            return cso.HasComp(type);
        }

        #endregion

        #region Ability

        public T AddAbility<T>(bool fromBuilder = true) where T : CSOAbilityComponent, new()
        {
            return cso.AddAbility<T>(fromBuilder);
        }

        public CSOAbilityComponent AddAbility(CSOAbilityComponent abilityComponent, bool fromBuilder)
        {
            return cso.AddAbility(abilityComponent, fromBuilder);
        }

        public CSOAbilityComponent RemoveAbility<T>() where T : CSOAbilityComponent
        {
            return cso.RemoveAbility<T>();
        }

        public T GetAbility<T>() where T : CSOAbilityComponent
        {
            return cso.GetAbility<T>();
        }

        public T GetAbilityByBaseClass<T>() where T : CSOAbilityComponent
        {
             return cso.GetAbilityByBaseClass<T>();
        }

        public bool HasAbility<T>()
        {
             return cso.HasAbility<T>();
        }

        #endregion

        #region Group Collection
         public CSOCollection GetCollection()
        {
            return cso.GetCollection();
        }

        public CSOCollection GetCollection<T>(bool onlySon=false) where T : CSODataComponent
        {
            return cso.GetCollection<T>(onlySon);
        }

        public CSOCollection GetCollection<T1, T2>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent
        {
            return cso.GetCollection<T1, T2>(onlySon);
        }

        public CSOCollection GetCollection<T1, T2, T3>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
        {
            return cso.GetCollection<T1, T2, T3>(onlySon);
        }

        public CSOCollection GetCollection<T1, T2, T3, T4>(bool onlySon = false) 
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
            where T4 : CSODataComponent
        {
             return cso.GetCollection<T1, T2, T3, T4>(onlySon);
        }
        
          public CSOCollection GetCollection<T1, T2, T3, T4, T5>(bool onlySon = false) 
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
            where T4 : CSODataComponent
            where T5 : CSODataComponent
        {
             return cso.GetCollection<T1, T2, T3, T4, T5>(onlySon);
        }
        
        public CSOCollection GetCollection(CSOMatcher matcher,bool onlySon = false)
        {
            return cso.GetCollection(matcher,onlySon);
        }
        #endregion


        #region Evt
        public void SendEvt(int evt,params object[] args)
        {
           cso.SendEvt(evt,args);
        }
        public void AddEvtListener(int msgId,Action<int,object[]> action)
        {
            cso.AddEvtListener(msgId,action);
        }
        
        
        public void RemoveEvtListener(int msgId,Action<int,object[]> action)
        {
            cso.RemoveEvtListener(msgId,action);
        }
        #endregion


        #region UniqKey

         public CObject GetChildByKey<T>(string id) where T: UniqKeyComponent
        {
            return cso.GetChildByKey<T>(id);
        }
        public CObject GetChildByKey<T>(int id) where T: UniqKeyComponent
        {
            return cso.GetChildByKey<T>(id);
        }

        #endregion


        #region  View

        public void ShowView()
        {
            cso.ShowView();
        }

        public void HideView()
        {
             cso.HideView();
        }

        public void DestroyView()
        {
             cso.DestroyView();
        }

        // public void RebuildView()
        // {
        //      cso.RebuildView();
        // }


        #endregion
        
    }
}

