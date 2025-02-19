﻿using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.OrderByComponent
{
    public partial class OrderByContainer<TValue> where TValue : class
    {
        [Parameter]
        public RenderFragment<List<TValue>> ChildContent { get; set; }
        [Parameter]
        public EventCallback<Tuple<string, OrderByType>> OnOrderChange { get; set; }
        [Parameter]
        public List<TValue> InputList { get; set; }
        [Parameter]
        public string InitialFieldName { get; set; }
        [Parameter] public OrderByType InitialOrderBy { get; set; } = OrderByType.NONE;
        [Parameter] public bool AllowSortToReset { get; set; }

        internal string CurrentFieldName = "";
        internal OrderByType OrderType;
        private RenderFragment<TValue>? _itemRender;
        private List<TValue> _output;

        protected override void OnInitialized()
        {
            CurrentFieldName = InitialFieldName;
            OrderType = InitialOrderBy;
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (InputList != null)
            {
                Order();
            }
            base.OnParametersSet();
        }

        public void Refresh()
        {
            Order();
        }

        internal void OnOrderBy(string fieldName, OrderByType orderBy)
        {
            CurrentFieldName = fieldName;
            OrderType = orderBy;
            OnOrderChange.InvokeAsync(new Tuple<string, OrderByType>(fieldName, orderBy));
            Order();
        }

        internal void DoesFieldExist(string fieldName)
        {
            return;
            if (typeof(TValue).GetProperty(fieldName) == null)
                throw new Exception($"Field name '{fieldName}' must exist in '{typeof(TValue)}'.");
        }
        public void OnOrder(string fieldName, OrderByType orderBy)
        {
            OnOrderBy(fieldName, orderBy);
        }
        private void Order()
        {
            if (OrderType == OrderByType.ASC)
            {
                _output = InputList.OrderBy(x => x.GetType().GetProperty(CurrentFieldName)?.GetValue(x)).ToList();
            }
            else if (OrderType == OrderByType.DESC)
            {
                _output = InputList.OrderByDescending(x => x.GetType().GetProperty(CurrentFieldName)?.GetValue(x)).ToList();
            }
            else
            {
                _output = InputList;
            }
            StateHasChanged();
        }
    }
}
