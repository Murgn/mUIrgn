using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Murgn.UI
{
    [Icon("")]
    public class ImageSelection : SelectionBase
    {
        [SerializeField] private Image selectionArrow;
        [Tooltip("The offset of the arrow from the selected item.")]
        [SerializeField] private Vector2[] selectionArrowOffset;
        [NonReorderable] [SerializeField] private SelectionImage[] selectionItems;
        private List<Vector2> selectionItemPositions = new();
        
        [Header("Item Settings")]
        [SerializeField] private Sprite normalPanel;
        [SerializeField] private Sprite selectedPanel;
        [SerializeField] private Color selectedColor = Color.yellow;
        [SerializeField] private Vector2 selectedOffset = new(0.0f, -1.0f);
        
        private int oldSelectedItem;
        private bool isDisabled;

        private new void Start()
        {
            // TODO: Since this is for every selection class, it may reduce peformance, need to think of a way to only 
            // TODO: call it once.
            //Canvas.ForceUpdateCanvases();
            
            for (int i = 0; i < selectionItems.Length; i++)
            {
                RectTransform rectTransform = (RectTransform)selectionItems[i].selectionImage.transform;
                selectionItemPositions.Add(rectTransform.anchoredPosition);
                //Debug.LogError(rectTransform.anchoredPosition);
            }
            selectionArrow.color = selectedColor;
        }
        
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
            // Show selectedItem as selected
            selectionItems[selectedItem].selectionImage.sprite = isDisabled ? normalPanel : selectedPanel;
            selectionItems[selectedItem].selectionImage.rectTransform.anchoredPosition = selectionItemPositions[selectedItem] + (isDisabled ? Vector2.zero : selectedOffset);

            // Move selectionArrow
            selectionArrow.rectTransform.SetParent(selectionItems[selectedItem].selectionImage.rectTransform);
            selectionArrow.rectTransform.anchorMin = new Vector2(0.5f, 1.0f);
            selectionArrow.rectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            selectionArrow.rectTransform.anchoredPosition = selectionArrowOffset[selectedItem];
            selectionArrow.rectTransform.pivot = new Vector2(0.5f, 1.0f);
           
            // Show oldSelectedItem as normal
            if (selectedItem != oldSelectedItem && oldSelectedItem >= 0)
            {
                selectionItems[oldSelectedItem].selectionImage.sprite = normalPanel;
                selectionItems[oldSelectedItem].selectionImage.rectTransform.anchoredPosition = selectionItemPositions[oldSelectedItem] - selectedOffset;
            }
            
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