using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Murgn.UI
{
    public class TextSelection : SelectionBase
    {
        
        [SerializeField] private Image selectionArrow;
        [Tooltip("The offset of the arrow from the selected item.")]
        [SerializeField] private Vector2 selectionArrowOffset;
        [NonReorderable] [SerializeField] private SelectionText[] selectionItems;

        [Header("Item Settings")]
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color selectedColor = Color.yellow;
        
        private int oldSelectedItem;
        private bool isDisabled;

        private new void Update() => base.Update();
        
        protected override void ItemOverflow()
        {
            // If selectedItem is less than 0, loop back to the end
            if (selectedItem < 0)
                selectedItem = selectionItems.Length - 1;

            // If selectedItem is more than the amount of items, loop back to the start
            if (selectedItem > selectionItems.Length - 1)
                selectedItem = 0;
        }

        protected override void ItemGraphics()
        {
            // Show selectedItem and arrow as selected
            selectionItems[selectedItem].selectionText.color = isDisabled ? normalColor : selectedColor;
            selectionArrow.color = isDisabled ? normalColor : selectedColor;

            // Move selectionArrow
            selectionArrow.rectTransform.SetParent(selectionItems[selectedItem].selectionText.rectTransform);
            selectionArrow.rectTransform.anchorMin = new Vector2(0.0f, 0.5f);
            selectionArrow.rectTransform.anchorMax = new Vector2(0.0f, 0.5f);
            selectionArrow.rectTransform.anchoredPosition = selectionArrowOffset;
            selectionArrow.rectTransform.pivot = new Vector2(0.5f, 0.5f);

            // Show oldSelectedItem as normal
            if (selectedItem != oldSelectedItem && oldSelectedItem >= 0)
                selectionItems[oldSelectedItem].selectionText.color = normalColor;
            
            oldSelectedItem = selectedItem;
            
        }
        
        protected override void ItemInteraction()
        {
            if (InteractionDisable())
            {
                isDisabled = true;
                return;
            }
            else isDisabled = false;

            if (input.UI.Interact.WasPerformedThisFrame())
            {
                selectionItems[selectedItem].interactActions.Invoke();
            }
        }
    }   
}