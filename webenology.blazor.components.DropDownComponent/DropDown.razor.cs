﻿using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace webenology.blazor.components.DropDownComponent
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
        [Parameter] public Expression<Func<string>>? ValueExpression { get; set; }
        [Parameter]
        public DropDownStyle Style { get; set; } = DropDownStyle.WebenologyDropDownStyle;
        [Parameter]
        public bool Readonly { get; set; }
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        public void onChangeValue(ChangeEventArgs val)
        {
            ErrorMessage = string.Empty;
            ValueChanged.InvokeAsync((string)val.Value);
            if (_editContext != null && ValueExpression != null)
            {
                var field = FieldIdentifier.Create(ValueExpression);
                _editContext.NotifyFieldChanged(field);
            }
            StateHasChanged();
        }
        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage;

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
