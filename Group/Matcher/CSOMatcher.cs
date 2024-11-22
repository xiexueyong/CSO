using System;
using System.Collections.Generic;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public partial class CSOMatcher
    {
        internal HashSet<Type> allof;
        internal HashSet<Type> anyof;
        internal HashSet<Type> noneof;

        private Func<CObject,bool> filter;

        private bool onlySon;

        private bool checkAllOf(CObject cso)
        {
            if (allof == null || allof.Count == 0)
            {
                return true;
            }
            foreach (var type in allof)
            {
                if (!cso.HasComp(type))
                {
                    return false;
                }
            }
            return true;
        }
        private bool checkAnyOf(CObject cso)
        {
            if (anyof == null || anyof.Count == 0)
            {
                return true;
            }
            foreach (var type in anyof)
            {
                if (cso.HasComp(type))
                {
                    return true;
                }
            }
            return false;
        }
        private bool checkNoneOf(CObject cso)
        {
            if (noneof == null || noneof.Count == 0)
            {
                return true;
            }
            foreach (var type in noneof)
            {
                if (cso.HasComp(type))
                {
                    return false;
                }
            }
            return true;
        }
        private bool checkFilter(CObject cso)
        {
            if (filter == null)
                return true;
            
            return filter.Invoke(cso) ;
        }

        private bool checkOnlySon(bool isSon)
        {
            if (!onlySon)
            {
               return true; 
            }

            return isSon;
        }

        internal bool Check(CObject cso ,bool isSon)
        {
            return checkAllOf(cso) && checkAnyOf(cso) && checkNoneOf(cso) && checkFilter(cso) && checkOnlySon(isSon);
        }
        
        
        

    }
}