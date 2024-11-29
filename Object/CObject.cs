using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Proxy;
using CSOEngine.State;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject:ICObject
    {
        public CObject()
        {
        }
        internal CSOStateMachine stateMachine;
        
        private  bool isRoot;
        public bool Activated { get; private set; }
        public bool Destroyed{ get; private set; }
        private CSOProxy _csoProxy;

        public CSOProxy Proxy => _csoProxy ??= new CSOProxy(this);
    }
}