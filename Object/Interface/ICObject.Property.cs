using System;
using CSOEngine.Component;
using CSOEngine.Group;

namespace CSOEngine.Object
{
    public partial interface ICObject
    {
        public bool Activated { get;  }
        public bool Destroyed{ get; }


        public void Destroy();
        public void Active(bool value);

    }
}