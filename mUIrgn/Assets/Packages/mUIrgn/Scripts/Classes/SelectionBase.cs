using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Murgn.UI
{
    public abstract class SelectionBase : MonoBehaviour
    {
        [Header("Selection Setup")]
        [Tooltip("The direction the selection will follow.")]
        [SerializeField] private SelectionDirection selectionDirection;
        [Tooltip("The item which will be first selected.")]
        [SerializeField] private int firstSelected;
        [Tooltip("If this GameObject is enabled, disable interaction.")]
        [SerializeField] private GameObject OnEnableInteractionDisable;

        [HideInInspector]
        protected int selectedItem = -1;
        protected UIInput input;

        protected void Awake() => input = new UIInput();

        private void OnEnable()
        {
            input.Enable();
            selectedItem = firstSelected;
        }
        
        private void OnDisable() 
        {
            input.Disable();
        }

        protected void Update()
        {
            ItemSelector();
            ItemGraphics();
            ItemInteraction();
        }

        private void ItemSelector()
        {
            if (InteractionDisable()) return;
            
            switch (selectionDirection)
            {
                case SelectionDirection.Horizontal:
                    float horizontal = input.UI.Horizontal.ReadValue<float>();

                    if (input.UI.Horizontal.WasPerformedThisFrame())
                    {
                        if (horizontal > 0)
                            selectedItem++;
                
                        else if (horizontal < 0)
                            selectedItem--;
                        ItemOverflow();
                    }
                    return;
                
                case SelectionDirection.Vertical:
                    float vertical = input.UI.Vertical.ReadValue<float>();

                    if (input.UI.Vertical.WasPerformedThisFrame())
                    {
                        if (vertical < 0)
                            selectedItem++;
                
                        else if (vertical > 0)
                            selectedItem--;
                        
                        ItemOverflow();
                    }
                    return;
            }
        }

        protected abstract void ItemOverflow();
        
        protected abstract void ItemGraphics();
        
        protected abstract void ItemInteraction();

        protected bool InteractionDisable()
        {
            if (OnEnableInteractionDisable != null && OnEnableInteractionDisable.activeSelf == true) 
                return true;
            
            return false;
        }
    }   
}