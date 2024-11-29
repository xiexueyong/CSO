using CSOEngine.State;
using System;

namespace CSOEngine.Object
{
    public interface ICOState
    {
        public void EnterState<T>(params object[] args) where T : CSOState;

       
    }
}