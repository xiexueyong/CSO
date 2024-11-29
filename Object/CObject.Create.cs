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
      
        internal CObject createChildObj()
        {
            if (Destroyed)
            {
                throw new Exception("CObject.CreateChildObj  :CObject has been destroyed");
            }
            CObject obj = CSOPool.Get<CObject>();
            obj.parent_group = ChildrenGroup;
            obj.parent = this;
            obj.isRoot = false;
            return obj;
        }
        internal static CObject createObj()
        {
            CObject obj = CSOPool.Get<CObject>();
            obj.isRoot = true;
            return obj;
        }
    }
}