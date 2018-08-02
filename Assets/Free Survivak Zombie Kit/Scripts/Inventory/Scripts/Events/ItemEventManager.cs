using UnityEngine;
using UnityEngine.EventSystems;
using Collect.Slots;

namespace Collect.Events {

    public class ItemEventManager {

        public delegate void ItemDidInvalidDrop(GameObject item, Slot slot, PointerEventData data);
        public static event ItemDidInvalidDrop OnItemDidInvalidDrop;

        public delegate void ItemDidDrop(GameObject item, PointerEventData data);
        public static event ItemDidDrop OnItemDidDrop;

        public delegate void ItemDidPickup(GameObject item, PointerEventData data);
        public static event ItemDidPickup OnItemDidPickup;

        /// <summary>
        /// Trigger an invalid item drop
        /// </summary>
        /// <param name="item">The `GameObject` being dropped</param>
        /// <param name="slot">The former parent `Slot` (may be null)</param>
        /// <param name="data">The `PointerEventData` of the event</param>
        public static void TriggerItemDidInvalidDrop(GameObject item, Slot slot, PointerEventData data) {
            if (OnItemDidInvalidDrop != null) {
                OnItemDidInvalidDrop(item, slot, data);
            }
        }

        /// <summary>
        /// Trigger any (valid or invalid) item drop
        /// </summary>
        /// <param name="item">The `GameObject` item being dropped</param>
        /// <param name="data">The `PointerEventData` of the event</param>
        public static void TriggerItemDidDrop(GameObject item, PointerEventData data) {
            if (OnItemDidDrop != null) {
                OnItemDidDrop(item, data);
            }
        }

        /// <summary>
        /// Trigger any (valid or invalid) item pickup
        /// </summary>
        /// <param name="item">The `GameObject` being picked up</param>
        /// <param name="data">The `PointerEventData` of the event</param>
        public static void TriggerItemDidPickup(GameObject item, PointerEventData data) {
            if (OnItemDidPickup != null) {
                OnItemDidPickup(item, data);
            }
        }
    }
}
