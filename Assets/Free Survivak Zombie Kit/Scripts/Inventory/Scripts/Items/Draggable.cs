using UnityEngine;
using UnityEngine.EventSystems;
using Collect.Slots;
using Collect.Events;
using Collect.Static.Inputs;

namespace Collect.Items {

    /// <summary>
    /// Will allow an object to be picked up on click or
    /// on click & hold. `Draggable` components can be
    /// dropped into `Slot`'s.
    /// </summary>
    [AddComponentMenu("Collect/Items/Draggable Item")]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {

        [Tooltip("The item currently being dragged")]
        public static Draggable DraggedItem;

        //  stores the old slot when the
        //  item is dragged so we can reset it
        //  if the drag is unsuccessful
        private Slot oldSlot;
        public Slot OldSlot {
            get { return oldSlot; }
            set { oldSlot = value; }
        }

        //  event that gets fired when a drag begins on this item
        public delegate void OnDragBegin(PointerEventData eventData);
        public event OnDragBegin OnBeginDragCallback;

        //  event that gets fired when a drag ends on this item
        public delegate void OnDragEnd(PointerEventData eventData);
        public event OnDragEnd OnEndDragCallback;

        private CanvasGroup canvasGroup;
        private Canvas canvas;
        private bool beingDragged = false;

        /// <summary>
        /// Will create a `CanvasGroup` component on this
        /// object if it does not already exist. This keeps
        /// the children aligned properly. Also retrieves
        /// the parent canvas of the objec.t
        /// </summary>
        public void Start() {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }

            if (canvas == null) {
                canvas = getParentCanvas();
            }
        }

        /// <summary>
        /// If `beingDragged` is (bool) true, we'll continue
        /// to follow the mouse position
        /// </summary>
        public void Update() {
            if (beingDragged) {
                followMouse(Input.mousePosition);

                //  this is for invalid location drops
                if (Input.GetButtonDown(InputName.General.FIRE) &&
                    !EventSystem.current.IsPointerOverGameObject()) {
                    PointerEventData data = new PointerEventData(EventSystem.current);
                    OnEndDrag(data);
                }
            }
        }

        /// <summary>
        /// This event is automatically fired automatically
        /// as this object is a MonoBehaviour. If the event
        /// is already used, or there is an item already 
        /// being dragged, will do nothing.
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnBeginDrag(PointerEventData eventData) {
            if ((eventData != null && eventData.used) || DraggedItem != null) {
                return;
            }

            if (OnBeginDragCallback != null) {
                OnBeginDragCallback(eventData);
            }

            DraggedItem = this;
            canvasGroup.blocksRaycasts = false;

            oldSlot = GetComponentInParent<Slot>();
            oldSlot.RemoveItem();

            //  place it in the parent canvas so
            //  it renders above everything else
            transform.SetParent(canvas.transform);

            beingDragged = true;

            //  trigger the item did get picked up event
            ItemEventManager.TriggerItemDidPickup(gameObject, eventData);
        }

        /// <summary>
        /// Called when an item is clicked. If no item is
        /// currently being dragged, will trigger the 
        /// `OnBeginDrag` event, otherwise will notify
        /// the parent slot that something is being
        /// dropped on it.
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnPointerClick(PointerEventData eventData) {
            if (eventData != null && eventData.used) {
                return;
            }

            if (DraggedItem == null) {
                OnBeginDrag(eventData);
            } else {
                Slot slot = GetComponentInParent<Slot>();
                slot.OnDrop(eventData);
            }
        }

        /// <summary>
        /// While this object is being dragged it will
        /// follow the position of the event. Will only
        /// follow the mouse if this is the item that is
        /// registered as being dragged.
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnDrag(PointerEventData eventData) {
            if (DraggedItem == this) {
                followMouse(eventData.position);
            }
        }

        /// <summary>
        /// Called when the item stops being dragged.
        /// Will get called _after_ `OnDrop` (which `Slot`
        /// implements). Will trigger the `ItemDidInvalidDrop`
        /// event if the item wasn't dropped successfully.
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnEndDrag(PointerEventData eventData) {
            //  OnEndDrag can be triggered via mouse down
            //  or mouse up, if there is no currently dragged
            //  item the event has already been fired
            if (DraggedItem == null) {
                return;
            }

            DraggedItem = null;

            //  if this item is still in the canvas transform
            //  that means the drop was unsuccessful- trigger
            //  the ItemDidDrop event
            if (transform.parent == canvas.transform) {

                transform.position = oldSlot.transform.position;
                transform.SetParent(oldSlot.transform);
                oldSlot.AddItem(this);

                if (!eventData.used) {
                    ItemEventManager.TriggerItemDidInvalidDrop(gameObject, oldSlot, eventData);
                }
            }

            oldSlot = null;
            canvasGroup.blocksRaycasts = true;

            beingDragged = false;

            //  trigger necessary events
            if (OnEndDragCallback != null) {
                OnEndDragCallback(eventData);
            }

            ItemEventManager.TriggerItemDidDrop(gameObject, eventData);
        }

        /// <summary>
        /// Whether or not this item is currently
        /// being dragged
        /// </summary>
        /// <returns>True if item is being dragged</returns>
        public bool IsBeingDragged() {
            return beingDragged;
        }

        /// <summary>
        /// Position this object to the specific
        /// `Vector3`. Convenient for both manual
        /// and event-driven following of position.
        /// </summary>
        /// <param name="position">The `Vector3` position to follow</param>
        private void followMouse(Vector3 position) {
            transform.position = position;
        }

        /// <summary>
        /// Get the parent canvas of this object.
        /// </summary>
        /// <returns></returns>
        private Canvas getParentCanvas() {
            return GetComponentInParent<Canvas>();
        }
    }
}
