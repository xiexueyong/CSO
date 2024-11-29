using System;
using CSOEngine.Component;
using CSOEngine.Group;
using CSOEngine.Group.DirtyMatcher;
using CSOEngine.Object;
using CSOEngine.State;
using UnityEngine;

namespace CSOEngine.Proxy
{
    public partial class CSOProxy:ICObject
    {
        public T AddContext<T>()where T:CSOContextComponent,new()
        {
            return cso.AddContext<T>();
        }

        public CSOContextComponent AddContext(CSOContextComponent contextComponent)
        {
            return cso.AddContext(contextComponent);
        }

        public T GetContext<T>()where T:CSOContextComponent
        {
            return cso.GetContext<T>();
        }
        
    }
}

