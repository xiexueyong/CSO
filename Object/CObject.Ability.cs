using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Component;
using CSOEngine.Utils.Pool;


namespace CSOEngine.Object
{
    public partial class CObject
    {
        internal ComponentsContainer abilityContainer;

        public T AddAbility<T>(bool fromBuilder = true)where T:CSOAbilityComponent,new()
        {
            var ability = CSOPool.Get<T>();
            return AddAbility(ability,fromBuilder) as T;
        }
        
        public CSOAbilityComponent AddAbility(CSOAbilityComponent abilityComponent,bool fromBuilder)
        {
            if (abilityContainer==null)
            {
                abilityContainer = new ComponentsContainer();
            }
            abilityComponent.owner = this;
            abilityContainer.AddComp(abilityComponent);

            if (!fromBuilder)
            {
                triggerAbilityLifecyle(abilityComponent);
                
                if (Activated)
                {
                    abilityComponent.Active(Activated);
                }
            }
            
            OnAddAbility?.Invoke(abilityComponent);
            return abilityComponent;
        }

        internal void triggerAbilityLifecyle()
        {
            if (abilityContainer != null && abilityContainer.components != null)
            {
                foreach (var item in abilityContainer.components)
                {
                    triggerAbilityLifecyle(item.Value as CSOAbilityComponent);
                }
            }
        }
        private void triggerAbilityLifecyle(CSOAbilityComponent abilityComponent)
        {
            //触发OnStart
            abilityComponent.Start();  
        }

        public CSOAbilityComponent RemoveAbility<T>() where T : CSOAbilityComponent
        {
            var component = abilityContainer?.RemoveComp<T>();
            OnRemoveAbility?.Invoke(component);
            return component as T;
        }
        
        public T GetAbility<T>()where T:CSOAbilityComponent
        {
            if (abilityContainer==null)
            {
                return null;
            }
            return abilityContainer.GetComp<T>() as T;
        }
        
        public T GetAbilityByBaseClass<T>()where T:CSOAbilityComponent
        {
            if (abilityContainer==null)
            {
                return null;
            }
            return abilityContainer.GetCompByBaseClass<T>();
        }

        public bool HasAbility<T>()
        {
            if (abilityContainer==null)
            {
                return false;
            }
            return abilityContainer.HasComp<T>();
        }
    }
}