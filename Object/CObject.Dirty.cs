using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using CSOEngine.Group.DirtyMatcher;
using UnityEngine;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        private Dictionary<DirtyMatcher, List<Action<CSODataComponent[],CSODirtyComponent.DirtyType>>> dirtyNotifiers;
        
        internal void ProcessCompDirty()
        {
            //cso dirty notify
            if (dirtyNotifiers != null && dirtyNotifiers.Count > 0)
            {
                foreach (var dirtyNotifier in dirtyNotifiers)
                {
                    if (dirtyNotifier.Key.CheckDirty(this,out var components))
                    {
                        dirtyNotifier.Value?.ForEach(x=>
                        {
                            try
                            {
                                x?.Invoke(components,CSODirtyComponent.DirtyType.Dirty);
                            }
                            catch (Exception e)
                            {
                                Debug.LogError(e);
                            }
                        });
                    }
                }   
            }
            
            //recover dirty and notify
            if (componentContainer != null && componentContainer.components.Count > 0)
            {
                foreach (var item in componentContainer.components)
                {
                    if (item.Value is CSODirtyComponent dirtyComp && dirtyComp.dirty)
                    {
                        dirtyComp.dirty = false;
                        dirtyComp.DirtyNotify(CSODirtyComponent.DirtyType.Dirty);
                    } 
                };
            }
            
            //recursive process children dirty comps
            children_group?.ProcessCompDirty();
        }


        public void AddDirtyListener(DirtyMatcher matcher,  Action<CSODataComponent[], CSODirtyComponent.DirtyType> action)
        {
            if (dirtyNotifiers == null)
                dirtyNotifiers = new();
            if (!dirtyNotifiers.ContainsKey(matcher))
            {
                dirtyNotifiers[matcher] = new List<Action<CSODataComponent[], CSODirtyComponent.DirtyType>>();
            }
            dirtyNotifiers[matcher].Add(action);
        }
        
        public void RemoveDirtyListener(DirtyMatcher matcher,  Action<CSODataComponent[], CSODirtyComponent.DirtyType> action)
        {
            if (dirtyNotifiers == null)
                return;
            if (!dirtyNotifiers.ContainsKey(matcher))
            {
                return;
            }
            dirtyNotifiers[matcher].Remove(action);
        }

        private void tryTriggerDirtyNotifierFromAddOrRemove(CSODataComponent component,CSODirtyComponent.DirtyType dirtyType)
        {
            if(dirtyNotifiers == null)
            {
                return;
            }
            foreach (var dirtyNotifier in dirtyNotifiers)
            {
                if (dirtyNotifier.Key.CheckContaine(component))
                {
                    dirtyNotifier.Value?.ForEach(x=>
                    {
                        try
                        {
                            x?.Invoke(new []{component},dirtyType);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError(e);
                        }
                    });
                }
            } 
        }
    }
}