using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

using webenology.blazor.components.TextInputComponent;

namespace webenology.blazor.components
{
    public partial class WebTextInput : IDisposable
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
        [Parameter]
        public WebTextInputStyle CssStyle { get; set; } = WebTextInputStyle.WebenologyStyle;
        [Parameter]
        public WebInputType InputType {get;set;}
        private bool _isError => !string.IsNullOrEmpty(_errorMessage);
        private string _errorMessage;

        public string LocalText
        {
            get => Text;
            set
            {
                _errorMessage = string.Empty;
                TextChanged.InvokeAsync(value);
            }
        }

        public string Css()
        {
            var css = new List<string> { CssStyle.WebInputCss };

            if (_isError)
            {
                css.Add(CssStyle.WebInputErrorCss);
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
            _errorMessage = string.Empty;
            if (_editContext != null && For != null && IsErrorFunc == null)
            {
                var fieldIdentifier = new FieldIdentifier(_editContext.Model, ((MemberExpression)For.Body).Member.Name);
                var message = _editContext.GetValidationMessages(fieldIdentifier);
                if (message != null && message.Any())
                {
                    _errorMessage = message.First();
                }
            }
            else if (_editContext != null && IsErrorFunc != null && For != null)
            {
                var isError = IsErrorFunc.Invoke();
                if (isError)
                {
                    var messageStore = new ValidationMessageStore(_editContext);
                    _errorMessage = ErrorMsg;
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
