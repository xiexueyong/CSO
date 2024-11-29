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
        [Obsolete("请在ViewComponent内实现 view相关的功能，CSOViewComponent.Transform/RootTransform")]
        public Transform Transform { get; internal set; }
        internal Transform LinkerTransform { get; set; }


        [Obsolete("请在ViewComponent内实现 view相关的功能，CSOViewComponent.Transform/RootTransform")]
        public Transform RootTransform
        {
            get
            {
                return GetRootCSO().Transform;
            }
        }
       
    }
}