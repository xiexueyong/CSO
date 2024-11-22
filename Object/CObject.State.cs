using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.State;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Object
{
    public sealed partial class CObject
    {
        public  Type BuilderType;//关联的构造器类型 调试用
        public void EnterState<T>(params object[] args) where T:CSOState
        {
            stateMachine?.EnterState<T>(args);
        }
        
        public CObject AddState<T>(bool isDestroyState=false) where T:CSOState,new()
        {
            if (stateMachine == null)
                stateMachine = new CSOStateMachine();
            
            // var s = Activator.CreateInstance<T>();
            var state = CSOPool.Get<T>();
            AddState(typeof(T), state, isDestroyState);
            return this;
        }

        private CSOState AddState(Type type, CSOState state, bool isDestroyState = false)
        {
            if (stateMachine == null)
                stateMachine = new CSOStateMachine();

            if (state is CSOState)
            {
                (state as CSOState).___cso = this;
            }

            return stateMachine?.AddState(type, state, isDestroyState);
        }

        public void SetFirstState<T>(params object[] args)
        {
            if (stateMachine == null)
            {
                stateMachine = new CSOStateMachine();
            }
            
            stateMachine.SetFirstState<T>(args);
        }

        //给状态机添加组件，用于在状态之间共享数据、记录数据
        public T AddStateComp<T>()where T: CSODataComponent,new()
        {
            if (stateMachine == null)
            {
                stateMachine = new CSOStateMachine();
            }

            return stateMachine.AddComp<T>();
        }

        public T GetStateComp<T>()where T: CSODataComponent
        {
            return stateMachine.GetComp<T>();
        }

       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}