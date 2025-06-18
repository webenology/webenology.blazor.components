using System.Text.RegularExpressions;

namespace webenology.blazor.components.calendar
{

    public static class SpecialCaseHelper
    {
        public static Regex SpecialValueMatch = new Regex(@"^(?:(?<a>last|next) (?<a1>\d+) (?<a2>years?|days?|weeks?|months?)|(?<b>this|next|last) (?<b1>week|year|month)|(?<c>\d*) (?<c1>years?|days?|months?|weeks?) ago|(?<d>[a-z]+) ?(?<d1>\d+)? ?t?o? ?(?<d2>[a-z]+)? ?(?<d3>\d+)?|(?<e>\d+) ?t?o? ?(?<e1>\d+))$", RegexOptions.IgnoreCase);

        public static Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?> RunCase(string pattern)
        {
            if (pattern.Equals("now"))
            {
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
                return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((DateTime.Today, time),
                    null);
            }
            if (pattern.Equals("yesterday"))
            {
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
                return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((DateTime.Today.AddDays(-1), time),
                    null);
            }

            if (pattern.Equals("tomorrow"))
            {
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
                return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((DateTime.Today.AddDays(1), time),
                    null);
            }

            if (pattern.Equals("today"))
            {
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
                return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((DateTime.Today, null),
                    null);
            }

            var month1 = GetMonth(pattern);
            if (month1 > -1)
            {
                var year = DateTime.Now.Year;
                var daysInMonth = DateTime.DaysInMonth(year, month1);
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);

                return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((new DateTime(year, month1, 1), time),
                    (new DateTime(year, month1, daysInMonth), time));
            }

            //if (pattern.Equals("last week"))
            //{
            //    var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
            //        DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
            //    var lastSunday = GetLastSunday();

            //    return new Tuple<(DateTime, WebenologyTime?), (DateTime?, WebenologyTime?)?>((lastSunday.AddDays(-7), time),
            //        (lastSunday.AddDays(-1), time));
            //}
            //if (pattern.Equals("next week"))
            //{
            //    var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
            //        DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
            //    var lastSunday = GetLastSunday();

            //    return new Tuple<(DateTime, WebenologyTime?), (DateTime?, WebenologyTime?)?>((lastSunday.AddDays(7), time),
            //        (lastSunday.AddDays(14), time));
            //}

            var match = SpecialValueMatch.Match(pattern.ToLower());
            if (match.Success)
            {
                var time = new WebenologyTime(DateTime.Now.Hour, DateTime.Now.Minute,
                    DateTime.Now.Hour > 12 ? MeridianEnum.PM : MeridianEnum.AM);
                var lastSunday = GetLastSunday();

                if (match.Groups["a"].Length > 0)
                {
                    var isLast = match.Groups["a"].Value.Equals("last", StringComparison.OrdinalIgnoreCase);
                    int.TryParse(match.Groups["a1"].Value, out var aNum);
                    var type = match.Groups["a2"].Value;
                    var startDate = new DateTime();
                    var endDate = new DateTime();
                    var now = DateTime.Today;

                    if (type.StartsWith("year"))
                    {
                        var newYear = now.AddYears(aNum * (isLast ? -1 : 1));
                        startDate = isLast ? newYear : now.AddDays(+1);
                        endDate = !isLast ? newYear : now.AddDays(-1);
                    }
                    else if (type.StartsWith("month"))
                    {
                        var newMonth = now.AddMonths(aNum * (isLast ? -1 : 1));
                        startDate = isLast ? newMonth : now.AddDays(+1);
                        endDate = !isLast ? newMonth : now.AddDays(-1);
                    }
                    else if (type.StartsWith("week"))
                    {
                        var newWeek = now.AddDays(7 * aNum * (isLast ? -1 : 1));
                        startDate = isLast ? newWeek : now.AddDays(+1);
                        endDate = !isLast ? newWeek : now.AddDays(-1);
                    }
                    else if (type.StartsWith("day"))
                    {
                        var newDay = now.AddDays(aNum * (isLast ? -1 : 1));
                        startDate = isLast ? newDay : now.AddDays(+1);
                        endDate = !isLast ? newDay : now.AddDays(-1);
                    }


                    return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((startDate, time),
                        (endDate, time));

                }

                if (match.Groups["b"].Length > 0)
                {
                    var isLast = match.Groups["b"].Value.Equals("last", StringComparison.OrdinalIgnoreCase);
                    var isThis = match.Groups["b"].Value.Equals("this", StringComparison.OrdinalIgnoreCase);
                    var type = match.Groups["b1"].Value;
                    var startDate = new DateTime();
                    var endDate = new DateTime();
                    var now = DateTime.Today;
                    var currentYear = DateTime.Today.Year;

                    if (type.StartsWith("year"))
                    {
                        var newYear = now.AddYears(isThis ? 0 : isLast ? -1 : 1);
                        startDate = new DateTime(newYear.Year, 1, 1);
                        endDate = new DateTime(newYear.Year, 12, 31);
                    }
                    else if (type.StartsWith("month"))
                    {
                        var newMonth = now.AddMonths(isThis ? 0 : isLast ? -1 : 1);
                        startDate = new DateTime(currentYear, newMonth.Month, 1);
                        endDate = new DateTime(currentYear, newMonth.Month,
                            DateTime.DaysInMonth(currentYear, newMonth.Month));
                    }
                    else if (type.StartsWith("week"))
                    {
                        var newWeek = lastSunday.AddDays(7 * (isThis ? 0 : isLast ? -1 : 1));
                        startDate = newWeek;
                        endDate = newWeek.AddDays(6);
                    }

                    return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((startDate, time),
                        (endDate, time));
                }

                if (match.Groups["c"].Length > 0)
                {
                    var type = match.Groups["c1"].Value;
                    int.TryParse(match.Groups["c"].Value, out var cNum);
                    var startDate = new DateTime();
                    var endDate = new DateTime();
                    var now = DateTime.Today;
                    var currentYear = DateTime.Today.Year;

                    if (type.StartsWith("year"))
                    {
                        var newYear = now.AddYears(-cNum);
                        startDate = new DateTime(newYear.Year, 1, 1);
                        endDate = new DateTime(newYear.Year, 12, 31);
                    }
                    else if (type.StartsWith("month"))
                    {
                        var newMonth = now.AddMonths(-cNum);
                        startDate = new DateTime(currentYear, newMonth.Month, 1);
                        endDate = new DateTime(currentYear, newMonth.Month,
                            DateTime.DaysInMonth(currentYear, newMonth.Month));
                    }
                    else if (type.StartsWith("week"))
                    {
                        var newWeek = lastSunday.AddDays(-7 * cNum);
                        startDate = newWeek;
                        endDate = newWeek.AddDays(6);
                    }
                    else if (type.StartsWith("day"))
                    {
                        var newDay = now.AddDays(-cNum);
                        startDate = newDay;
                        endDate = newDay;
                    }

                    return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((startDate, time),
                        (endDate, time));
                }

                if (match.Groups["d"].Length > 0)
                {
                    var d = match.Groups["d"].Value;
                    var hasD1 = match.Groups["d1"].Length > 0;
                    var hasD2 = match.Groups["d2"].Length > 0;
                    var hasD3 = match.Groups["d3"].Length > 0;
                    var month = GetMonth(d);
                    var startDate = new DateTime();
                    var endDate = new DateTime();
                    var now = DateTime.Today;
                    var currentYear = DateTime.Today.Year;
                    int.TryParse(match.Groups["d1"].Value, out var d1Num);

                    if (month > -1 && hasD1 && !hasD2)
                    {
                        startDate = new DateTime(d1Num, month, 1);
                        endDate = new DateTime(d1Num, month, DateTime.DaysInMonth(d1Num, month));
                    }
                    else if (month > -1 && hasD2)
                    {
                        var month2 = GetMonth(match.Groups["d2"].Value);
                        month2 = month2 == -1 ? month : month2;
                        int.TryParse(match.Groups["d1"].Value, out var year1);
                        int.TryParse(match.Groups["d3"].Value, out var year2);
                        year1 = year1 == 0 ? currentYear : year1;
                        year2 = year2 == 0 ? currentYear : year2;

                        startDate = new DateTime(year1, month, 1);
                        endDate = match.Groups["d2"].Value is "now" or "date" ? now : new DateTime(year2, month2, DateTime.DaysInMonth(year2, month2));
                    }
                    else if (month == -1)
                    {
                        month = now.Month;
                        var month2 = GetMonth(match.Groups["d2"].Value);
                        month2 = month2 == -1 ? month : month2;
                        int.TryParse(match.Groups["d3"].Value, out var year2);
                        year2 = year2 == 0 ? currentYear : year2;

                        if (d.StartsWith("year"))
                        {
                            startDate = new DateTime(currentYear, 1, 1);
                        }
                        else if (d.StartsWith("month"))
                        {
                            startDate = new DateTime(currentYear, month, 1);
                        }
                        else if (d.StartsWith("week"))
                        {
                            startDate = lastSunday;
                        }

                        endDate = match.Groups["d2"].Value is "now" or "date" ? now : new DateTime(year2, month2, DateTime.DaysInMonth(year2, month2));
                    }

                    if (hasD1 || hasD2 || hasD3)
                    {
                        return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((startDate, time),
                            (endDate, time));
                    }
                }

                if (match.Groups["e"].Length > 0)
                {
                    int.TryParse(match.Groups["e"].Value, out var year1);
                    int.TryParse(match.Groups["e1"].Value, out var year2);
                    var startDate = new DateTime(year1, 1, 1);
                    var endDate = new DateTime(year2, 12, 31);

                    return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((startDate, time),
                        (endDate, time));

                }
            }

            return new Tuple<(DateTime?, WebenologyTime?), (DateTime?, WebenologyTime?)?>((null, null),
                (null, null));
        }

        public static DateTime GetLastSunday()
        {
            var today = DateTime.Today.DayOfWeek;
            var sunday = DateTime.Today.AddDays(-(int)today);
            return sunday;
        }

        private static int GetMonth(string month)
        {
            return month switch
            {
                "january" or "jan" => 1,
                "february" or "feb" => 2,
                "march" or "mar" => 3,
                "april" or "apr" => 4,
                "may" => 5,
                "june" or "jun" => 6,
                "july" or "jul" => 7,
                "august" or "aug" => 8,
                "september" or "sep" => 9,
                "october" or "oct" => 10,
                "november" or "nov" => 11,
                "december" or "dec" => 12,
                _ => -1
            };
        }
    }

}
