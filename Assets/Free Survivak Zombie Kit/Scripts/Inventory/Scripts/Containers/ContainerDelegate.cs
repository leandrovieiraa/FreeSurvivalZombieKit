using UnityEngine;

namespace Collect.Containers {

    /// <summary>
    /// Delegate for subscribing to `Container` events
    /// </summary>
    public interface ContainerDelegate {

        /// <summary>
        /// Triggered when an item is added to the
        /// container
        /// </summary>
        /// <param name="item">The `GameObject` added</param>
        void ItemWasAdded(GameObject item);

        /// <summary>
        /// Triggered when an item is removed from
        /// the container
        /// </summary>
        /// <param name="item">The `GameObject` removed</param>
        void ItemWasRemoved(GameObject item);

    }
}
