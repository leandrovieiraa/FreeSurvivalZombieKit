using UnityEngine;
using Collect.Utils;

namespace Collect.Items {

    /// <summary>
    /// Exposes a convenience method for spawning
    /// a `StackableSplitter` in the canvas on top
    /// of a specific `Stackable`
    /// </summary>
    public static class StackableSplitterFactory {

        private static string stackableSplitterPath = "Prefabs/Collect/Stack Splitter";

        /// <summary>
        /// Creates a `StackableSplitter` directly on
        /// top of this stack and sets its parent to
        /// the canvas.
        /// </summary>
        /// <param name="stack">The `Stackable` to create a `StacakbleSplitter` for</param>
        public static void Create(Stackable stack) {
            GameObject stackableSplitterResource = Resources.Load(stackableSplitterPath) as GameObject;
            GameObject stackableSplitterObject = GameObject.Instantiate(stackableSplitterResource, stack.transform.position, Quaternion.identity) as GameObject;
            stackableSplitterObject.transform.SetParent(CanvasHelper.GetCanvas().transform);
            StackableSplitter stackableSplitter = stackableSplitterObject.GetComponent<StackableSplitter>();
            stackableSplitter.Stack = stack;
        }
    }
}
