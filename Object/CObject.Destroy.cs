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
        private bool _destroying;
        public void Destroy()
        {
            if (Destroyed || _destroying)
            {
                return;
            }
            _destroying = true;
            parent_group?.Remove(this);
           
            //view
            viewContainer?.Destroy();
            viewContainer = null;
            
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
            
            
            OnDestroy?.Invoke();
            CSOPool.Release(this);
        }
    }
}