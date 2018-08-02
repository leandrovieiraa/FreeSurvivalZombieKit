using UnityEngine;
using Collect.Utils;

namespace Collect.Items.Tooltips {

    public class TooltipFactory {

        /// <summary>
        /// Create a new tooltip from passed prefab, with text as
        /// content, and the trigger as the bounds to appear near.
        /// </summary>
        /// <param name="prefab">The `Tooltip` prefab</param>
        /// <param name="text">The text to display in the tooltip</param>
        /// <param name="trigger">The RectTransform that triggered
        /// this tooltip. Will dictate where the tooltip will display
        /// on the canvas</param>
        /// <returns>The `Tooltip` that is created</returns>
        public static Tooltip Create(GameObject prefab, string text, RectTransform trigger) {

            Canvas canvas = CanvasHelper.GetCanvas();
            GameObject tooltipObject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            tooltipObject.transform.SetParent(canvas.transform);

            //  TODO: if the tooltip is off the screen, reposition
            //  should this be moved elsewhere? In the tooltip itself?
            Vector3 newPosition = trigger.position;
            newPosition.x += (trigger.rect.width * canvas.scaleFactor) / 2;
            newPosition.y += (trigger.rect.height * canvas.scaleFactor) / 2;
            tooltipObject.transform.position = newPosition;

            Tooltip t = tooltipObject.GetComponent<Tooltip>();
            t.Display(text);

            return t;
        }
        
    }
}
