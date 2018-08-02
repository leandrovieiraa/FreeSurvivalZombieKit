using UnityEngine;
using System.Collections;
using Collect.Slots;

namespace Collect.Containers {

    /// <summary>
    /// Abstract base class for Collect containers.
    /// Handles opening, closing, toggling, and events.
    /// Requires implementation of Add/Remove methods
    /// </summary>
    public abstract class ContainerBase : MonoBehaviour, Container, SlotDelegate {

        public event ItemAdded ItemWasAdded;
        public event ItemRemoved ItemWasRemoved;

        /// <summary>
        /// Subscribing items being added to a slot in
        /// this container
        /// </summary>
        public event ItemAddedToSlot ItemWasAddedToSlot;

        /// <summary>
        /// Subscribing to items being removed from slots
        /// in this container
        /// </summary>
        public event ItemRemovedFromSlot ItemWasRemovedFromSlot;

        /// <summary>
        /// The slots in this container
        /// </summary>
        protected ArrayList slots;

        /// <summary>
        /// Accessor for slots in this container
        /// </summary>
        public ArrayList Slots {
            get { return slots; }
        }

        public virtual void Open() {
            gameObject.SetActive(true);
        }

        public virtual void Close() {
            gameObject.SetActive(false);
        }

        public virtual void Toggle() {
            if (gameObject.activeSelf) {
                gameObject.SetActive(false);
            } else {
                gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Called when an item is added to this container
        /// </summary>
        /// <param name="item">The `GameObject` added</param>
        public virtual void ItemAddedToSlotDelegate(GameObject item) {
            if (ItemWasAdded != null) {
                ItemWasAdded(item);
            }
        }

        /// <summary>
        /// Called when an item is removed from this container
        /// </summary>
        /// <param name="item">The `GameObject` removed</param>
        public virtual void ItemRemovedFromSlotDelegate(GameObject item) {
            if (ItemWasRemoved != null) {
                ItemWasRemoved(item);
            }
        }

        public abstract void Add(GameObject item);

        public abstract GameObject Remove(Slot slot);

        public abstract GameObject Remove(GameObject item);
    }
}
