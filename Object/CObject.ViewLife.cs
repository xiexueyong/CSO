using CSOEngine.State;
using System;
using System.Collections.Generic;
using CSOEngine.Group;
using CSOEngine.Component;
using UnityEngine;

namespace CSOEngine.Object
{
    public partial class CObject
    {
        public enum CSOViewStatus
        {
            Show,
            Hide,
            Destroy,
            Rebuild,
            None,
        }
       
        protected bool ViewHidden { get; set; }
        protected bool ViewDestroyed { get; set; }

        public void ShowView()
        {
            RebuildView();

            if (ViewHidden)
            {
                showTransform(true);
                ViewHidden = false;
                //自己的ability
                if (abilityContainer != null)
                {
                    foreach (var ability in abilityContainer.components)
                    {
                        (ability.Value as CSOAbilityComponent)?.ShowView();
                    }
                }
                //自己的view
                if (viewContainer != null)
                {
                    foreach (var view in viewContainer.components)
                    {
                        (view.Value as CSOViewComponent)?.ShowView();
                    }
                }

                //Children ability
                if (children_group != null && children_group.GetCollection().Objects != null)
                {
                    foreach (var child in children_group.GetCollection().Objects)
                    {
                        child.ShowView();
                    }
                }
            }
        }


        public void HideView()
        {
            if (!ViewHidden)
            {
                ViewHidden = true;
                showTransform(false);
                 
                //自己的ability
                if (abilityContainer != null)
                {
                    foreach (var ability in abilityContainer.components)
                    {
                        (ability.Value as CSOAbilityComponent)?.HideView();
                    }
                }
                //自己的view
                if (viewContainer != null)
                {
                    foreach (var view in viewContainer.components)
                    {
                        (view.Value as CSOViewComponent)?.HideView();
                    }
                }
                
                //Children ability
                if (children_group != null && children_group.GetCollection().Objects != null)
                {
                    foreach (var child in children_group.GetCollection().Objects)
                    {
                        child.HideView();
                    } 
                }
            }
        }
        
        public void DestroyView()
        {
            if (!ViewDestroyed)
            {
                ViewDestroyed = true;
                
                //自己的ability
                if (abilityContainer != null)
                {
                    foreach (var ability in abilityContainer.components)
                    {
                        (ability.Value as CSOAbilityComponent)?.DestroyView();
                    }
                }
                
                //自己的view
                if (viewContainer != null)
                {
                    foreach (var view in viewContainer.components)
                    {
                        (view.Value as CSOViewComponent)?.DestroyView();
                    }
                }
                
                //Children ability
                if (children_group != null && children_group.GetCollection().Objects != null)
                {
                    foreach (var child in children_group.GetCollection().Objects)
                    {
                        child.DestroyView();
                    } 
                }
            }
        }
        
        private void RebuildView()
        {
            //重建
            if (ViewDestroyed)
            {
                 ViewDestroyed = false;
                //自己的ability
                if (abilityContainer != null)
                {
                    foreach (var ability in abilityContainer.components)
                    {
                        (ability.Value as CSOAbilityComponent)?.RebuildView();
                    }
                }
                //Children ability
                if (children_group != null && children_group.GetCollection().Objects != null)
                {
                    foreach (var child in children_group.GetCollection().Objects)
                    {
                        child.RebuildView();
                    } 
                }
            }
        }

        private void showTransform(bool value)
        {
              if(Transform.gameObject.activeSelf != value)
                    Transform.gameObject.SetActive(value);
        }
       
    }
}