using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;

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

        public static string ToMinMaxDtFormat(this DateTime dt, string format, bool enableTime)
        {
            var baseDate = "MM-dd-yyyy";
            var baseTime = "HH:mm";

            if (string.IsNullOrEmpty(format))
            {
                var f = $"{baseDate}";
                if (enableTime)
                    f += $" {baseTime}";

                return dt.ToString(f);
            }

            return dt.ToString(format);
        }

        public static string ToMinMaxDtFormat(this DateTime? dt, string format, bool enableTime)
        {
            if (!dt.HasValue)
                return string.Empty;

            return dt.Value.ToMinMaxDtFormat(format, enableTime);
        }

        public static string ToDtFormat(this DateTime? dt, string format, bool enableTime)
        {
            if (!dt.HasValue)
                return string.Empty;

            return dt.ToDtFormat(format, enableTime);
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

            return dt.Value.ToTimeOnly(format);
        }
    }
}