using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class RadioButton<TItem>
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        [Parameter]
        public string Name { get; set; }

        [Parameter]
        public TItem Value { get; set; }

        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        [Parameter]
        public TItem CheckedValue { get; set; }

        private void OnChange()
        {
            ValueChanged.InvokeAsync(CheckedValue);
        }
    }
}
