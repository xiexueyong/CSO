using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.State;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        public CObject()
        {
            
        }
        internal CSOStateMachine stateMachine;
        
        private  bool isRoot;
        public bool Activated { get; private set; }
        public bool Destroyed{ get; private set; }

 
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
            
            //state
            stateMachine?.Active(value); 
            
            //children
            children_group?.Active(value);
            
            OnActive?.Invoke(value);
        }

     
    }
}