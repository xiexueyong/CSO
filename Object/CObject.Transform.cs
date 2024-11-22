using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Group;

using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        public Transform Transform { get; internal set; }
        internal Transform LinkerTransform { get; set; }


        public Transform RootTransform
        {
            get
            {
                return GetRootCSO().Transform;
            }
        }
       
    }
}