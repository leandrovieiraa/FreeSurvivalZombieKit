using UnityEngine;

namespace Collect.Exceptions {

    /// <summary>
    /// A prefab is missing (either by reference or 
    /// loading directly)
    /// </summary>
    public class MissingPrefabException : UnityException {

        public MissingPrefabException() { }

        public MissingPrefabException(string text) : base(text) { }
    }
}
