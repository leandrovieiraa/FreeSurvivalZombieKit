using UnityEngine;
using UnityEngine.EventSystems;
using Collect.Exceptions;

namespace Collect.Items.Tooltips {

    /// <summary>
    /// Attach to any `GameObject` to support mouseover/mouseout
    /// `Tooltip` display. Allows flexibility to support different
    /// tooltips via associated prefab.
    /// </summary>
    [AddComponentMenu("Collect/Items/Tooltips/Tooltip Trigger")]
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        [Tooltip("Tooltip prefab")]
        public GameObject tooltipPrefab;

        [TextArea(1, 100)]
        [Tooltip("Content of the tooltip")]
        public string text;

        private Tooltip tooltip;

        //  The Rect Transform of this object
        //  Used to determine bounds
        private RectTransform rectTransform;

        /// <summary>
        /// Will attempt to retrieve the `Tooltip` prefab and 
        /// `RectTransform`, throwing exceptions if either doesn't exist
        /// </summary>
        public void Start() {
            if (tooltipPrefab == null) {
                throw new MissingPrefabException("Must have a tooltip prefab attached to a `TooltipTrigger`");
            }

            rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null) {
                throw new MissingComponentException("Must have a `RectTransform` attached to this object");
            }
        }

        /// <summary>
        /// When moused over, and if an item is _not_ being dragged,
        /// this will use the `TooltipFactory` to instantiate a new
        /// `Tooltip` at the location around the associated `RectTransform`
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnPointerEnter(PointerEventData eventData) {
            if (tooltipPrefab == null || rectTransform == null) {
                return;
            }

            //  don't show tooltip if an item is being dragged
            if (Draggable.DraggedItem != null) {
                return;
            }

            tooltip = TooltipFactory.Create(tooltipPrefab, text, rectTransform);
        }

        /// <summary>
        /// On mouse out, hide the tooltip (which in turn, gets destroyed)
        /// and set the current tooltip to `null`
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnPointerExit(PointerEventData eventData) {
            if (tooltip == null) {
                return;
            }

            //  TODO: this is gross
            tooltip.Hide();
            tooltip = null;
        }
    }
}
