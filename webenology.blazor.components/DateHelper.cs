using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace webenology.blazor.components
{
    internal static class DateHelper
    {
        public static string ToDtFormat(this DateTime dt, string format, bool enableTime)
        {
            var baseDate = "MM/dd/yyy";
            var baseTime = "hh:mm t";

            if (string.IsNullOrEmpty(format))
            {
                var f = $"{baseDate}";
                if (enableTime)
                    f += $" {baseTime}";

                return dt.ToString(f);
            }

            return dt.ToString(format);
        }

        public static string ToDtFormat(this DateTime? dt, string format, bool enableTime)
        {
            if (!dt.HasValue)
                return string.Empty;

            var baseDate = "MM/dd/yyy";
            var baseTime = "hh:mm t";

            if (string.IsNullOrEmpty(format))
            {
                var f = $"{baseDate}";
                if (enableTime)
                    f += $" {baseTime}";

                return dt.Value.ToString(f);
            }

            return dt.Value.ToString(format);
        }
    }
}
