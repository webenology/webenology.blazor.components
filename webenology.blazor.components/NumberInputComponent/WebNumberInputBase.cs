using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace webenology.blazor.components.NumberInputComponent
{
    public class WebNumberInputBase<TValue> : ComponentBase, IDisposable
    {
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public TValue Number { get; set; }
        [Parameter]
        public EventCallback<TValue> NumberChanged { get; set; }
        [Parameter]
        public Expression<Func<TValue>> For { get; set; }
        [Parameter] public decimal? Step { get; set; }
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage;

        public TValue LocalText
        {
            get => Number;
            set
            {
                ErrorMessage = string.Empty;
                NumberChanged.InvokeAsync(value);
            }
        }

        public string Css()
        {
            var css = new List<string> { "form-control" };

            if (IsError)
            {
                css.Add("error");
            }

            return string.Join(" ", css);
        }

        protected override void OnInitialized()
        {
            Step ??= 1;

            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(int) ||
                targetType == typeof(long) ||
                targetType == typeof(short) ||
                targetType == typeof(float) ||
                targetType == typeof(double) ||
                targetType == typeof(decimal))
            {
                //do nothing;
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported numeric type.");
            }


            if (_editContext != null)
            {
                _editContext.OnValidationRequested += _editContext_OnValidationRequested;
            }
            base.OnInitialized();
        }

        private void _editContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            ErrorMessage = string.Empty;
            if (_editContext != null && For != null)
            {
                var fieldIdentifier = new FieldIdentifier(_editContext.Model, ((MemberExpression)For.Body).Member.Name);
                var message = _editContext.GetValidationMessages(fieldIdentifier);
                if (message != null && message.Any())
                {
                    ErrorMessage = message.First();
                }
            }
        }

        public void Dispose()
        {
            if (_editContext != null)
            {
                _editContext.OnValidationRequested -= _editContext_OnValidationRequested;
            }
        }
    }
}
