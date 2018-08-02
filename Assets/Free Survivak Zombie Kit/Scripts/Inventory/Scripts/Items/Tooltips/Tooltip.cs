using System;

namespace Collect.Items.Tooltips {

    /// <summary>
    /// Tooltip to display text
    /// </summary>
    public interface Tooltip {

        /// <summary>
        /// Display the tooltip with the passed text
        /// </summary>
        /// <param name="text">The text to display</param>
        void Display(String text);

        /// <summary>
        /// Hide this tooltip
        /// </summary>
        void Hide();
    }
}
