using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public sealed partial class CSOGroup
    {
        internal CObject OwnerCSO;
        public CSOGroup()
        {
            theAllCollection = new CSOCollection(true);
            matcherCollections = new List<CSOCollection>();
        }

        internal void Active(bool active)
        {
            foreach (var item in theAllCollection.Objects)
            {
                item.Active(active);
            }
        }
    }
}