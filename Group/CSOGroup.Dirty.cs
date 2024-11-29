using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public partial class CSOGroup
    {
        internal void ProcessCompDirty()
        {
            foreach (var item in theAllCollection.Objects)
            {
                item.ProcessCompDirty();
            }
        }
    }
}