using System;
using System.Collections.Generic;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public partial class CSOMatcher
    {
        private CSOMatcher()
        {
        }
        public static CSOMatcher Create()
        {
            return new CSOMatcher();
        }

        public CSOMatcher AllOf(HashSet<Type> types)
        {
            this.allof = types;
            return this;
        }
        public CSOMatcher AnyOf(HashSet<Type> types)
        {
            this.anyof = types;
            return this;
        }
        public CSOMatcher NoneOf(HashSet<Type> types)
        {
            this.allof = types;
            return this;
        }
        public CSOMatcher Filter(Func<CObject,bool> filter)
        {
            this.filter = filter;
            return this;
        }
        public CSOMatcher OnlySon(bool onlySon)
        {
            this.onlySon = onlySon;
            return this;
        }
        
        public CSOMatcher AllOf<T1>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            this.allof = types;
            return this;
        }
        public CSOMatcher AllOf<T1,T2>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            this.allof = types;
            return this;
        }

        public CSOMatcher AllOf<T1,T2,T3>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            this.allof = types;
            return this;
        }
        
        public CSOMatcher AllOf<T1,T2,T3,T4>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            types.Add(typeof(T4));
            this.allof = types;
            return this;
        }
        public CSOMatcher AllOf<T1, T2, T3, T4,T5>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            types.Add(typeof(T4));
            types.Add(typeof(T5));
            this.allof = types;
            return this;
        }
        // AnyOf with 1 to 4 generic types
        public CSOMatcher AnyOf<T1>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            this.anyof = types;
            return this;
        }

        public CSOMatcher AnyOf<T1, T2>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            this.anyof = types;
            return this;
        }

        public CSOMatcher AnyOf<T1, T2, T3>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            this.anyof = types;
            return this;
        }

        public CSOMatcher AnyOf<T1, T2, T3, T4>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            types.Add(typeof(T4));
            this.anyof = types;
            return this;
        }
        
        
        // AnyOf with 1 to 4 generic types
        public CSOMatcher NoneOf<T1>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            this.noneof = types;
            return this;
        }

        public CSOMatcher NoneOf<T1, T2>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            this.noneof = types;
            return this;
        }

        public CSOMatcher NoneOf<T1, T2, T3>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            this.noneof = types;
            return this;
        }

        public CSOMatcher NoneOf<T1, T2, T3, T4>()
        {
            HashSet<Type> types = new HashSet<Type>();
            types.Add(typeof(T1));
            types.Add(typeof(T2));
            types.Add(typeof(T3));
            types.Add(typeof(T4));
            this.noneof = types;
            return this;
        }
        
        
        
    




    }
}