using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class OrderBy<TValue> where TValue : class
    {
        [Parameter] public string FieldName { get; set; }
        [Parameter] public string FieldText { get; set; }

        [CascadingParameter]
        private OrderByContainer<TValue> OrderByContainer { get; set; }

        protected override void OnInitialized()
        {
            if (OrderByContainer == null)
                throw new Exception("OrderBy must be in an OrderByContainer");

            OrderByContainer.DoesFieldExist(FieldName);

            base.OnInitialized();
        }

        private void OnToggleChanges()
        {
            if (OrderByContainer.CurrentFieldName == FieldName && OrderByContainer.OrderType == OrderByType.NONE)
            {
                OrderByContainer.OnOrderBy(FieldName, OrderByType.ASC);
            }
            else if (OrderByContainer.CurrentFieldName == FieldName && OrderByContainer.OrderType == OrderByType.ASC)
            {
                OrderByContainer.OnOrderBy(FieldName, OrderByType.DESC);
            }
            else if (OrderByContainer.CurrentFieldName == FieldName && OrderByContainer.OrderType == OrderByType.DESC && OrderByContainer.AllowSortToReset)
            {
                OrderByContainer.OnOrderBy(FieldName, OrderByType.NONE);
            }
            else if (OrderByContainer.CurrentFieldName == FieldName && OrderByContainer.OrderType == OrderByType.DESC)
            {
                OrderByContainer.OnOrderBy(FieldName, OrderByType.ASC);
            }
            else
            {
                OrderByContainer.OnOrderBy(FieldName, OrderByType.ASC);
            }
        }
    }
}
