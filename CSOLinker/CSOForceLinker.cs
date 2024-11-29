using System;
using CSOEngine.Component;
using CSOEngine.Object;
using UnityEngine;

namespace CSOEngine
{
    [DisallowMultipleComponent]
    public class CSOForceLinker:CSOLinker
    {
        private bool destroyed;
        public GameObject ViewGameobject;
         
        private void Awake()
        {
            gameObject.SetActive(false);
        }
        
        protected override void OnActive(bool value)
        {
            gameObject.SetActive(value);
        }
           
        protected override void OnCSObjectDestroy()
        {
            clearCSOEvt();
            csobject = null;
            Destroy(this.gameObject);//linker gameobject
            Destroy(this.ViewGameobject);// view gameobjecct
        }

        private void OnDestroy()
        {
            if (csobject != null && !csobject.Destroyed)
            {
                Destroy(this.gameObject);//linker gameobject
                Destroy(this.ViewGameobject);// view gameobjecct
                clearCSOEvt();
                csobject?.Destroy();
                csobject = null;
            }
        }
    }
}