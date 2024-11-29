using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Object;

namespace CSOEngine.Group
{
    public partial class CSOGroup
    {
        private List<CSOCollection> matcherCollections;
        private CSOCollection theAllCollection;
      
        /// <summary>
        /// 移除组件时，尝试从所有的collection里移除对应的cso
        /// </summary>
        /// <param name="cObject"></param>
        internal void TryRemoveFromMatcherCollection(CObject cObject)
        {
            if (matcherCollections != null)
            {
                foreach (var collection in matcherCollections)
                {
                    collection.TryRemvoe(cObject);
                }
            }
            
            OwnerCSO.parent?.ChildrenGroup.TryRemoveFromMatcherCollection(cObject );
        }
        /// <summary>
        /// 创建时：直接添加；添加组件时：不添加到theAll，只尝试加到matcherCollection
        /// </summary>
        /// <param name="cObject"></param>
        /// <param name="fromCreate"></param>
        internal void Add(CObject cObject,bool fromCreate)
        {
            //添加到theAll
            if (fromCreate)
            {
                theAllCollection.Add(cObject);
            }
            //添加到matcherCollections
            if (matcherCollections != null)
            {
                bool isSon = cObject.parent == OwnerCSO;
                foreach (var collection in matcherCollections)
                {
                    collection.TryAdd(cObject,isSon);
                } 
            } 
            
            OwnerCSO.parent?.ChildrenGroup.Add(cObject, fromCreate );
        }
        
        ///cso.destroy时，从所有collection里移除，从uniqKey里移除
        internal void Remove(CObject cObject)
        {
            TryRemoveUniqKey(cObject);
            
            theAllCollection.Remove(cObject);

            if (matcherCollections != null && matcherCollections.Count > 0)
            {
                foreach (var collection in matcherCollections)
                {
                    collection.Remove(cObject);
                }
            }
            
            OwnerCSO.parent?.ChildrenGroup.Remove(cObject);
        }
    }
}