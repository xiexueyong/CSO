using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        internal CObject parent;

        public T GetParentComp<T>() where T : CSODataComponent
        {
            return parent?.GetComp<T>();
        }
        
        public Type GetCurState()
        {
            return  stateMachine?.GetCurState();
        }

        public Type GetParentCurState()
        {
            return parent?.stateMachine?.GetCurState();
        }
        
        internal CObject GetRootCSO()
        {
            var root = this;
            while (!root.isRoot)
            {
                root = root.parent;
            }

            return root;
        }
    }
}