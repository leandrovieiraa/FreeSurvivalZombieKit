using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Collect.Exceptions;
using Collect.Slots;

namespace Collect.Items {

    /// <summary>
    /// Component that allows `Draggable` items to be stacked.
    /// Will allow items of the same prefab to be stacked
    /// and unstacked (when clicked with the `keyModifier`
    /// </summary>
    [AddComponentMenu("Collect/Items/Stackable Item")]
    [RequireComponent(typeof (Draggable))]
    public class Stackable : MonoBehaviour, IPointerClickHandler {

        [Tooltip("How many items this stack can hold")]
        public int max = 5;

        [Tooltip("Key modifier used to split stack")]
        public KeyCode keyModifier = KeyCode.LeftShift;

        [Tooltip("Stack of items")]
        public List<Stackable> Stack = new List<Stackable>();

        private Text countLabel;
        private const string countLabelName = "Count Label";

        /// <summary>
        /// Will retrieve the child `Text` component 
        /// and update it
        /// </summary>
        void Start() {
            countLabel = GetComponentInChildren<Text>();

            if (countLabel == null) {
                throw new MissingComponentException("Cannot display stack count without child `Text` component on item");
            }

            UpdateCountLabel();
        }

        /// <summary>
        /// Add an item to this stack.If a `Stackable` item
        /// is added that has several children, will iterate
        /// through the children first and add them. Then,
        /// it will add the container of the stack. 
        /// If the size of both stacks is larger than the max
        /// allowed, will throw a `NotStackableException`.
        /// </summary>
        /// <param name="stackable">The `Stackable` item to stack</param>
        public void Add(Stackable stackable) {
            if (stackable.GetType() != GetType()) {
                throw new NotStackableException("Unable to stack, these items are not of the same type");
            }

            if (Size() >= max - 1 || Size() + stackable.Size() >= max - 1) {
                throw new NotStackableException("Unable to stack: " + this + " with " + stackable);
            }

            foreach(Stackable s in stackable.Stack) {
                Add(s);
            }

            stackable.transform.SetParent(transform);
            stackable.gameObject.SetActive(false);
            stackable.Stack.Clear();
            Stack.Add(stackable);
            UpdateCountLabel();
        }

        /// <summary>
        /// Remove the specified number of items
        /// from this stack.Will return `this` if
        /// count requested is larger (or equal to)
        /// current size.
        /// 
        /// If not, will pop off the top of the stack
        /// and create a new `Stackable` that will then
        /// be returned.
        /// </summary>
        /// <param name="requestedCount">The number of `Stackable`
        /// items to return</param>
        /// <returns>A new `Stackable`</returns>
        public Stackable Remove(int requestedCount) {
            if (requestedCount - 1 >= Size()) {
                return this;
            }

            Stackable baseStackable = Get(0);

            if (requestedCount > 1) {
                List<Stackable> otherStackables = Stack.GetRange(0, requestedCount - 1);
                Stack.RemoveRange(0, requestedCount - 1);

                foreach (Stackable s in otherStackables) {
                    baseStackable.Add(s);
                }
            }

            UpdateCountLabel();
            baseStackable.UpdateCountLabel();
            return baseStackable;
        }

        /// <summary>
        /// Convenience method to remove a
        /// single item from the stack
        /// </summary>
        /// <returns>A single `Stackable` item</returns>
        public Stackable Remove() {
            return Remove(1);
        }

        /// <summary>
        /// Get the current size of the `Stackable`
        /// </summary>
        /// <returns></returns>
        public int Size() {
            return Stack.Count;
        }

        /// <summary>
        /// When this stack is clicked, check if the
        /// user is holding down the key modifier. If
        /// so, display the `StackableSplitter` which
        /// allows the user to enter a number to
        /// remove from this stack
        /// </summary>
        /// <param name="eventData">The `PointerEventData` of the event</param>
        public void OnPointerClick(PointerEventData eventData) {
            if (Input.GetKey(keyModifier)) {
                StackableSplitterFactory.Create(this);
                eventData.Use();
            }
        }

        /// <summary>
        /// Get the parent slot of this stack.Useful
        /// when stacking is not possible, and `Draggable`
        /// does not retain this reference
        /// </summary>
        /// <returns>The parent `Slot`</returns>
        public Slot GetParentSlot() {
            return GetComponentInParent<Slot>();
        }

        /// <summary>
        /// Update the count label assocaited with this
        /// stack. If only 1 item is in this `Stackable`,
        /// will not display a number.
        /// </summary>
        public void UpdateCountLabel() {
            if (Size() >= 1) {
                countLabel.text = Size() + 1 + "/" + max;
            } else {
                countLabel.text = "";
            }
        }

        /// <summary>
        /// Returns true if otherStack can be stacked
        /// onto this stack
        /// </summary>
        /// <param name="otherStack">The `Stackable` that may or may not
        /// be able to be stacked</param>
        /// <returns>True if passed `Stackable` can be stacked
        /// on this `Stackable`</returns>
        public bool CanStack(Stackable otherStack) {
            if (otherStack.GetType() != GetType()) {
                return false;
            }

            if (Size() >= max - 1 || Size() + otherStack.Size() >= max - 1) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if otherStack is of same
        /// type. This should eventually be replaced
        /// by a custom comparator
        /// </summary>
        /// <param name="otherStack">The `Stackable` to compare types with</param>
        /// <returns>True if passed `Stackable` is of same type</returns>
        public bool IsSameType(Stackable otherStack) {
            return otherStack.GetType() == GetType();
        }

        /// <summary>
        /// Remove an item from the stack and
        /// prepare it for the scene- this will
        /// activate the object and clear it from
        /// the stack.
        /// </summary>
        /// <param name="index">The position in the stack to retrieve
        /// the item</param>
        /// <returns>A new `Stackable` stack</returns>
        private Stackable Get(int index) {
            Stackable stack = Stack[index];
            stack.gameObject.SetActive(true);
            Stack.RemoveAt(index);
            return stack;
        }
    }
}
