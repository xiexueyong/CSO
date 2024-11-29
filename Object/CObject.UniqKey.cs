using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        public CObject GetChildByKey<T>(string id) where T: UniqKeyComponent
        {
            int hashCode = UniqKeyComponent.GetHashCode(-1,id, typeof(T));
            return getChildByHashCode(hashCode);
        }
        public CObject GetChildByKey<T>(int id) where T: UniqKeyComponent
        {
            int hashCode = UniqKeyComponent.GetHashCode(id,null, typeof(T));
            return getChildByHashCode(hashCode);
        }
       
        //只查一级孩子
        private CObject getChildByHashCode(int hashcode)
        {
            return GetRootCSO().ChildrenGroup.GetChildByHashCode(hashcode);
        }
    }
}