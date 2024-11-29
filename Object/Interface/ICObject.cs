using System;
using CSOEngine.Component;
using CSOEngine.Group;
using CSOEngine.State;

namespace CSOEngine.Object
{
    public partial interface ICObject
    {
        public Type GetCurState();
        
        public void ShowView();
        public void HideView();
        public void DestroyView();
        // public void RebuildView();

        public CObject AddState<T>(bool isDestroyState = false) where T : CSOState, new();
        public void SetFirstState<T>(params object[] args);
        
        public T GetParentComp<T>() where T : CSODataComponent;

        public T GetOrAddComp<T>(bool fromBuilder = false) where T : CSODataComponent, new();

        public T AddComp<T>(bool fromBuilder = true) where T : CSODataComponent, new();

        public CSODataComponent AddComp(CSODataComponent component, bool fromBuilder = true);

        public T GetComp<T>() where T : CSODataComponent;
        public void RemoveComp<T>() where T : CSODataComponent;

        public void RemoveComp(CSODataComponent component);

        public bool HasComp<T>();

        public bool HasComp(Type type);


        #region Ability

        public T AddAbility<T>(bool fromBuilder = true) where T : CSOAbilityComponent, new();

        public CSOAbilityComponent AddAbility(CSOAbilityComponent abilityComponent, bool fromBuilder);

        public CSOAbilityComponent RemoveAbility<T>() where T : CSOAbilityComponent;

        public T GetAbility<T>() where T : CSOAbilityComponent;

        public T GetAbilityByBaseClass<T>() where T : CSOAbilityComponent;

        public bool HasAbility<T>();

        #endregion

        #region Group Collection
        public CSOCollection GetCollection();
        public CSOCollection GetCollection<T>(bool onlySon=false) where T : CSODataComponent;

        public CSOCollection GetCollection<T1, T2>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent;

        public CSOCollection GetCollection<T1, T2, T3>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent;

        public CSOCollection GetCollection<T1, T2, T3, T4>(bool onlySon = false) 
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
            where T4 : CSODataComponent;
        
           public CSOCollection GetCollection<T1, T2, T3, T4, T5>(bool onlySon = false) 
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
            where T4 : CSODataComponent
            where T5 : CSODataComponent;
        
        public CSOCollection GetCollection(CSOMatcher matcher,bool onlySon = false);
        #endregion


        #region Evt
        public void SendEvt(int evt,params object[] args);
        public void AddEvtListener(int msgId,Action<int,object[]> action);


        public void RemoveEvtListener(int msgId, Action<int, object[]> action);
        #endregion


        #region UniqKey

        public CObject GetChildByKey<T>(string id) where T : UniqKeyComponent;
        public CObject GetChildByKey<T>(int id) where T : UniqKeyComponent;

        #endregion

        #region Context
        public T AddContext<T>() where T : CSOContextComponent, new();
        public CSOContextComponent AddContext(CSOContextComponent contextComponent);
        public T GetContext<T>() where T : CSOContextComponent;
        #endregion

    }
}