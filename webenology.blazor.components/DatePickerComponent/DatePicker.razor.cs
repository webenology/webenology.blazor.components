using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace webenology.blazor.components
{
    public partial class DatePicker<TValue>
    {
        [Inject] private IDateTimerPickerJsHelper js { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }
        [Parameter] public TValue Date { get; set; }
        [Parameter] public string Label { get; set; }
        [Parameter] public EventCallback<TValue> DateChanged { get; set; }
        [Parameter] public string DateFormat { get; set; }
        [Parameter] public Expression<Func<TValue>> For { get; set; }
        [Parameter] public DatePickerType DateType { get; set; } = DatePickerType.Single;
        [Parameter] public bool EnableTime { get; set; }
        /// <summary>
        /// Only set to true if using inside a modal
        /// </summary>
        [Parameter] public bool MakeStatic { get; set; }
        [Parameter]
        public DatePickerStyle CssStyle { get; set; } = DatePickerStyle.WebenologyStyle;
        [CascadingParameter] private EditContext _editContext { get; set; }

        private DatePickerType _oldType = DatePickerType.Single;
        public bool _isError => !string.IsNullOrEmpty(_errorMessage);
        public string _errorMessage;

        private string DateTimeStr
        {
            get
            {
                if (Date == null)
                    return null;
                try
                {
                    if (typeof(List<DateTime>) == typeof(TValue))
                    {
                        var dtLst = (List<DateTime>)Convert.ChangeType(Date, typeof(List<DateTime>));
                        if (dtLst == null || !dtLst.Any())
                            return null;

                        switch (DateType)
                        {

                            case DatePickerType.Multiple:
                                return string.Join(", ", dtLst.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList());
                            case DatePickerType.Range:
                                return $"{dtLst.First().ToDtFormat(DateFormat, EnableTime)} to {dtLst.Last().ToDtFormat(DateFormat, EnableTime)}";
                        }
                    }

                    if (typeof(List<DateTime?>) == typeof(TValue))
                    {
                        var dtLst = (List<DateTime?>)Convert.ChangeType(Date, typeof(List<DateTime?>));
                        if (dtLst == null || !dtLst.Any())
                            return null;

                        switch (DateType)
                        {

                            case DatePickerType.Multiple:
                                return string.Join(", ", dtLst.Where(x => x.HasValue).OrderBy(x => x.Value).Select(x => x.Value.ToDtFormat(DateFormat, EnableTime)).ToList());
                            case DatePickerType.Range:
                                return $"{dtLst.First().ToDtFormat(DateFormat, EnableTime)} to {dtLst.Last().ToDtFormat(DateFormat, EnableTime)}";
                        }
                    }

                    // ReSharper disable once PossibleNullReferenceException

                    var dtSng = (DateTime)Convert.ChangeType(Date, typeof(DateTime));
                    return dtSng.ToDtFormat(DateFormat, EnableTime);
                }
                catch (Exception e)
                {
                    //probably one of the items in the list was null
                    return null;
                }

            }
        }

        private ElementReference _inputRef;

        private void UpdateSetting(string setting, string value)
        {
            js.UpdateSettings(_inputRef, setting, value);
        }


        [JSInvokable]
        public void OnChange(List<DateTime?> dt)
        {
            if (DateType == DatePickerType.Single)
            {
                var dtSng = dt.FirstOrDefault();
                if (typeof(TValue) == typeof(DateTime?))
                {
                    var converted = (TValue)Convert.ChangeType(dtSng, typeof(DateTime));
                    DateChanged.InvokeAsync(converted);
                }
                else
                {
                    var converted = (TValue)Convert.ChangeType(dtSng, typeof(TValue));
                    DateChanged.InvokeAsync(converted);
                }

            }
            else
            {
                DateChanged.InvokeAsync((TValue)(object)dt);
            }
        }

        public string Css()
        {
            var css = new List<string> { CssStyle.InputCss };

            if (_isError)
                css.Add(CssStyle.InputErrorCss);

            return string.Join(" ", css);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                js.SetupPicker(DotNetObjectReference.Create(this), _inputRef, DateType.ToString().ToLower(), EnableTime,
                    MakeStatic);
            }

            base.OnAfterRender(firstRender);
        }

        protected override void OnParametersSet()
        {
            if (_oldType != DateType)
            {
                UpdateSetting("mode", DateType.ToString().ToLower());
                _oldType = DateType;
            }

            base.OnParametersSet();
        }

        protected override void OnInitialized()
        {
            var targetType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
            if (targetType == typeof(DateTime) ||
                targetType == typeof(DateTime?) ||
                targetType == typeof(List<DateTime>) ||
                    targetType == typeof(List<DateTime?>))
            {
                //do nothing;
            }
            else
            {
                throw new InvalidOperationException($"The type '{targetType}' is not a supported date type.");
            }

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
