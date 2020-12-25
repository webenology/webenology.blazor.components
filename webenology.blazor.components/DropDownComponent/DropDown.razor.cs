using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace webenology.blazor.components
{
    public partial class DropDown
    {
        [Parameter]

        public List<KeyValuePair<string, string>> Items { get; set; }
        [Parameter]
        public string EmptyValue { get; set; }
        [Parameter]
        public string EmptyText { get; set; }
        [Parameter]
        public bool ShowEmpty { get; set; }
        [Parameter]
        public Expression<Func<string>> For { get; set; }
        [Parameter]
        public string Value { get; set; }
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [CascadingParameter]
        private EditContext _editContext { get; set; }

        public void onChangeValue(ChangeEventArgs val)
        {
            ErrorMessage = string.Empty;
            ValueChanged.InvokeAsync((string)val.Value);
            StateHasChanged();
        }
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage;

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
