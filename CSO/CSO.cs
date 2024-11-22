using System;
using CSOEngine.Object;
using JetBrains.Annotations;
using CSOEngine.Utils.Pool;
using UnityEngine;

namespace CSOEngine {
    /// <summary>
    /// CSObject工厂类
    /// </summary>
    public  static partial class CSO {

        private static void CSORootUpdate()
        {
            for (int i = csObjects.Count - 1; i >= 0; i--)
            {
                var cso = csObjects[i];
                if (cso.Destroyed)
                {
                    csObjects.RemoveAt(i);
                    continue;
                }
                if (cso.Activated)
                {
                    csObjects[i].RootUpdate();
                }
            }
        }
    }
}