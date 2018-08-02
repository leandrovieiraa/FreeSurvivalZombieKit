using UnityEngine;

namespace Collect.Slots {

    public delegate void ItemAddedToSlot(GameObject item);
    public delegate void ItemRemovedFromSlot(GameObject item);

    /// <summary>
    /// Extend this interface to receive events
    /// from slots. When an item is removed
    /// or added, will notify the delegate- this
    /// object will _need_ to be set as the
    /// slot's delegate
    /// </summary>
    public interface SlotDelegate {

        event ItemAddedToSlot ItemWasAddedToSlot;

        event ItemRemovedFromSlot ItemWasRemovedFromSlot;
        
    }
}
