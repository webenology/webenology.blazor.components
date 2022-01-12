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
            var baseDate = "MM-dd-yyyy";
            var baseTime = "hh:mm:ss tt";

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

            var baseDate = "MM-dd-yyyy";
            var baseTime = "hh:mm:ss tt";

            if (string.IsNullOrEmpty(format))
            {
                var f = $"{baseDate}";
                if (enableTime)
                    f += $" {baseTime}";

                return dt.Value.ToString(f);
            }

            return dt.Value.ToString(format);
        }

        public static string ToTimeOnly(this DateTime dt, string format)
        {
            var baseTime = "hh:mm tt";
            if (string.IsNullOrEmpty(format))
            {
                return dt.ToString(baseTime);
            }

            return dt.ToString(format);
        }

        public static string ToTimeOnly(this DateTime? dt, string format)
        {
            if (!dt.HasValue)
                return string.Empty;

            var baseTime = "hh:mm tt";
            if (string.IsNullOrEmpty(format))
            {
                return dt.Value.ToString(baseTime);
            }

            return dt.Value.ToString(format);
        }
    }
}
