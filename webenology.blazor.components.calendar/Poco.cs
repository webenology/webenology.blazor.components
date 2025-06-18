namespace webenology.blazor.components.calendar
{
    public class WebenologyTime
    {
        public int Hour { get; set; }
        public int Minute { get; set; }
        public MeridianEnum Meridian { get; set; }

        public WebenologyTime() : this(12, 0, MeridianEnum.AM)
        {
        }
        public WebenologyTime(int hour) : this(hour, 0, MeridianEnum.AM)
        {
        }
        public WebenologyTime(int hour, int minute) : this(hour, minute, MeridianEnum.AM)
        {
        }
        public WebenologyTime(int hour, int minute, MeridianEnum meridian)
        {
            Hour = hour;
            Minute = minute;
            Meridian = meridian;
        }

        public override string ToString()
        {
            return $"{Hour:00}:{Minute:00} {Meridian.ToString()}";
        }
    }

    public enum MeridianEnum
    {
        AM,
        PM
    }
}
