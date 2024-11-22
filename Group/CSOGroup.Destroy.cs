using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Group
{
    public partial class CSOGroup
    {
        internal void Destroy()
        {
            //destroy all objects
            foreach (var item in theAllCollection.ObjectList)
            {
                item.Destroy();
            }
            
            theAllCollection.Clear();
            matcherCollections?.Clear();
            _uniqKeys?.Clear();
            
            theAllCollection = null;
            matcherCollections = null;
            _uniqKeys = null;
            OwnerCSO = null;
        }
    }
}