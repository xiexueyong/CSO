using System;
using System.Collections.Generic;
using System.Linq;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group.DirtyMatcher
{
    public partial class DirtyMatcher
    {
            public override bool Equals(object obj)
            {
                if (ReferenceEquals(this, obj))
                    return true;

                if (obj == null || GetType() != obj.GetType())
                    return false;

                var other = (DirtyMatcher)obj;

                if (anyof == null && other.anyof == null)
                    return true;
                if (anyof == null || other.anyof == null)
                    return false;
        
                return anyof.SetEquals(other.anyof);
            }

            public override int GetHashCode()
            {
                return GetHashSetHashCode<Type>(anyof);
            }
            
            
            private int GetHashSetHashCode<T>(HashSet<T> set)
            {
                if (set == null) return 0;
                int hashCode = 0;
                foreach (var item in set)
                {
                    hashCode = hashCode ^ (item?.GetHashCode() ?? 0);
                }
                return hashCode;
            }
        
        }
        
    
}