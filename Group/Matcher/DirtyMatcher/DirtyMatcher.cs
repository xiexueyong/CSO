using System;
using System.Collections.Generic;
using System.Linq;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group.DirtyMatcher
{
    public partial class DirtyMatcher
    {
        internal HashSet<Type> anyof;
        
    

        // private void AnyOf(HashSet<Type> types)
        // {
        //     this.anyof = types;
        // }

        public bool CheckDirty(CObject obj,out CSODirtyComponent[] components)
        {
            if (anyof == null || anyof.Count==0)
            {
                components = null;
                return false;
            }
            foreach (var type in anyof)
            {
                var comp = obj.GetComp(type);
                if (comp != null && comp is CSODirtyComponent dirtyComponent && dirtyComponent.dirty)
                {
                    cacheDirtyComps(dirtyComponent);
                }
            }

            if (tempDirtyComponents != null && tempDirtyComponents.Count > 0)
            {
                components = tempDirtyComponents.ToArray();
                tempDirtyComponents = null;
                return true;
            }
            else
            {
                components = null;
                return false;
            }
        }

        private List<CSODirtyComponent> tempDirtyComponents;
        private void cacheDirtyComps(CSODirtyComponent dirtyComponent)
        {
            if (tempDirtyComponents == null)
                tempDirtyComponents = new List<CSODirtyComponent>();
            tempDirtyComponents.Add(dirtyComponent);

        }
        
        public bool CheckContaine(CSODataComponent component)
        {
            if (anyof == null || anyof.Count==0)
            {
                return false;
            }

            var compType = component.GetType();
            foreach (var type in anyof)
            {
                if (type == compType)
                {
                    return true;
                }
            }
            return false;
        }
        
        
    }
}