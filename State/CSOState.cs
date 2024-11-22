using System;
using CSOEngine.Component;
using CSOEngine.Object;
using UnityEngine;

namespace CSOEngine.State
{
    public class CSOState
    {
        public CSOState()
        {
            
        }
        internal CObject ___cso;
        protected CObject cso => ___cso;
       

        protected void EnterState<T>(params object[] args) where T:CSOState
        {
            cso.EnterState<T>(args);
        }
        public T GetStateComp<T>()where T: CSODataComponent
        {
            return cso.GetStateComp<T>();
        }
        public void AddStateComp<T>()where T: CSODataComponent,new()
        {
            cso.AddStateComp<T>();
        }
        
        internal void  Enter(params object[] args)
        {
            try
            {
                OnEnter(args);
            }
            catch (Exception e)
            {
                // Debug.LogError(e.StackTrace);
                Debug.LogError(e);
            }
        }

        internal void Exit()
        {
            try
            {
                OnExit();
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError(e);
            } 
        }

        internal void Update()
        {
            try
            {
                OnUpdate();
            }
            catch (Exception e)
            {
                Debug.LogError(e.StackTrace);
                Debug.LogError(e);
            }
        }

        protected virtual void OnEnter(params object[] args)
        {
            
        }
        protected virtual void OnExit()
        {
            
        }
        protected virtual void OnUpdate()
        {
            
        }

    }
}