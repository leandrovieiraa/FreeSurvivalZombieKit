using UnityEngine;
using UnityEngine.UI;
using Collect.Slots;

namespace Collect.Items {

    /// <summary>
    /// The `StackableSplitter` is a UI component that allows
    /// the user to enter a number of items to pull out of a
    /// `Stackable` stack.
    /// </summary>
    [AddComponentMenu("Collect/Items/Stackable Item Splitter")]
    public class StackableSplitter : MonoBehaviour {

        [Tooltip("Stack that items will be removed from")]
        public Stackable Stack;

        private InputField inputField;

        /// <summary>
        /// Get the `InputField` child object of this gameObject
        /// and activate it so the user does not have to 
        /// click on it prior to entering a number.
        /// </summary>
        public void Start() {
            inputField = GetComponentInChildren<InputField>();

            if (inputField == null) {
                throw new MissingComponentException();
            }

            inputField.ActivateInputField();
        }

        /// <summary>
        /// Listens for presses to `KeyCode.Return` or
        /// `KeyCode.KeypadEnter` and will then
        /// attempt to retrieve the entered number of
        /// items from the `Stackable` and kick-off
        /// the `OnBeginDrag` event on it.
        /// </summary>
        public void Update() {
            //  if they lose focus on the splitter,
            //  destroy it
            if (!inputField.isFocused) {
                Destroy(gameObject);
            }

            //  also destroy it if they hit escape
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Destroy(gameObject);
            }

            //  `Enter` or `Return` has been pressed,
            //  get the value of what was entered and
            //  retrieve that number of items from the
            //  current stack
            if (Input.GetKeyDown(KeyCode.Return) || 
                Input.GetKeyDown(KeyCode.KeypadEnter)) {

                int numberToRemove = int.Parse(inputField.text);
                if (numberToRemove == 0) {
                    return;
                }

                Stackable newStack = Stack.Remove(numberToRemove);
                Slot parentSlot = Stack.GetParentSlot();

                Draggable newDraggableStack = newStack.GetComponent<Draggable>();
                newDraggableStack.OnBeginDrag(null);

                //  if the item that was asked to be split is not
                //  being dragged, put it back in it's parent
                //  slot. Otherwise, it will also start being
                //  dragged
                Draggable thisDraggable = Stack.GetComponent<Draggable>();
                if (!thisDraggable.IsBeingDragged()) {
                    parentSlot.Item = thisDraggable;
                }
            }
        }
    }
}
