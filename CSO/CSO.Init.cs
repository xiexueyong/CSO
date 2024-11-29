using System;
using System.Collections.Generic;
using CSOEngine.Object;
using CSOEngine.Utils.Pool;
using UnityEngine;

namespace CSOEngine {
  

    /// <summary>
    /// CSObject工厂类
    /// </summary>
    public  static partial class CSO 
    {
        private static bool Inited;
        private static Transform CSOLinkerRootTransform;
        private static Transform CSOViewRootTransform;
        public static void Init()
        {
            if (!Inited)
            {
                Inited = true;
                var csoRoot = new GameObject("CSORoot");
                csoRoot.AddComponent<CSOMono>();
                GameObject.DontDestroyOnLoad(csoRoot);
                
                //create cso linker container
                var csoLinkers = new GameObject("CSOLinkers");
                csoLinkers.transform.parent = csoRoot.transform;
                CSOLinkerRootTransform = csoLinkers.transform;
                
                //create cso view container
                var csoViews = new GameObject("CSOViews");
                csoViews.transform.parent = csoRoot.transform;
                CSOViewRootTransform = csoViews.transform;
            }
        }
        
        
        private class CSOMono:MonoBehaviour
        {
            private void Update()
            {
                CSO.Update();
            }
        }
    }
}