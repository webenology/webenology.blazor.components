using System.Text;

using Microsoft.AspNetCore.Components;

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
    private string _wrapperCss => IsDisabled ? "wc-outline wc-disabled" : "wc-outline";
    private bool _isRangeCalendar;
    private bool _isCalendarVisible;
    private int middleMonth;
    private DateTime today = DateTime.Today;
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

    private static string[] MONTHS =
    {
        "JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER",
        "DECEMBER"
    };

    protected override void OnParametersSet()
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
        }

        base.OnParametersSet();
    }

    protected override void OnInitialized()
    {
        middleMonth = DateTime.Now.Month;
        currentYear = DateTime.Now.Year;
        visibleYear = currentYear;
        base.OnInitialized();
    }

    private Task HideCalendar()
    {
        _isCalendarVisible = false;
        return Task.CompletedTask;
    }

    private Task ToggleCalendar()
    {
        if (IsDisabled)
            return Task.CompletedTask;

        _isCalendarVisible = !_isCalendarVisible;
        return Task.CompletedTask;
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
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day);
        if (day == 1)
        {
            var skipIt = $"wc-skip-{(int)dt.DayOfWeek}";
            str.Append(skipIt);
            str.Append(" ");
        }

        var isToday = dt.Date == today;
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
        var isInMinMaxDisabled = MinDateTime.HasValue && dt <= MinDateTime.Value ||
                                 MaxDateTime.HasValue && dt >= MaxDateTime.Value;

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
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day);
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
    }

    private Task SetLastDate(int i, int day)
    {
        var monthAndYear = GetNewMonthAndYear(i);
        var dt = new DateTime(monthAndYear.Item2, monthAndYear.Item1, day);

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
                break;
            case QuickSelect.Yesterday:
                CurrentDateRange = new List<DateTime?> { today.AddDays(-1), today.AddDays(-1) };
                break;
            case QuickSelect.LastSevenDays:
                CurrentDateRange = new List<DateTime?> { today.AddDays(-6), today };
                break;
            case QuickSelect.LastWeek:
                var thisSaturday = today.AddDays(6 - (int)today.DayOfWeek);
                CurrentDateRange = new List<DateTime?> { thisSaturday.AddDays(-13), thisSaturday.AddDays(-7) };
                break;
            case QuickSelect.LastThreeWeeks:
                var thisSaturdayAgain = today.AddDays(6 - (int)today.DayOfWeek);
                CurrentDateRange = new List<DateTime?>
                    { thisSaturdayAgain.AddDays(-27), thisSaturdayAgain.AddDays(-7) };
                break;
            case QuickSelect.MonthToDate:
                var startOfMonth = new DateTime(today.Year, today.Month, 1);
                CurrentDateRange = new List<DateTime?> { startOfMonth, today };
                break;
            case QuickSelect.YearToDate:
                var startOfYear = new DateTime(today.Year, 1, 1);
                CurrentDateRange = new List<DateTime?> { startOfYear, today };
                break;
            case QuickSelect.Year:
                CurrentDateRange = new List<DateTime?> { new DateTime(today.Year, 1, 1), new DateTime(today.Year, 12, 31) };
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
            new DateTime(monthAndYear.Item2, monthAndYear.Item1, 1),
            new DateTime(monthAndYear.Item2, monthAndYear.Item1, daysInMonth),
        };
        return Task.CompletedTask;
    }

    private string GetDateRangeData()
    {
        var str = new StringBuilder();
        DateTime? date1 = null;
        DateTime? date2 = null;

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

        if (date1 == null)
        {
            str.Append("NONE");
            return str.ToString();
        }

        str.Append(date1.GetValueOrDefault().ToString("ddd, MMM dd, yyyy"));
        if (date2 != null && date1 != date2)
        {
            str.Append(" to ");
            str.Append(date2.GetValueOrDefault().ToString("ddd, MMM dd, yyyy"));
        }

        return str.ToString();
    }

    private string GetDateRangeForInput()
    {
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
        if (date2 != null && date1 != date2)
        {
            str.Append(" to ");
            str.Append(date2.GetValueOrDefault().ToString("MM-dd-yyyy"));
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
        if (CurrentDateRange != null && CurrentDateRange.Any())
        {
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(CurrentDateRange);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(CurrentDateRange.Last());
        }
        else if (DateRange.Any())
        {
            if (DateRangeChanged.HasDelegate)
                DateRangeChanged.InvokeAsync(DateRange);
            if (DateChanged.HasDelegate)
                DateChanged.InvokeAsync(DateRange.Last());
        }

        return Reset();
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
}