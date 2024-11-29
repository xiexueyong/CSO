using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{

    public partial class CSOGroup
    {
       
        /// <summary>
        /// 不通过base.Update循环，因为要控制CSOGroup在第一个执行
        /// </summary>
        internal void OnUpdate()
        {
            if (theAllCollection != null && theAllCollection.Objects != null)
            {
                foreach (var item in theAllCollection.Objects)
                {
                    item.OnUpdate();
                }
            }
        }
    }
}