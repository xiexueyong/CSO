using CSOEngine.Component;
using CSOEngine.Group;
using CSOEngine.Utils.Pool;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        internal CSOGroup parent_group;
        
        private CSOGroup children_group;
        internal CSOGroup ChildrenGroup
        {
            get
            {
                if (children_group == null)
                {
                    children_group = CSOPool.Get<CSOGroup>();
                    children_group = new CSOGroup();
                    children_group.OwnerCSO = this;
                }
                return children_group;
            }
        }
        
        public CSOCollection GetCollection()
        {
            return ChildrenGroup.GetCollection();
        }

        public CSOCollection GetCollection<T>(bool onlySon=false) where T : CSODataComponent
        {
            var matcher = CSOMatcher.Create().AllOf<T>();
            return GetCollection(matcher, onlySon);
        }

        public CSOCollection GetCollection<T1, T2>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent
        {
            var matcher = CSOMatcher.Create().AllOf<T1, T2>();
            return GetCollection(matcher, onlySon);
        }

        public CSOCollection GetCollection<T1, T2, T3>(bool onlySon = false)
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
        {
            var matcher = CSOMatcher.Create().AllOf<T1, T2, T3>();
            return GetCollection(matcher, onlySon);
        }

        public CSOCollection GetCollection<T1, T2, T3, T4>(bool onlySon = false) 
            where T1 : CSODataComponent
            where T2 : CSODataComponent
            where T3 : CSODataComponent
            where T4 : CSODataComponent
        {
             var matcher = CSOMatcher.Create().AllOf<T1, T2,T3,T4>();
             return GetCollection(matcher, onlySon);
        }
        public CSOCollection GetCollection<T1, T2, T3, T4,T5>(bool onlySon = false)
          where T1 : CSODataComponent
          where T2 : CSODataComponent
          where T3 : CSODataComponent
          where T4 : CSODataComponent
          where T5 : CSODataComponent
        {
            var matcher = CSOMatcher.Create().AllOf<T1, T2, T3, T4, T5>();
            return GetCollection(matcher, onlySon);
        }

        public CSOCollection GetCollection(CSOMatcher matcher,bool onlySon = false)
        {
            return ChildrenGroup.GetCollection(matcher,onlySon);
        }
    }
}