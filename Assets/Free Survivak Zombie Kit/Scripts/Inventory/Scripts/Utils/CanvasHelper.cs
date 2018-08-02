using UnityEngine;

namespace Collect.Utils {

    /// <summary>
    /// A static helper for finding the current
    /// `Canvas` in the scene.
    /// </summary>
    public static class CanvasHelper {

        private static Canvas CurrentCanvas;
        private static readonly string canvasName = "Canvas";

        /// <summary>
        /// Retrieve the canvas currently on the scene,
        /// as long as it's named `Canvas`!
        /// </summary>
        /// <returns></returns>
        public static Canvas GetCanvas() {
            if (CurrentCanvas != null) {
                return CurrentCanvas;
            }

            GameObject canvasObject = GameObject.Find(canvasName);
            CurrentCanvas = canvasObject.GetComponent<Canvas>();
            return CurrentCanvas;
        }
    }
}
