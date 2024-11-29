using System.Collections.Generic;
using CSOEngine.Object;
using CSOEngine.Utils;

namespace CSOEngine.Component
{
    public abstract class CSOEvtComponent:CSOComponent
    {
        private List<int> intrestEvts;
        public void AddEvt(int evtId)
        {
            if (intrestEvts == null)
            {
                intrestEvts = new List<int>();
            }

            if (!intrestEvts.Contains(evtId))
            {
                intrestEvts.Add(evtId);
            }
        }
        public void RemoveEvt(int evtId)
        {
            if (intrestEvts != null && intrestEvts.Contains(evtId))
            {
                intrestEvts.Remove(evtId);
            }
        }

        internal bool checkIntrestEvt(int evtId)
        {
            if (intrestEvts == null)
            {
                return false;
            }
            return intrestEvts.Contains(evtId);
        }

        internal void ReceiverEvt(int evtId,params object[] args)
        {
            OnReceiveEvt(evtId,args);
        }
        protected virtual void OnReceiveEvt(int evtId,params object[] args)
        {
            
            
        }
    }
}