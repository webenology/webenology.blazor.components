using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace webenology.blazor.components
{
    public partial class WebNumberInput<TValue> : IDisposable
    {
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public TValue Number { get; set; }
        [Parameter]
        public EventCallback<TValue> NumberChanged { get; set; }
        [Parameter] public Expression<Func<TValue>>? NumberExpression { get; set; }
        [Parameter]
        public Expression<Func<TValue>> For { get; set; }

        [Parameter] public double? Step { get; set; }
        [Parameter] public double? Min { get; set; }
        [Parameter] public double? Max { get; set; }
        [Parameter]
        public bool HighlightOnFocus { get; set; }
        [Parameter]
        public WebNumberInputStyle CssStyle { get; set; } = WebNumberInputStyle.WebenologyStyle;
        [Parameter]
        public bool Readonly { get; set; }
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        private Dictionary<string, object> _attributes = new();

        private bool _isError => !string.IsNullOrEmpty(_errorMessage);
        private string _errorMessage;
        private ElementReference _el { get; set; }
        [Inject]
        private IWebTextInputJsHelper jsHelper { get; set; }

        public TValue LocalText
        {
            get => Number;
            set
            {
                _errorMessage = string.Empty;
                NumberChanged.InvokeAsync(value);
                if (_editContext != null && NumberExpression != null)
                {
                    var field = FieldIdentifier.Create(NumberExpression);
                    _editContext.NotifyFieldChanged(field);
                }
            }
        }

        private async Task DoFocus(FocusEventArgs e)
        {
            if (HighlightOnFocus)
            {
                await jsHelper.HighlightText(_el);
            }
        }
        public string ErrorCss()
        {
            var css = new List<string> { CssStyle.InputGroupAddonCss };

            if (_isError)
                css.Add(CssStyle.InputGroupAddonErrorCss);

            return string.Join(" ", css);
        }

        public string InputCss()
        {
            var css = new List<string> { CssStyle.InputCss };

            if (_isError)
                css.Add(CssStyle.InputErrorCss);

            return string.Join(" ", css);
        }

        protected override void OnInitialized()
        {
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

            if (Step.HasValue)
                _attributes.Add("step", Step.Value);

            if (Min.HasValue)
                _attributes.Add("min", Min.Value);

            if (Max.HasValue)
                _attributes.Add("max", Max.Value);
            if (Readonly)
                _attributes.Add("readonly", "readonly");

            if (_editContext != null)
            {
                _editContext.OnValidationRequested += _editContext_OnValidationRequested;
            }
            base.OnInitialized();
        }

        private void _editContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            _errorMessage = string.Empty;
            if (_editContext != null && For != null)
            {
                var fieldIdentifier = new FieldIdentifier(_editContext.Model, ((MemberExpression)For.Body).Member.Name);
                var message = _editContext.GetValidationMessages(fieldIdentifier);
                if (message != null && message.Any())
                {
                    _errorMessage = message.First();
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
