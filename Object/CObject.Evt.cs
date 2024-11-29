using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        private List<Tuple<int, object[]>> evts1 = new List<Tuple<int, object[]>>();
        private List<Tuple<int, object[]>> evts2 = new List<Tuple<int, object[]>>();

        private void ProcessEvts()
        {
            evts2.AddRange(evts1);
            evts1.Clear();

            for (int k = 0; k < evts2.Count; k++)
            {
                ProcessOneEvt(evts2[k].Item1,evts2[k].Item2);
            }
            evts2.Clear();
        }

        private void ProcessOneEvt(int evt,params object[] args)
        {
            //components evt
            if (abilityContainer != null)
            {
                var cs = abilityContainer.GetComps();
                if (cs != null)
                {
                    foreach (var item in cs)
                    {
                        var e = item.Value as CSOAbilityComponent;
                        if (e.checkIntrestEvt(evt))
                        {
                            try
                            {
                                e.ReceiverEvt(evt, args);
                            }
                            catch (Exception exception)
                            {
                                Debug.LogError(exception);
                                Debug.LogError(exception.StackTrace);
                            }
                        }
                    }
                }
            }
            
            if (registerEvts!= null && registerEvts.TryGetValue(evt,out var actions))
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i]?.Invoke(evt,args);
                }
            }
        }
        /// <summary>
        /// 发送的事件不会立即执行，而是在一帧的结尾执行。防止循环发送事件，导致死循环。
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="args"></param>
        public void SendEvt(int evt,params object[] args)
        {
            if (evts1 == null || evts2 == null)
            {
                evts1 = new List<Tuple<int, object[]>>();
                evts2 = new List<Tuple<int, object[]>>();
            }
            evts1.Add(Tuple.Create(evt,args));
        }


        private Dictionary<int, List<Action<int,object[]>>> registerEvts;
        public void AddEvtListener(int msgId,Action<int,object[]> action)
        {
            if (registerEvts == null)
            {
                registerEvts = new Dictionary<int, List<Action<int,object[]>>>();
            }

            if (!registerEvts.ContainsKey(msgId))
            {
                registerEvts[msgId] = new List<Action<int,object[]>>();
            }
            
            if(!registerEvts[msgId].Contains(action))
                registerEvts[msgId].Add(action);
        }
        
        
        public void RemoveEvtListener(int msgId,Action<int,object[]> action)
        {
            if (registerEvts == null)
            {
                return;
            }

            if (!registerEvts.ContainsKey(msgId))
            {
                return;
            }
            if(registerEvts[msgId].Contains(action))
                registerEvts[msgId].Remove(action);
        }
       
    }
}