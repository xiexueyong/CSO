using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public partial class CSOGroup
    {
        public CSOCollection GetCollection()
        {
            return theAllCollection;
        }
        
        internal CSOCollection GetCollection(CSOMatcher matcher,bool onlySon = false)
        {
            if (onlySon)
            {
                matcher.OnlySon(onlySon);
            }
            //已存在
            for (int i = 0; i < matcherCollections.Count; i++)
            {
                if (matcherCollections[i].sameMatcher(matcher))
                {
                    return matcherCollections[i];
                }
            }
            CSOCollection newCollection = new CSOCollection(false);
            newCollection.SetMatcher(matcher);
            matcherCollections.Add(newCollection);
            newCollection.fillCollection(theAllCollection,OwnerCSO);
            return newCollection;
        }
    }
}