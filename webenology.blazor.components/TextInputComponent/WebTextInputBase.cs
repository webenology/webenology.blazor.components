using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace webenology.blazor.components.TextInputComponent
{
    public class WebTextInputBase : ComponentBase, IDisposable
    {
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public EventCallback<string> TextChanged { get; set; }
        [Parameter]
        public Expression<Func<string>> For { get; set; }
        [Parameter]
        public Func<bool> IsErrorFunc { get; set; }
        [Parameter]
        public string ErrorMsg { get; set; }
        [Parameter] public string PlaceHolder { get; set; }
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
        public string ErrorMessage;

        public string LocalText
        {
            get => Text;
            set
            {
                ErrorMessage = string.Empty;
                TextChanged.InvokeAsync(value);
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
            if (_editContext != null)
            {
                _editContext.OnValidationRequested += _editContext_OnValidationRequested;
            }
            base.OnInitialized();
        }

        private void _editContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            ErrorMessage = string.Empty;
            if (_editContext != null && For != null && IsErrorFunc == null)
            {
                var fieldIdentifier = new FieldIdentifier(_editContext.Model, ((MemberExpression)For.Body).Member.Name);
                var message = _editContext.GetValidationMessages(fieldIdentifier);
                if (message != null && message.Any())
                {
                    ErrorMessage = message.First();
                }
            }
            else if (_editContext != null && IsErrorFunc != null && For != null)
            {
                var isError = IsErrorFunc.Invoke();
                if (isError)
                {
                    var messageStore = new ValidationMessageStore(_editContext);
                    ErrorMessage = ErrorMsg;
                    //messageStore.Add(For, ErrorMsg);

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
