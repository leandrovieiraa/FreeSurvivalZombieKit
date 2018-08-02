using UnityEngine;
using UnityEngine.UI;

namespace Collect.Items.Tooltips {

    /// <summary>
    /// A tooltip component that supports displaying of text
    /// and hooks for displaying and hiding
    /// </summary>
    [AddComponentMenu("Collect/Items/Tooltips/Standard Tooltip")]
    public class StandardTooltip : MonoBehaviour, Tooltip {

        private string textValue;
        private Text text;

        // Use this for initialization
        void Start() {
            Text text = GetComponentInChildren<Text>();

            if (text == null) {
                throw new MissingComponentException("Tooltip requires a `Text` component");
            }

            text.text = textValue;
        }

        /// <summary>
        /// Display the tooltip with the passed text as content
        /// </summary>
        /// <param name="text">The text to display</param>
        public void Display(string text) {
            textValue = text;
        }

        /// <summary>
        /// Hide and consequently destroy this tooltip
        /// </summary>
        public void Hide() {
            Destroy(gameObject);
        }
    }
}
