using System.Collections.Generic;
using System;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;

namespace CSOEngine.State
{
    public sealed class CSOStateMachine:ComponentsContainer
    {
        private Dictionary<Type,CSOState> _states;
        private CSOState _curState;
        
        private Type _firstStateType;
        private Type _destroyStateType;
        private object[] _firstArgs;


        internal void AddState<T>(bool isDestroyState = false) where T:CSOState
        {
            var s = Activator.CreateInstance<T>();
            AddState(typeof(T),s,isDestroyState);
        }

        internal CSOState AddState(Type type,CSOState state, bool isDestroyState = false)
        {
            if (_states == null && !isDestroyState)
            {
                _states = new Dictionary<Type, CSOState>();
                _firstStateType = type;
            }
            _states[type] = state;

            if (isDestroyState)
            {
                _destroyStateType = type; 
            }

            return state;
        }
      
        internal void EnterState<T>(params object[] args) {
            if (_states != null && _states.TryGetValue(typeof(T),out var s) && s != null)
            {
                _curState?.Exit();
                _curState = s;
                _curState.Enter(args);
            }
        }
        
        public bool IsActive{get;private set;}
    
        internal void Active(bool value)
        {
            if (IsActive != value)
            {
                IsActive = value;
                OnActive(value);
            }
        }

        
        private void OnActive(bool active)
        {
            if (_curState == null && active)
            {
                _states.TryGetValue(_firstStateType, out var s);
                s.Enter(_firstArgs);
            }
        }

        public void OnDestroy()
        {
            if (_states == null || _states.Count == 0)
            {
                return;
            }
            if (_curState != null)
            {
                _curState?.Exit();
                _curState = null;
            }

            if (_destroyStateType != null && _states.TryGetValue(_destroyStateType, out var destroyState))
            {
                destroyState.Enter();
            }
            
            //回收
            foreach (var state in _states)
            {
                CSOPool.Release(state.Value);
            }
            _states.Clear();
            
        }

        internal void Update()
        {
            _curState?.Update();
        }

        internal Type GetCurState()
        {
            return  _curState?.GetType();
        }

        internal void SetFirstState<T>(params object[] args)
        {
            _firstStateType = typeof(T);
            _firstArgs = args;
        }
    }
}