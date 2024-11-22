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
        internal void RootUpdate()
        {
            OnUpdate();
            
            //清理dirty
            ProcessCompDirty();
        }
        internal void OnUpdate()
        {
            children_group?.OnUpdate();
            
            //ability, component作为数据不会update
            if (abilityContainer != null)
            {
                var cs = abilityContainer.GetComps();
                if (cs != null)
                {
                    foreach (var item in cs)
                    {
                        try
                        {
                            (item.Value as CSOAbilityComponent)?.Update();
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }  
                    }
                }
            }
            
            //state
            try
            {
                stateMachine?.Update();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            
            
            //处理事件，在结尾处理因为防止事件循环
            ProcessEvts();
        }
    }
}