using UnityEngine;

namespace Collect.Exceptions {

    /// <summary>
    /// Call when an item cannot be added to a `Slot`
    /// </summary>
    public class CannotAddItemException : UnityException {

        public CannotAddItemException() { }

        public CannotAddItemException(string text) : base(text) { }
    }
}
