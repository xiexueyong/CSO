using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Group
{
    public partial class CSOGroup:ICSOPoolObject
    {
        public void OnPOActivate()
        {
        }
        public void OnPODeactivate()
        {
        }
        public bool InPool { get; set; }
        public bool UseDefaultReset => false;
    }
}