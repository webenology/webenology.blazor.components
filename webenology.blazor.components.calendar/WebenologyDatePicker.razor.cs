using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace webenology.blazor.components.calendar;

public partial class WebenologyDatePicker
{
    [Parameter] public List<DateTime?> DateRange { get; set; }
    [Parameter] public EventCallback<List<DateTime?>> DateRangeChanged { get; set; }
    [Parameter] public DateTime? Date { get; set; }
    [Parameter] public EventCallback<DateTime?> DateChanged { get; set; }
    [Parameter] public bool? IsRangeCalendar { get; set; }
    [Parameter] public bool IsSmall { get; set; }
    [Parameter] public DateTime? MinDateTime { get; set; }
    [Parameter] public DateTime? MaxDateTime { get; set; }
    [Parameter] public List<DateTime>? BlackoutDates { get; set; }
    [Parameter] public bool IsDisabled { get; set; }
    [Parameter] public bool ShowTime { get; set; }
    [Parameter] public string? BaseCssClass { get; set; }
    [Parameter] public string? InputCss { get; set; }
    [Parameter] public TimeZoneInfo TimeZone { get; set; } = TimeZoneInfo.Local;
    [Inject]
    private IJSRuntime jsRuntime { get; set; }
    private string _wrapperCss => IsDisabled ? "wc-outline wc-disabled" : "wc-outline";
    private bool _isRangeCalendar;
    private bool _isCalendarVisible;
    private int middleMonth;
    private DateTime today = new DateTime(DateTime.Now.Ticks, DateTimeKind.Local);
    private int selectedYear;
    private int currentYear;
    private int visibleYear;
    private DateTime? FirstDate;
    private DateTime? LastDate;
    private DateTime? ClickedDate;
    private List<DateTime?> CurrentDateRange { get; set; }
    private int StartingInt => IsSmall ? 0 : -1;
    private int EndingInt => IsSmall ? 0 : 1;
    private int dayOfToday = DateTime.Now.Day;
    private ElementReference _el;
    private CalendarJsHelper _jsHelper;
    private WebenologyTime _fromTime = new();
    private WebenologyTime? _toTime = new();

    private Regex _dateRegex =
        new Regex(
            @"^(?<Month>\d{2})[\/\.-]?(?<Day>\d{2})[\/\.-]?(?<Year>\d{2,4})[\/\.-]?(\s?(?<Hour>\d{2}):?(?<Minute>\d{2})\s?(?<Meridian>[AaPp][Mm])?)?$");

    private Regex _dateRangeRegex = new Regex(
        @"^(?<Month1>\d{2})[\/\.-]?(?<Day1>\d{2})[\/\.-]?(?<Year1>\d{2,4})[\/\.-]?(\s?(?<Hour1>\d{2}):?(?<Minute1>\d{2})\s?(?<Meridian1>[AaPp][Mm])?)?\s?[TtOo-]{1,2}\s?(?<Month2>\d{2})[\/\.-]?(?<Day2>\d{2})[\/\.-]?(?<Year2>\d{2,4})[\/\.-]?(\s?(?<Hour2>\d{2}):?(?<Minute2>\d{2})\s?(?<Meridian2>[AaPp][Mm])?)?$");

    private Regex _dateTimeRangeRegex = new Regex(
        @"^(?<Month>\d{2})[\/\.-]?(?<Day>\d{2})[\/\.-]?(?<Year>\d{2,4})[\/\.-]?(\s?(?<Hour1>\d{2}):?(?<Minute1>\d{2})\s?(?<Meridian1>[AaPp][Mm])?)?\s?[TtOo-]{1,2}\s?(\s?(?<Hour2>\d{2}):?(?<Minute2>\d{2})\s?(?<Meridian2>[AaPp][Mm])?)?$");

    private static string[] MONTHS =
    {
        "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER",
        "DECEMBER"
    };

    private string _textValue;
    private ElementReference _textInput;

    private string TextValue
    {
        get => GetDateRangeForInput(false);
        set => _textValue = value;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await _jsHelper.StopPropagationOnEnter(_textInput, DotNetObjectReference.Create(this));
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    [JSInvokable]
    public void OnEnterHit(string val)
    {
        var oldVal = TextValue;
        TextValue = val;
        var canParse = DateParser();
        if (!canParse)
        {
            TextValue = oldVal;
        }
        _isCalendarVisible = false;

    }

    private bool DateParser()
    {
        (DateTime, WebenologyTime?) dt1 = new ValueTuple<DateTime, WebenologyTime?>(DateTime.MinValue, null);
        (DateTime, WebenologyTime?) dt2 = new ValueTuple<DateTime, WebenologyTime?>(DateTime.MinValue, null);
        var ranSpecial = false;
        if (SpecialCaseHelper.SpecialValueMatch.IsMatch(_textValue.ToLower()))
        {
            var data = SpecialCaseHelper.RunCase(_textValue.ToLower());
            if (data.Item1.Item1 != null)
            {
                dt1 = ParseDate(data.Item1.Item1!.Value.Month.ToString(), data.Item1.Item1!.Value.Day.ToString(), data.Item1.Item1!.Value.Year.ToString(), data.Item1.Item2?.Hour.ToString() ?? "0",
                    data.Item1.Item2?.Minute.ToString() ?? "0",
                    data.Item1.Item2?.Meridian.ToString() ?? "AM");
                ranSpecial = true;
            }
            if (data.Item2.GetValueOrDefault().Item1.HasValue && IsRangeCalendar.GetValueOrDefault())
            {
                dt2 = ParseDate(data.Item2?.Item1?.Month.ToString() ?? "0", data.Item2?.Item1?.Day.ToString() ?? "0", data.Item2?.Item1?.Year.ToString() ?? "0", data.Item2?.Item2?.Hour.ToString() ?? "0",
                    data.Item1.Item2?.Minute.ToString() ?? "0",
                    data.Item1.Item2?.Meridian.ToString() ?? "AM");
                ranSpecial = true;
            }
        }

        if (!ranSpecial)
        {
            if (_dateRegex.IsMatch(_textValue))
            {
                var matches = _dateRegex.Matches(_textValue);
                var m = matches[0].Groups["Month"].Value;
                var d = matches[0].Groups["Day"].Value;
                var y = matches[0].Groups["Year"].Value;
                var h = matches[0].Groups["Hour"].Value;
                var min = matches[0].Groups["Minute"].Value;
                var mer = matches[0].Groups["Meridian"].Value;
                dt1 = ParseDate(m, d, y, h, min, mer);
            }
            else if (_dateRangeRegex.IsMatch(_textValue))
            {
                var matches = _dateRangeRegex.Matches(_textValue);
                var m = matches[0].Groups["Month1"].Value;
                var d = matches[0].Groups["Day1"].Value;
                var y = matches[0].Groups["Year1"].Value;
                var h = matches[0].Groups["Hour1"].Value;
                var min = matches[0].Groups["Minute1"].Value;
                var mer = matches[0].Groups["Meridian1"].Value;
                dt1 = ParseDate(m, d, y, h, min, mer);

                var m2 = matches[0].Groups["Month2"].Value;
                var d2 = matches[0].Groups["Day2"].Value;
                var y2 = matches[0].Groups["Year2"].Value;
                var h2 = matches[0].Groups["Hour2"].Value;
                var min2 = matches[0].Groups["Minute2"].Value;
                var mer2 = matches[0].Groups["Meridian2"].Value;
                dt2 = ParseDate(m2, d2, y2, h2, min2, mer2);
            }
            else if (_dateTimeRangeRegex.IsMatch(_textValue))
            {
                var matches = _dateTimeRangeRegex.Matches(_textValue);
                var m = matches[0].Groups["Month"].Value;
                var d = matches[0].Groups["Day"].Value;
                var y = matches[0].Groups["Year"].Value;
                var h1 = matches[0].Groups["Hour1"].Value;
                var min1 = matches[0].Groups["Minute1"].Value;
                var mer1 = matches[0].Groups["Meridian1"].Value;
                dt1 = ParseDate(m, d, y, h1, min1, mer1);

                var h2 = matches[0].Groups["Hour2"].Value;
                var min2 = matches[0].Groups["Minute2"].Value;
                var mer2 = matches[0].Groups["Meridian2"].Value;
                dt2 = ParseDate(m, d, y, h2, min2, mer2);
            }
            else
            {
                _textValue = "NONE";
            }
        }

        if (!_isRangeCalendar)
        {
            if (dt1.Item1 != DateTime.MinValue)
            {
                if (IsDisabledDate(dt1.Item1))
                    return true;
                Date = dt1.Item1;
                CurrentDateRange = new List<DateTime?>
                    {
                        dt1.Item1,
                        dt1.Item1
                    };
                visibleYear = dt1.Item1.Year;
                middleMonth = dt1.Item1.Month;

                if (dt1.Item2 != null)
                    _fromTime = dt1.Item2;
            }
        }
        else
        {
            if (dt1.Item1 != DateTime.MinValue)
            {
                var fromDt = dt1.Item1;
                var toDt = dt2.Item1 != DateTime.MinValue ? dt2.Item1 : dt1.Item1;

                if (IsDisabledDate(fromDt))
                    return false;
                if (IsDisabledDate(toDt))
                    return false;
                CurrentDateRange = new List<DateTime?>
                    {
                        fromDt > toDt ? toDt : fromDt,
                        fromDt > toDt ? fromDt : toDt
                    };
                visibleYear = CurrentDateRange.Last().GetValueOrDefault().Year;
                middleMonth = CurrentDateRange.Last().GetValueOrDefault().Month;
                if (dt1.Item2 != null)
                    _fromTime = dt1.Item2;

                _toTime = dt2.Item2 ?? dt1.Item2;
            }
        }

        SelectDateRange();
        return true;
    }

    private async Task SelectAll()
    {
        await _jsHelper.SelectAll(_textInput);
    }

    private (DateTime, WebenologyTime) ParseDate(string mon, string d, string y, string h, string min, string mer)
    {
        int.TryParse(mon, out var month);
        int.TryParse(d, out var day);
        int.TryParse(y, out var year);
        var decade =
            (int)Math.Floor(DateTime.Now.Year / 1000d) * 1000;
        if (year < 100)
        {
            if (year - (DateTime.Now.Year - decade) > 10)
            {
                year += (decade - 100);
            }
            else
            {
                year += decade;
            }
        }
        if (month is < 1 or > 12)
            month = DateTime.Now.Month;
        if (day < 1)
            day = DateTime.Now.Day;
        day = Math.Min(DateTime.DaysInMonth(year, month), day);
        var dt = new DateTime(year, month, day, 0, 0, 0, 0, DateTimeKind.Unspecified);

        var respondTime = new WebenologyTime();

        int.TryParse(h, out var hour);
        int.TryParse(min, out var minute);
        var meridian = mer;
        if (hour is < 0 or > 24)
            hour = 0;
        if (minute is < 0 or > 59)
            minute = 0;

        if (hour > 11)
        {
            hour -= 12;
            meridian = "PM";
        }

        if (hour == 0)
            hour = 12;

        if (string.IsNullOrEmpty(meridian))
            meridian = "AM";

        var m = meridian.StartsWith("a", StringComparison.OrdinalIgnoreCase)
            ? MeridianEnum.AM
            : MeridianEnum.PM;

        respondTime = new WebenologyTime(hour, minute, m);

        return new ValueTuple<DateTime, WebenologyTime>(dt, respondTime);
    }


    protected override void OnParametersSet()
    {
        if (!_isCalendarVisible)
        {
            SetupDates();
        }

        base.OnParametersSet();
    }

    private void SetupDates()
    {
        if (DateRange != null && Date.HasValue)
            throw new ArgumentException("You can only set a date range or a single date");

        _isRangeCalendar = (DateRangeChanged.HasDelegate || DateRange != null) &&
                           (IsRangeCalendar.HasValue && IsRangeCalendar.Value || !IsRangeCalendar.HasValue);

        if (Date.HasValue)
        {
            CurrentDateRange = new List<DateTime?>
            {
                Date.Value,
                Date.Value
            };
            _fromTime = SetTimeFromDateTime(Date.Value);
            _toTime = null;
        }
        else
        {
            _fromTime = SetTimeFromDateTime(DateRange?.FirstOrDefault(new DateTime?()).GetValueOrDefault() ?? new DateTime());
            _toTime = SetTimeFromDateTime(DateRange?.LastOrDefault(new DateTime?()).GetValueOrDefault() ?? new DateTime());
        }
    }

    private WebenologyTime SetTimeFromDateTime(DateTime dt)
    {
        var isPm = dt.Hour >= 12;
        var hour = isPm && dt.Hour > 12 ? dt.Hour - 12 : dt.Hour;
        if (hour == 0)
            hour = 12;

        return new WebenologyTime(hour, dt.Minute, isPm ? MeridianEnum.PM : MeridianEnum.AM);
    }

    protected override void OnInitialized()
    {
        middleMonth = DateTime.Now.Month;
        currentYear = DateTime.Now.Year;
        visibleYear = currentYear;
        _jsHelper = new CalendarJsHelper(jsRuntime);
        base.OnInitialized();
    }

    private Task HideCalendar()
    {
        SelectDateRange();
        _isCalendarVisible = false;
        return Task.CompletedTask;
    }

    private async Task ToggleCalendar()
    {
        if (IsDisabled)
            return;


        _isCalendarVisible = !_isCalendarVisible;

        await Task.Yield();
        if (_isCalendarVisible)
        {
            UpdateCalendarPosition();
            SetupDates();

        }
    }

    private string GetMonthName(int i)
    {
        var (month, year) = GetNewMonthAndYear(i);
        return $"{MONTHS[month - 1]} {year}";
    }

    private int GetDaysInMonth(int i)
    {
        var monthAndYear = GetNewMonthAndYear(i);
        return DateTime.DaysInMonth(monthAndYear.Item2, monthAndYear.Item1);
    }

    private string BuildCss(int i, int day)
    {
        var str = new StringBuilder();
        var monthAndYear = GetNewMonthAndYear(i);
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day, 0, 0, 0, 0, DateTimeKind.Unspecified);
        if (day == 1)
        {
            var skipIt = $"wc-skip-{(int)dt.DayOfWeek}";
            str.Append(skipIt);
            str.Append(" ");
        }

        var isToday = dt.Date == today.Date;
        if (isToday)
            str.Append("wc-today ");

        var isSelected = IsSelected(dt);
        if (isSelected)
            str.Append("wc-selected ");

        if (IsDisabledDate(dt))
            str.Append("wc-disabled ");

        var isClicked = IsClicked(dt);
        if (isClicked)
            str.Append("wc-clicked ");

        return str.ToString();
    }

    private bool IsDisabledDate(DateTime dt)
    {
        var isInMinMaxDisabled = MinDateTime.HasValue && dt.Date < MinDateTime.Value.Date ||
                                 MaxDateTime.HasValue && dt.Date > MaxDateTime.Value.Date;

        if (_isRangeCalendar)
            return isInMinMaxDisabled;

        return isInMinMaxDisabled ||
               BlackoutDates != null && BlackoutDates.Any(x => x.Date == dt.Date);
    }

    private bool IsSelected(DateTime currentDate)
    {
        if (IsDisabledDate(currentDate))
            return false;

        if (CurrentDateRange == null)
        {
            if (DateRange != null && DateRange.Any())
            {
                return DateRange.First().GetValueOrDefault().Date <= currentDate.Date &&
                       DateRange.Last().GetValueOrDefault().Date >= currentDate;
            }
        }
        else
        {
            return CurrentDateRange.First().GetValueOrDefault().Date <= currentDate.Date &&
                   CurrentDateRange.Last().GetValueOrDefault().Date >= currentDate;
        }

        return false;
    }

    private bool IsClicked(DateTime currentDate)
    {
        if (IsDisabledDate(currentDate))
            return false;

        if (FirstDate.HasValue && LastDate.HasValue)
        {
            return FirstDate.Value.Date <= currentDate.Date && LastDate >= currentDate.Date;
        }

        if (FirstDate.HasValue)
        {
            return FirstDate.Value.Date == currentDate.Date;
        }

        return false;
    }

    private void OnClickDate(int i, int day)
    {
        var monthAndYear = GetNewMonthAndYear(i);
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day, 0, 0, 0, 0, DateTimeKind.Unspecified);

        if (IsDisabledDate(dt))
            return;

        if (!_isRangeCalendar)
        {
            CurrentDateRange = new List<DateTime?>
            {
                dt,
                dt
            };
            return;
        }

        if (!ClickedDate.HasValue)
        {
            ClickedDate = dt;
            FirstDate = dt;
            return;
        }

        LastDate ??= dt;

        CurrentDateRange = new List<DateTime?>
        {
            FirstDate.GetValueOrDefault(),
            LastDate.GetValueOrDefault()
        };
        ClickedDate = null;
        FirstDate = null;
        LastDate = null;
        _textValue = string.Empty;
    }

    private Task SetLastDate(int i, int day)
    {
        var monthAndYear = GetNewMonthAndYear(i);
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day, 0, 0, 0, 0, DateTimeKind.Local);

        if (ClickedDate.HasValue)
        {
            if (ClickedDate > dt)
            {
                LastDate = ClickedDate;
                FirstDate = dt;
            }
            else
            {
                LastDate = dt;
                FirstDate = ClickedDate;
            }
        }

        return Task.CompletedTask;
    }


    private void UpdateMiddleMonth(int i)
    {
        middleMonth += i;
        if (middleMonth < 1)
        {
            middleMonth = 12;
            visibleYear--;
        }

        if (middleMonth > 12)
        {
            middleMonth = 1;
            visibleYear++;
        }
    }

    private Tuple<int, int> GetNewMonthAndYear(int i)
    {
        var month = middleMonth + i;
        var year = visibleYear;
        switch (month)
        {
            case 0:
                month = 12;
                year--;
                break;
            case 13:
                month = 1;
                year++;
                break;
        }

        return new Tuple<int, int>(month, year);
    }

    internal enum QuickSelect
    {
        None,
        Today,
        Yesterday,
        LastSevenDays,
        LastWeek,
        LastThreeWeeks,
        MonthToDate,
        YearToDate,
        Year
    }

    private Task OnQuickSelect(QuickSelect quickSelect)
    {
        switch (quickSelect)
        {
            case QuickSelect.Today:
                CurrentDateRange = new List<DateTime?> { today, today };
                _textValue = string.Empty;
                break;
            case QuickSelect.Yesterday:
                CurrentDateRange = new List<DateTime?> { today.AddDays(-1), today.AddDays(-1) };
                _textValue = string.Empty;
                break;
            case QuickSelect.LastSevenDays:
                CurrentDateRange = new List<DateTime?> { today.AddDays(-6), today };
                _textValue = string.Empty;
                break;
            case QuickSelect.LastWeek:
                var thisSaturday = today.AddDays(6 - (int)today.DayOfWeek);
                CurrentDateRange = new List<DateTime?> { thisSaturday.AddDays(-13), thisSaturday.AddDays(-7) };
                _textValue = string.Empty;
                break;
            case QuickSelect.LastThreeWeeks:
                var thisSaturdayAgain = today.AddDays(6 - (int)today.DayOfWeek);
                CurrentDateRange = new List<DateTime?>
                    { thisSaturdayAgain.AddDays(-27), thisSaturdayAgain.AddDays(-7) };
                _textValue = string.Empty;
                break;
            case QuickSelect.MonthToDate:
                var startOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, 0, DateTimeKind.Local);
                CurrentDateRange = new List<DateTime?> { startOfMonth, today };
                _textValue = string.Empty;
                break;
            case QuickSelect.YearToDate:
                var startOfYear = new DateTime(today.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
                CurrentDateRange = new List<DateTime?> { startOfYear, today };
                _textValue = string.Empty;
                break;
            case QuickSelect.Year:
                CurrentDateRange = new List<DateTime?> { new DateTime(today.Year, 1, 1, 0, 0, 0, 0, DateTimeKind.Local),
                    new DateTime(today.Year, 12, 31, 0, 0, 0, 0, DateTimeKind.Local) };
                _textValue = string.Empty;
                break;
        }

        UpdateQuickSelect();

        visibleYear = today.Year;
        middleMonth = today.Month;

        return Task.CompletedTask;
    }

    private void UpdateQuickSelect()
    {
        if (MinDateTime.HasValue && CurrentDateRange.First().Value < MinDateTime.Value)
        {
            CurrentDateRange.RemoveAt(0);
            CurrentDateRange.Insert(0, MinDateTime.Value.AddDays(1));
        }

        if (MaxDateTime.HasValue && CurrentDateRange.Last().Value > MaxDateTime.Value)
        {
            CurrentDateRange.RemoveAt(1);
            CurrentDateRange.Insert(1, MaxDateTime.Value.AddDays(-1));
        }

        if (CurrentDateRange[1] < CurrentDateRange[0])
        {
            CurrentDateRange[1] = CurrentDateRange[0];
        }
    }

    private Task OnSelectMonth(int month)
    {
        if (!_isRangeCalendar)
            return Task.CompletedTask;

        var monthAndYear = GetNewMonthAndYear(month);
        var daysInMonth = GetDaysInMonth(month);
        CurrentDateRange = new List<DateTime?>
        {
            new DateTime(monthAndYear.Item2, monthAndYear.Item1, 1, 0, 0, 0, 0, DateTimeKind.Local),
            new DateTime(monthAndYear.Item2, monthAndYear.Item1, daysInMonth, 0, 0, 0, 0, DateTimeKind.Local),
        };
        _textValue = string.Empty;
        return Task.CompletedTask;
    }

    private string GetDateRangeData()
    {
        var str = new StringBuilder();
        DateTime? date1 = null;
        DateTime? date2 = null;
        WebenologyTime? time1 = null;
        WebenologyTime? time2 = null;

        if (FirstDate.HasValue && LastDate.HasValue)
        {
            date1 = FirstDate;
            date2 = LastDate;
        }
        else if (FirstDate.HasValue && !LastDate.HasValue)
        {
            date1 = FirstDate;
            date2 = null;
        }
        else if (CurrentDateRange != null)
        {
            date1 = CurrentDateRange.First();
            date2 = CurrentDateRange.Last();
        }
        else if (DateRange != null && DateRange.Any())
        {
            date1 = DateRange.First();
            date2 = DateRange.Last();
        }

        if (ShowTime)
        {
            time1 = _fromTime;
            time2 = _toTime ?? new WebenologyTime();
        }

        if (date1 == null)
        {
            str.Append("NONE");
            return str.ToString();
        }

        str.Append(date1.GetValueOrDefault().ToString("ddd, MMM dd, yyyy"));
        if (ShowTime)
        {
            str.Append(" ");
            str.Append(time1);
        }
        if (date2 != null && date1 != date2)
        {
            str.Append(" to ");
            str.Append(date2.GetValueOrDefault().ToString("ddd, MMM dd, yyyy"));
            if (ShowTime)
            {
                str.Append(" ");
                str.Append(time2);
            }
        }

        return str.ToString();
    }

    private string GetDateRangeForInput(bool bypassTextMatch)
    {
        if (!string.IsNullOrEmpty(_textValue) && SpecialCaseHelper.SpecialValueMatch.IsMatch(_textValue) && !bypassTextMatch)
            return _textValue;

        var str = new StringBuilder();
        DateTime? date1 = null;
        DateTime? date2 = null;

        if (!Date.HasValue)
        {
            if (DateRange == null || !DateRange.Any())
            {
                return "NONE";
            }

            date1 = DateRange.First();
            date2 = DateRange.Last();
        }
        else
        {
            date1 = Date.GetValueOrDefault();
        }

        str.Append(date1.GetValueOrDefault().ToString("MM-dd-yyyy"));
        if (ShowTime)
        {
            str.Append(" ");
            str.Append(date1.GetValueOrDefault().ToString("hh:mm tt"));
        }
        if (date2 != null && date1 != date2)
        {
            str.Append(" to ");
            str.Append(date2.GetValueOrDefault().ToString("MM-dd-yyyy"));
            if (ShowTime)
            {
                str.Append(" ");
                str.Append(date2.GetValueOrDefault().ToString("hh:mm tt"));
            }
        }

        return str.ToString();
    }

    private Task GoToLastMonthSelected()
    {
        if (CurrentDateRange != null && CurrentDateRange.Any() && CurrentDateRange.Last().HasValue)
        {
            var last = CurrentDateRange.Last().Value;
            visibleYear = last.Year;
            middleMonth = last.Month;
        }
        else if (DateRange != null && DateRange.Any() && DateRange.Last().HasValue)
        {
            var last = DateRange.Last().GetValueOrDefault();
            visibleYear = last.Year;
            middleMonth = last.Month;
        }

        return Task.CompletedTask;
    }

    private Task Reset()
    {
        CurrentDateRange = null;
        _isCalendarVisible = false;
        return Task.CompletedTask;
    }

    private Task SelectDateRange()
    {
        var time1 = _fromTime;
        var time2 = _isRangeCalendar ? _toTime ?? new WebenologyTime() : _fromTime;


        if (CurrentDateRange != null && CurrentDateRange.Any())
        {
            CurrentDateRange[0] = SetDateAndTime(CurrentDateRange[0].GetValueOrDefault(), time1);
            CurrentDateRange[1] = SetDateAndTime(CurrentDateRange[1].GetValueOrDefault(), time2);
            if (CurrentDateRange[0] > CurrentDateRange[1])
            {
                (CurrentDateRange[0], CurrentDateRange[1]) = (CurrentDateRange[1], CurrentDateRange[0]);
            }
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(CurrentDateRange);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(CurrentDateRange.Last());
        }
        else if (DateRange is { Count: > 1 })
        {
            DateRange[0] = SetDateAndTime(DateRange.First().GetValueOrDefault(), time1);
            DateRange[1] = SetDateAndTime(DateRange.Last().GetValueOrDefault(), time2);
            if (DateRange[0] > DateRange[1])
            {
                (DateRange[0], DateRange[1]) = (DateRange[1], DateRange[0]);
            }
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(DateRange);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(DateRange.Last());
        }

        return Reset();
    }

    private DateTime SetDateAndTime(DateTime date, WebenologyTime time)
    {
        var hour = time.Hour;
        if (time.Meridian == MeridianEnum.PM && hour < 12)
            hour += 12;
        else if (time is { Meridian: MeridianEnum.AM, Hour: 12 })
            hour = 0;

        return new DateTime(date.Year, date.Month, date.Day, hour, time.Minute, 0, DateTimeKind.Local);
    }

    private int GetYear(int i)
    {
        return visibleYear + i;
    }

    private Task Clear()
    {
        var dt = new List<DateTime?>();
        if (DateRangeChanged.HasDelegate)
            DateRangeChanged.InvokeAsync(dt);
        if (DateChanged.HasDelegate)
            DateChanged.InvokeAsync(null);

        return Reset();
    }

    private Task AddYear(int year)
    {
        if (CurrentDateRange is { Count: > 1 })
        {
            CurrentDateRange[0] = CurrentDateRange[0]?.AddYears(year);
            CurrentDateRange[1] = CurrentDateRange[1]?.AddYears(year);
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(CurrentDateRange);
        }
        else if (CurrentDateRange is { Count: 1 })
        {
            CurrentDateRange[0] = CurrentDateRange[0]?.AddYears(year);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(CurrentDateRange.Last());
        }
        else if (DateRange is { Count: > 1 })
        {
            DateRange[0] = DateRange[0]?.AddYears(year);
            DateRange[1] = DateRange[1]?.AddYears(year);
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(DateRange);
        }
        else if (DateRange is { Count: 1 })
        {
            DateRange[0] = DateRange[0]?.AddYears(year);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(DateRange.Last());
        }

        visibleYear += year;
        return Task.CompletedTask;
    }

    private async void UpdateCalendarPosition()
    {
        await _jsHelper.PositionCalendar(_el);
    }
}

