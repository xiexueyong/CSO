using CSOEngine.Object;
using CSOEngine.Utils.Pool;

namespace CSOEngine.Component
{
    public class CSOBaseComponent
    {
        internal CObject owner;
        protected virtual void OnAdd()
        {
        }
        protected virtual void OnStart()
        {
        }
        protected virtual void OnRemove()
        {
        }
        protected virtual  void OnActive(bool active)
        {
        }
      
        //================================
        public bool Added{get;private set;}
        public bool Started{get;private set;}
        public bool Actived{get;private set;}


        internal void Add()
        {
            Added = true;
            OnAdd();
        }
        internal void Start()   {
            Started = true;
            OnStart();
        }
        internal void Remove()   {
            Active(false);
            Added = false;
            Started = false;
            OnRemove();
            CSOPool.Release(this);
        }
        internal void Active(bool value){
            if (Actived != value)
            {
                Actived = value;
                OnActive(value);
            }
        }
      
    }
}