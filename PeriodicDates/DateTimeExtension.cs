namespace PeriodicDates
{
    public static class DateTimeExtension
    {
        public static DateTime Add(this DateTime date, RepeaterUnit unit, int value = 1)
        {
            switch (unit)
            {
                case RepeaterUnit.Minute:
                    return date.AddMinutes(value);
                case RepeaterUnit.Hour:
                    return date.AddHours(value);
                case RepeaterUnit.Day:
                    return date.AddDays(value);
                case RepeaterUnit.Week:
                    return date.AddDays(7*value);
                case RepeaterUnit.Month:
                    return date.AddMonths(value);
                case RepeaterUnit.Year:
                    return date.AddYears(value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        public static DateTime Round(this DateTime date, RepeaterUnit unit)
        {
            switch (unit)
            {
                case RepeaterUnit.Minute:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
                case RepeaterUnit.Hour:
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
                case RepeaterUnit.Day:
                    return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                case RepeaterUnit.Week:
                    {
                        var result = new DateTime(date.Year, date.Month, date.Day);
                        switch (result.DayOfWeek)
                        {
                            case DayOfWeek.Sunday:
                                result = result.AddDays(-6);
                                break;
                            case DayOfWeek.Monday:
                                break;
                            case DayOfWeek.Tuesday:
                                result = result.AddDays(-1);
                                break;
                            case DayOfWeek.Wednesday:
                                result = result.AddDays(-2);
                                break;
                            case DayOfWeek.Thursday:
                                result = result.AddDays(-3);
                                break;
                            case DayOfWeek.Friday:
                                result = result.AddDays(-4);
                                break;
                            case DayOfWeek.Saturday:
                                result = result.AddDays(-5);
                                break;
                        }
                        return result;
                    }
                case RepeaterUnit.Month:
                    return new DateTime(date.Year, date.Month, 1);
                case RepeaterUnit.Year:
                    return new DateTime(date.Year, 1, 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        public static TimeSpan GetDuration(this RepeaterUnit unit, int value = 1)
        {
            switch (unit)
            {
                case RepeaterUnit.Minute:
                    return TimeSpan.FromMinutes(value);
                case RepeaterUnit.Hour:
                    return TimeSpan.FromHours(value);
                case RepeaterUnit.Day:
                    return TimeSpan.FromDays(value);
                case RepeaterUnit.Week:
                    return TimeSpan.FromDays(7 * value);
                case RepeaterUnit.Month:
                case RepeaterUnit.Year:
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }
    }
}
