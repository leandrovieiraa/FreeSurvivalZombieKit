using UnityEngine;
using UnityEngine.EventSystems;
using Collect.Items;
using Collect.Exceptions;

namespace Collect.Slots {

    /// <summary>
    /// A `Slot` that limits items of specific
    /// `ItemType'`s to be dropped into it.
    /// </summary>
    [AddComponentMenu("Collect/Slots/Slot With Type")]
    public class SlotWithType : Slot {

        [Tooltip("Only allows items to be placed if they are this type")]
        public ItemType Type;

        /// <summary>
        /// When this slot gets an item dropped on it,
        /// it verifies that it is of the same ItemType.
        /// If so, it adds the item to the slot.If not,
        /// it uses the eventData as the drop was successful.
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public override void OnDrop(PointerEventData eventData) {

            DraggableItemType itemType = Draggable.DraggedItem.GetComponent<DraggableItemType>();

            if (itemType == null) {
                eventData.Use();
                return;
            }

            if (itemType.ItemType == Type) {
                base.OnDrop(eventData);
            } else {
                eventData.Use();
            }
        }

        /// <summary>
        /// Attempts to add an item to this slot.
        /// If the item does not have a `DraggableItemType` or
        /// does not match the type specified by this slot,
        /// will throw a `CannotAddItemException`.
        /// </summary>
        /// <param name="item">The `Draggable` item to add to this slot</param>
        public override void AddItem(Draggable item) {
            DraggableItemType draggableItemType = item.GetComponent<DraggableItemType>();

            if (draggableItemType == null) {
                throw new CannotAddItemException("Item does not have a Type.");
            }

            if (draggableItemType.ItemType == Type)
            {
                base.AddItem(item);          
            }
            else
            {
                throw new CannotAddItemException("Slot requires Type: " + Type + " and item is of Type: " + draggableItemType.ItemType);
            }
        }
    }
}
