using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        [Parameter] public Expression<Func<TValue>>? DateExpression { get; set; }
        [Parameter] public string DateFormat { get; set; }
        [Parameter] public Expression<Func<TValue>> For { get; set; }
        [Parameter] public DatePickerType DateType { get; set; } = DatePickerType.Single;
        [Parameter] public bool EnableTime { get; set; }
        [Parameter] public bool IsInline { get; set; }
        [Parameter] public DateTime? MinDate { get; set; }
        [Parameter] public DateTime? MaxDate { get; set; }

        /// <summary>
        /// Only set to true if using inside a modal
        /// </summary>
        [Parameter]
        public bool MakeStatic { get; set; }

        [Parameter] public DatePickerStyle CssStyle { get; set; } = DatePickerStyle.WebenologyStyle;
        [Parameter] public bool Readonly { get; set; }
        [Parameter] public bool? CanUnlock { get; set; }
        [CascadingParameter] private EditContext _editContext { get; set; }

        private DatePickerType _oldType = DatePickerType.Single;
        public bool _isError => !string.IsNullOrEmpty(_errorMessage);
        public string _errorMessage;
        private bool MovingMouse = false;
        private bool CanUnlockReadonly => Readonly && CanUnlock.GetValueOrDefault();
        private DateTime? _oldMinDate;
        private DateTime? _oldMaxDate;
        private bool _isLoaded;
        private bool _isUnlocked = true;

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
                                return string.Join(", ",
                                    dtLst.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToList());
                            case DatePickerType.Range:
                                return
                                    $"{dtLst.First().ToDtFormat(DateFormat, EnableTime)} to {dtLst.Last().ToDtFormat(DateFormat, EnableTime)}";
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
                                return string.Join(", ",
                                    dtLst.Where(x => x.HasValue).OrderBy(x => x.Value)
                                        .Select(x => x.Value.ToDtFormat(DateFormat, EnableTime)).ToList());
                            case DatePickerType.Range:
                                return
                                    $"{dtLst.First().ToDtFormat(DateFormat, EnableTime)} to {dtLst.Last().ToDtFormat(DateFormat, EnableTime)}";
                        }
                    }

                    // ReSharper disable once PossibleNullReferenceException

                    var dtSng = (DateTime)Convert.ChangeType(Date, typeof(DateTime));
                    return DateType == DatePickerType.TimeOnly
                        ? dtSng.ToTimeOnly(DateFormat)
                        : dtSng.ToDtFormat(DateFormat, EnableTime);
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
            if (_isLoaded)
                js.UpdateSettings(_inputRef, setting, value);
        }

        private async Task OpenCalendar()
        {
            await js.OpenCalendar(_inputRef);
        }

        [JSInvokable]
        public void OnChange(List<DateTime?> dt)
        {
            if (DateType == DatePickerType.Single || DateType == DatePickerType.TimeOnly)
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
                if (typeof(TValue) == typeof(List<DateTime?>))
                {
                    var newDt = new List<DateTime?>();
                    newDt.AddRange(dt.Select(x => x));
                    DateChanged.InvokeAsync((TValue)(object)newDt);
                }
                else
                {
                    var newDt = new List<DateTime>();
                    newDt.AddRange(dt.Select(x => x.GetValueOrDefault()));
                    DateChanged.InvokeAsync((TValue)(object)newDt);
                }
            }

            if (_editContext != null && DateExpression != null)
            {
                var field = FieldIdentifier.Create(DateExpression);
                _editContext.NotifyFieldChanged(field);
            }
        }

        [JSInvokable]
        public bool CanOpen()
        {
            if (!_isUnlocked)
                return false;

            return !Readonly || CanUnlockReadonly;
        }

        public string Css()
        {
            var css = new List<string> { CssStyle.InputCss };

            if (_isError)
                css.Add(CssStyle.InputErrorCss);

            if (!_isUnlocked)
                css.Add(CssStyle.InputInactiveCss);

            css.Add("flatpickr-input");
            return string.Join(" ", css);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var minDate = MinDate?.ToDtFormat(DateFormat, EnableTime);
                var maxDate = MaxDate?.ToDtFormat(DateFormat, EnableTime);
                var mode = DateType == DatePickerType.TimeOnly ? "single" : DateType.ToString().ToLower();
                js.SetupPicker(DotNetObjectReference.Create(this), _inputRef, mode, EnableTime,
                    MakeStatic, IsInline, minDate, maxDate, DateType == DatePickerType.TimeOnly);
                _isLoaded = true;
                if (Readonly || CanUnlockReadonly)
                    _isUnlocked = false;
                StateHasChanged();
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

            if (_oldMinDate != MinDate)
            {
                UpdateSetting("minDate", MinDate?.ToDtFormat(DateFormat, EnableTime));
                _oldMinDate = MinDate;
            }

            if (_oldMaxDate != MaxDate)
            {
                UpdateSetting("maxDate", MaxDate?.ToDtFormat(DateFormat, EnableTime));
                _oldMaxDate = MaxDate;
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

            if (DateType == DatePickerType.Single || DateType == DatePickerType.TimeOnly)
            {
                if (targetType == typeof(List<DateTime?>) || targetType == typeof(List<DateTime>))
                {
                    throw new InvalidOperationException(
                        $"The type '{targetType}' is not a supported with {DateType.ToString()}.");
                }
            }

            if (DateType == DatePickerType.Range || DateType == DatePickerType.Multiple)
            {
                if (targetType == typeof(DateTime) || targetType == typeof(DateTime))
                {
                    throw new InvalidOperationException(
                        $"The type '{targetType}' is not a supported with {DateType.ToString()}.");
                }
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

        private async Task OpenOrClear()
        {
            if (CanUnlockReadonly && !_isUnlocked)
            {
                _isUnlocked = true;
                return;
            }

            if (Readonly && !CanUnlockReadonly)
                return;

            if (string.IsNullOrEmpty(DateTimeStr))
                await OpenCalendar();
            else
            {
                await DateChanged.InvokeAsync();
            }
        }

        private bool IsDisabled()
        {
            if (CanUnlockReadonly && !_isUnlocked)
                return true;

            if (CanUnlockReadonly && _isUnlocked)
                return false;

            return Readonly;
        }
    }
}