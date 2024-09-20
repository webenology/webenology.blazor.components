using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.OrderByComponent
{
    public partial class OrderBy<TValue> where TValue : class
    {
        [Parameter] public string FieldName { get; set; }
        [Parameter] public string FieldText { get; set; }
        [Parameter] public Expression<Func<TValue, object>>? Field { get; set; }

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
            var fieldName = FieldName;
            if (Field != null)
            {
                var b = (PropertyInfo)((MemberExpression)((UnaryExpression)Field.Body).Operand).Member;
                fieldName = b.Name;
            }
            if (OrderByContainer.CurrentFieldName == fieldName && OrderByContainer.OrderType == OrderByType.NONE)
            {
                OrderByContainer.OnOrderBy(fieldName, OrderByType.ASC);
            }
            else if (OrderByContainer.CurrentFieldName == fieldName && OrderByContainer.OrderType == OrderByType.ASC)
            {
                OrderByContainer.OnOrderBy(fieldName, OrderByType.DESC);
            }
            else if (OrderByContainer.CurrentFieldName == fieldName && OrderByContainer.OrderType == OrderByType.DESC && OrderByContainer.AllowSortToReset)
            {
                OrderByContainer.OnOrderBy(fieldName, OrderByType.NONE);
            }
            else if (OrderByContainer.CurrentFieldName == fieldName && OrderByContainer.OrderType == OrderByType.DESC)
            {
                OrderByContainer.OnOrderBy(fieldName, OrderByType.ASC);
            }
            else
            {
                OrderByContainer.OnOrderBy(fieldName, OrderByType.ASC);
            }
        }
    }
}
