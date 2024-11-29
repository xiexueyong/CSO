using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;

namespace CSOEngine.Component
{
    public partial class ComponentsContainer
    {
        /// <summary>
        /// 根据基类获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public  T GetCompByBaseClass<T>()where T: CSOBaseComponent
        {
            if (components == null || components.Count == 0)
            {
                return null;
            }

            foreach (var item in components)
            {
                if (item.Value is T)
                {
                    return item.Value as T;
                }
            }

            return null;
        }
    }
}