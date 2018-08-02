using UnityEngine;

using Collect.Slots;

namespace Collect.Containers {

    /// <summary>
    /// Delegate called when an item is added to this container
    /// </summary>
    /// <param name="item">The item added to the container</param>
    public delegate void ItemAdded(GameObject item);

    /// <summary>
    /// Delegate called when an item is removed from this container
    /// </summary>
    /// <param name="item">The item removed from the container</param>
    public delegate void ItemRemoved(GameObject item);

    /// <summary>
    /// Interface for all Collect containers
    /// </summary>
    public interface Container {

        /// <summary>
        /// Open this container
        /// </summary>
        void Open();

        /// <summary>
        /// Close this container
        /// </summary>
        void Close();

        /// <summary>
        /// Toggle the visibility of this container
        /// </summary>
        void Toggle();

        /// <summary>
        /// Add an item to this container. Does not
        /// require `Draggable` item
        /// </summary>
        /// <param name="item"></param>
        void Add(GameObject item);

        /// <summary>
        /// Remove whatever item is in the passed `Slot`
        /// </summary>
        /// <param name="slot">The slot in this container</param>
        /// <returns>The item in the slot</returns>
        GameObject Remove(Slot slot);

        /// <summary>
        /// Remove the `GameObject` if it exists in this container
        /// </summary>
        /// <param name="item">The `GameObject` to remove from
        /// this container</param>
        /// <returns>The GameObject or null if the item is not
        /// in this container</returns>
        GameObject Remove(GameObject item);

        /// <summary>
        /// Event for when an item is added to this container
        /// </summary>
        event ItemAdded ItemWasAdded;

        /// <summary>
        /// Event for when an item is removed from this container
        /// </summary>
        event ItemRemoved ItemWasRemoved;

    }
}
