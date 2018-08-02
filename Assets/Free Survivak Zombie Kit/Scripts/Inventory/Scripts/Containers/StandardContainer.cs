using UnityEngine;
using System.Collections;
using Collect.Slots;
using Collect.Items;
using Collect.Exceptions;

namespace Collect.Containers {

    /// <summary>
    /// A standard Collect `Container`.
    /// </summary>
    [AddComponentMenu("Collect/Containers/Standard Container")]
    public class StandardContainer : ContainerBase, Container, SlotDelegate {

        /// <summary>
        /// retrieve all slots when started
        /// </summary>
        void Start() {
            slots = retrieveSlots();
        }

        /// <summary>
        /// Add a `GameObject` to the first open slot.
        /// Expects `GameObject` to have a `Draggable`
        /// component attached.
        /// </summary>
        /// <param name="item">The `GameObject` item to add to
        /// this container</param>
        public override void Add(GameObject item) {
            Draggable dragHandler = item.GetComponent<Draggable>();
            if (dragHandler == null) {
                throw new MissingComponentException("Adding to Container requires `Draggable` component");
            }

            Stackable stackHandler = item.GetComponent<Stackable>();
            Slot emptySlot = null, stackableSlot = null;

            foreach(Slot slot in Slots) {

                //  retrieve the first empty slot and retain
                if (emptySlot == null && slot.Item == null) {
                    emptySlot = slot;

                //  if there's an item in this slot
                //  but the item being added is stackable
                //  check if it can stack
                } else if (slot.Item && stackHandler != null) {
                    Stackable itemStack = slot.Item.GetComponent<Stackable>();

                    if (itemStack != null && itemStack.CanStack(stackHandler)) {
                        stackableSlot = slot;
                    }
                }
            }

            //  add the item to the slot
            //  will prioritize stackable slot
            if (stackableSlot != null) {
                stackableSlot.AddItem(dragHandler);
            } else if (emptySlot != null) {
                emptySlot.AddItem(dragHandler);
            }

            throw new NotStackableException("Unable to add item (" + dragHandler.name + ") to container ( " + name + ")");
        }

        /// <summary>
        /// Remove the item that resides in this specific slot.
        /// </summary>
        /// <param name="slot">The `Slot` to remove
        /// an item from</param>
        /// <returns>The `GameObject` in the slot, or null</returns>
        public override GameObject Remove(Slot slot) {
            if (slot.Item == null) {
                return null;
            }

            return slot.RemoveItem().gameObject;
        }

        /// <summary>
        /// Remove this item from the container. Will iterate
        /// through `Slot`'s until it finds the item. 
        /// </summary>
        /// <param name="item">The `GameObject` to remove</param>
        /// <returns>The `GameObject` or null</returns>
        public override GameObject Remove(GameObject item) {
            Draggable dragHandler = item.GetComponent<Draggable>();
            if (dragHandler == null) {
                throw new MissingComponentException("Removing from Container requires Draggable component");
            }

            foreach(Slot slot in Slots) {
                if (slot.Item == dragHandler) {
                    return slot.Item.gameObject;
                }
            }

            return null;
        }

        /// <summary>
        /// Retrieve all the `Slot`'s in this container
        /// </summary>
        /// <returns>ArrayList of `Slot`'s</returns>
        private ArrayList retrieveSlots() {
            ArrayList slots = new ArrayList();
            Slot[] childSlots = transform.GetComponentsInChildren<Slot>();
            foreach (Slot slot in childSlots) {
                slot.ItemAddedDelegate = ItemAddedToSlotDelegate;
                slot.ItemRemovedDelegate = ItemRemovedFromSlotDelegate;
                slots.Add(slot);
            }
            return slots;
        }
    }
}
