using UnityEngine;
using UnityEngine.EventSystems;

namespace Collect.Items {

    /// <summary>
    /// Play a sound when an item is removed from a slot
    /// or placed into a slot.
    /// </summary>
    [RequireComponent(typeof(Draggable))]
    [AddComponentMenu("Collect/Items/Draggable Item Sound")]
    public class DraggableSound : MonoBehaviour {

        [Tooltip("The sound played when this item is picked up")]
        public AudioClip PickUpSound;

        [Tooltip("The sound played when this item is put down")]
        public AudioClip PutDownSound;

        private Draggable draggable;

        /// <summary>
        /// Will retrieve the `Draggable` component on this object
        /// and subscribe to the proper callbacks
        /// </summary>
        public void Start() {
            draggable = GetComponent<Draggable>();
            if (draggable == null) {
                throw new MissingComponentException("`DraggableSound` requires a `Draggable` component to play sounds for _most_ events.");
            }

            //  subscribe to events
            draggable.OnBeginDragCallback += BeginDragCallback;
            draggable.OnEndDragCallback += EndDragCallback;
        }

        /// <summary>
        /// Unsubscribe from callbacks to prevent leaks
        /// </summary>
        public void OnDestroy() {
            //  unsubscribe
            draggable.OnBeginDragCallback -= BeginDragCallback;
            draggable.OnEndDragCallback -= EndDragCallback;
        }

        /// <summary>
        /// Will play the `PickUpSound` clip
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void BeginDragCallback(PointerEventData eventData) {
            playAudioClip(PickUpSound);
        }

        /// <summary>
        /// Will play the `PutDownSound` clip
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void EndDragCallback(PointerEventData eventData) {
            playAudioClip(PutDownSound);
        }

        /// <summary>
        /// Play an `AudioClip` at this place in the world
        /// </summary>
        /// <param name="audioClip">The `AudioClip` to play</param>
        private void playAudioClip(AudioClip audioClip) {
            if (audioClip != null) {
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
            }
        }
    }
}
