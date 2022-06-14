using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    /// <summary>
    /// Radio button that can be part of a group or as a single radio button
    /// </summary>
    /// <typeparam name="TItem">The Selected Type</typeparam>
    public partial class RadioButton<TItem>
    {
        /// <summary>
        /// Additional html attributes that get added to the <code>&lt;input type="radio" /&gt;</code>
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }
        /// <summary>
        /// Name of the radio button (2 or more radio buttons with the same name will be part of the same group)
        /// </summary>
        [Parameter]
        public string Name { get; set; }
        /// <summary>
        /// The value that is returned when clicking on the radio button
        /// </summary>
        [Parameter]
        public TItem Value { get; set; }
        /// <summary>
        /// Function to enable binding the value to the result
        /// </summary>
        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }
        /// <summary>
        /// What the checked value is
        /// </summary>
        [Parameter]
        public TItem CheckedValue { get; set; }

        private void OnChange()
        {
            ValueChanged.InvokeAsync(CheckedValue);
        }
    }
}
