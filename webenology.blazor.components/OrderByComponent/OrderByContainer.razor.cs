using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class OrderByContainer<TValue> where TValue : class
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public List<TValue> InputList { get; set; }
        [Parameter]
        public EventCallback<List<TValue>> OnOutputList { get; set; }
        [Parameter]
        public string InitialFieldName { get; set; }

        [Parameter] public OrderByType InitialOrderBy { get; set; } = OrderByType.NONE;
        [Parameter] public bool AllowSortToReset { get; set; }

        internal string CurrentFieldName = "";
        internal OrderByType OrderType;

        protected override void OnInitialized()
        {
            CurrentFieldName = InitialFieldName;
            OrderType = InitialOrderBy;
            base.OnInitialized();
        }

        internal void OnOrderBy(string fieldName, OrderByType orderBy)
        {
            CurrentFieldName = fieldName;
            OrderType = orderBy;
            Order();
        }

        internal void DoesFieldExist(string fieldName)
        {
            if (typeof(TValue).GetProperty(fieldName) == null)
                throw new Exception($"Field name '{fieldName}' must exist in '{typeof(TValue)}'.");
        }

        private void Order()
        {
            List<TValue> outputList;

            if (OrderType == OrderByType.ASC)
            {
                outputList = InputList.OrderBy(x => x.GetType().GetProperty(CurrentFieldName)?.GetValue(x)).ToList();
            }
            else if (OrderType == OrderByType.DESC)
            {
                outputList = InputList.OrderByDescending(x => x.GetType().GetProperty(CurrentFieldName)?.GetValue(x)).ToList();
            }
            else
            {
                outputList = InputList;
            }

            OnOutputList.InvokeAsync(outputList);
        }
    }
}
