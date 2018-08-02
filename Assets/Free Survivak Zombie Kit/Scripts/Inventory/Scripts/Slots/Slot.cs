using UnityEngine;
using UnityEngine.EventSystems;
using Collect.Items;
using Collect.Exceptions;
using Collect.Items.Tooltips;

namespace Collect.Slots {

    /// <summary>
    /// Hold `Draggable` objects in this `Slot`. Exposes events
    /// for when an item is added or removed from this slot.
    /// </summary>
    [AddComponentMenu("Collect/Slots/Slot")]
    public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler {

        //  the item in this slot
        public Draggable Item;

        //  delegate called when an item is added to this slot
        public ItemAddedToSlot ItemAddedDelegate;

        //  delegate called when an item is removed from this slot
        public ItemRemovedFromSlot ItemRemovedDelegate;

        public void Start() {
            Item = GetComponentInChildren<Draggable>();
        }

        /// <summary>
        /// When this slot is clicked it will invoke an
        /// `OnPointerClick` event. This is useful if an item
        /// is dropped onto an empty slot (otherwise the event
        /// will be consumed by the item in the slot)
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public virtual void OnPointerClick(PointerEventData eventData) {
            if (eventData.used || Draggable.DraggedItem == null) {
                return;
            }

            OnDrop(eventData);
        }

        /// <summary>
        /// This event is fired when an item is dropped
        /// onto this slot. Will accept the item
        /// into the slot OR swap the item. If the item is
        /// `Stackable`, will attempt to stack it
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public virtual void OnDrop(PointerEventData eventData) {
            if (eventData.used || Draggable.DraggedItem == null) {
                return;
            }

            //  if this slot doesn't have an item,
            //  add this one
            if (Item == null) {
                AddItem(Draggable.DraggedItem);
            } else {

                Stackable stackableItem = Draggable.DraggedItem.GetComponent<Items.Stackable>();
                Stackable slotStackableItem = Item.GetComponent<Items.Stackable>();

                //  the dropped item is stackable, as is
                //  the item in this slot. Attempt to stack.
                //  will return if successful.
                if (stackableItem != null && slotStackableItem != null) {
                    try {
                        slotStackableItem.Add(stackableItem);

                        Draggable.DraggedItem.OnEndDrag(eventData);
                        Draggable.DraggedItem = null;

                        //  manually spawn the tooltip as the
                        //  `OnPointerEnter` event won't trigger
                        //  automatically
                        TooltipTrigger tooltipTrigger = Item.GetComponent<TooltipTrigger>();
                        if (tooltipTrigger != null) {
                            tooltipTrigger.OnPointerEnter(
                                new PointerEventData(EventSystem.current)
                            );
                        }

                    //  if the item isn't stackable, swap items
                    } catch (NotStackableException e) {
                        Slot otherSlot = Draggable.DraggedItem.OldSlot;
                        otherSlot.AddItem(RemoveItem());
                        AddItem(Draggable.DraggedItem);

                        Draggable.DraggedItem.OnEndDrag(eventData);
                        Draggable.DraggedItem = null;
                    }
                } else {

                    //  swap the item with the item
                    //  that was dropped
                    Draggable otherItem = Draggable.DraggedItem;
                    Slot otherSlot = otherItem.OldSlot;

                    //  if other slot is currently occupied,
                    //  the draggable item will need to be
                    //  dropped before it can be swapped
                    if (otherSlot.Item != null) {
                        return;
                    }

                    //  try to swap items- will only fail
                    //  if one slot requires a specific 
                    //  `ItemType`
                    try {
                        //  don't remove the item from
                        //  this slot until it successfully
                        //  gets added to the other slot
                        Draggable itemInThisSlot = Item;
                        otherSlot.AddItem(itemInThisSlot);

                        RemoveItem();
                        AddItem(otherItem);
                    } catch(CannotAddItemException e) {
                        Debug.LogWarning(e);
                        return;
                    }
                }
            }

            Item.OnEndDrag(eventData);
            eventData.Use();
        }

        /// <summary>
        /// Remove the `Draggable` from this slot.
        /// Will return null if no item is present.
        /// </summary>
        /// <returns>The `Draggable` item in this slot</returns>
        public virtual Draggable RemoveItem() {

            if (Item == null) {
                return null;
            }

            Draggable oldItem = Item;
            Item = null;

            if (ItemRemovedDelegate != null) {
                ItemRemovedDelegate(oldItem.gameObject);            
            }

            return oldItem;
        }

        /// <summary>
        /// Add the `Draggable` to this slot. Will set this 
        /// transform as the parent.
        /// </summary>
        /// <param name="item">The `Draggable` to add to this slot</param>
        public virtual void AddItem(Draggable item) {
            this.Item = item;
            item.transform.SetParent(transform);

            if (ItemAddedDelegate != null) {
                ItemAddedDelegate(item.gameObject);
            }
        }
    }
}
