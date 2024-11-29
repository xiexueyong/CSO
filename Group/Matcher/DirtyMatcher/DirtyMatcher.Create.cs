using System;
using System.Collections.Generic;
using CSOEngine.Component;

namespace CSOEngine.Group.DirtyMatcher
{
    public partial class DirtyMatcher
    {
        // public static DirtyMatcher Create()
        // {
        //     return new DirtyMatcher();
        // }
        
        public static DirtyMatcher AnyOf<T>()where T:CSODirtyComponent
        {
            var matcher = new DirtyMatcher();
            matcher.anyof = new HashSet<Type>{typeof(T)};
            return matcher;
        }
        public static DirtyMatcher AnyOf<T1,T2>()where T1:CSODirtyComponent where T2:CSODirtyComponent
        {
            var matcher = new DirtyMatcher();
            matcher.anyof = new HashSet<Type>{typeof(T1),typeof(T2)};
            return matcher;
        }
        public static DirtyMatcher AnyOf<T1,T2,T3>()where T1:CSODirtyComponent where T2:CSODirtyComponent where T3:CSODirtyComponent
        {
            var matcher = new DirtyMatcher();
            matcher.anyof = new HashSet<Type>{typeof(T1),typeof(T2),typeof(T3)};
            return matcher;
        }
        public static DirtyMatcher AnyOf<T1,T2,T3,T4>()where T1:CSODirtyComponent where T2:CSODirtyComponent where T3:CSODirtyComponent where T4:CSODirtyComponent
        {
            var matcher = new DirtyMatcher();
            matcher.anyof = new HashSet<Type>{typeof(T1),typeof(T2),typeof(T3),typeof(T4)};
            return matcher;
        }
        public static DirtyMatcher AnyOf<T1, T2, T3, T4,T5>() where T1 : CSODirtyComponent where T2 : CSODirtyComponent where T3 : CSODirtyComponent where T4 : CSODirtyComponent where T5 : CSODirtyComponent
        {
            var matcher = new DirtyMatcher();
            matcher.anyof = new HashSet<Type> { typeof(T1), typeof(T2), typeof(T3), typeof(T4) , typeof(T5)};
            return matcher;
        }

    }
}