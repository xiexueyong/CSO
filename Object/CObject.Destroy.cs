using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Group;
using CSOEngine.Utils.Pool;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        public void Destroy()
        {
            if (Destroyed)
            {
                return;
            }
            parent_group?.Remove(this);
            destroy_actual();
            CSOPool.Release(this);
            OnDestroy?.Invoke();
        }
        
        internal void destroy_actual()
        {
            //state
            stateMachine?.OnDestroy();
            stateMachine = null;
            
            //ability
            abilityContainer?.Destroy();
            abilityContainer = null;
            
            //children_group
            children_group?.Destroy();
            CSOPool.Release(children_group);
            children_group = null;
            
            //component
            componentContainer?.Destroy();
            componentContainer = null;

            Destroyed = true;
            Activated = false;
            parent = null;
            parent_group = null;
            children_group = null;
        }
    }
}