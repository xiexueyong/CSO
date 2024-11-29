using System;
using System.Collections.Generic;
using CSOEngine.Object;
using CSOEngine.Utils.Pool;
using UnityEngine;

namespace CSOEngine {
  

    /// <summary>
    /// CSObject工厂类
    /// </summary>
    public  static partial class CSO {

        private static readonly List<Action> mOnUpdates = new List<Action>();
        private static readonly List<Action> mTempInvokeActions = new List<Action>();
        
        private static void Update()
        {
            CSORootUpdate();
            InvokeActions(mOnUpdates);
        }
        
        public static void RegisterUpdate(Action callback)
        {
            mOnUpdates.Add(callback);
        }
        public static void UnRegisterUpdate(Action callback) { mOnUpdates.Remove(callback); }
        
        /// <summary>
        ///Call callback
        /// </summary>
        /// <param name="actions"></param>
        private static void InvokeActions(List<Action> actions)
        {
            mTempInvokeActions.AddRange(actions);
            for (int i = 0,  ci= mTempInvokeActions.Count; i < ci; ++i)
            {
                try
                {
                    mTempInvokeActions[i].Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    Debug.LogError(e.StackTrace);
                }
            }
            mTempInvokeActions.Clear();
        }
    }
}