using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.State;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject:ICObject
    {
        public void Active(bool value)
        {
            if (Activated == value || Destroyed)
            {
                return;
            }
            Activated = value;
            
            //component
            componentContainer?.Active(value);

            //_abilities
            abilityContainer?.Active(value);
            
            //_views
            viewContainer?.Active(value);
            
            //state
            stateMachine?.Active(value); 
            
            //children
            children_group?.Active(value);
            
            OnActive?.Invoke(value);
        }

     
    }
}