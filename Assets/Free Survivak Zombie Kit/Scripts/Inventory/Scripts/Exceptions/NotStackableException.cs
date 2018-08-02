using UnityEngine;

namespace Collect.Exceptions {

    /// <summary>
    /// Throw if an item is not stackable in this item stack
    /// </summary>
    public class NotStackableException : UnityException {

        public NotStackableException() { }

        public NotStackableException(string text) : base(text) { }
    }
}
