using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ShedulerResearch
{
    public class AgileRepeater
    {
        public RepeaterUnit Unit { get; set; }
        public RepeaterLimit Limit { get; set; }
        public DateTime? UnlimitStartDate { get; set; }
        public RepeaterPattern Pattern { get; set; }

        public DateTime GetNextOccurrence(DateTime startTime)
        {
            return GetNextOccurrences(startTime).First();
        }

        public IEnumerable<DateTime> GetNextOccurrences(DateTime startTime)
        {
            var periods = GetNextPeriods(startTime);
            foreach (var period in periods)
            {
                yield return period.Start;
            }
        }

        public struct Period
        {
            public DateTime Start { get; set; }
            public TimeSpan Duration { get; set; } 
        }

        public IEnumerable<Period> GetNextPeriods(DateTime startTime)
        {
            DateTime position = startTime;
            position = position.Round(Unit);
            if (position < startTime)
            {
                position = position.Add(Unit);
            }

            Func<DateTime, DateTime> limitCalculator;
            Func<DateTime, DateTime> limitEnlarger;
            switch (Limit)
            {
                case RepeaterLimit.Hour:
                    limitCalculator = time => time.Round(RepeaterUnit.Hour);
                    limitEnlarger = time => time.Add(RepeaterUnit.Hour);
                    break;
                case RepeaterLimit.Day:
                    limitCalculator = time => time.Round(RepeaterUnit.Day);
                    limitEnlarger = time => time.Add(RepeaterUnit.Day);
                    break;
                case RepeaterLimit.Week:
                    limitCalculator = time => time.Round(RepeaterUnit.Week);
                    limitEnlarger = time => time.Add(RepeaterUnit.Week);
                    break;
                case RepeaterLimit.Month:
                    limitCalculator = time => time.Round(RepeaterUnit.Month);
                    limitEnlarger = time => time.Add(RepeaterUnit.Month);
                    break;
                case RepeaterLimit.Year:
                    limitCalculator = time => time.Round(RepeaterUnit.Year);
                    limitEnlarger = time => time.Add(RepeaterUnit.Year);
                    break;
                case RepeaterLimit.Unlimit:
                default:
                    limitCalculator = time => UnlimitStartDate ?? position;
                    limitEnlarger = time => time;
                    break;
            }

            var limit = limitCalculator.Invoke(position);
            var offset = Pattern.Offset;
            var runer = limit.Add(Unit, offset);
            limit = limitEnlarger.Invoke(limit);
            while (true)
            {
                foreach (var patternIndex in Pattern.Indexes)
                {
                    var result = runer.Add(Unit, patternIndex);
                    if (result >= startTime)
                    {
                        TimeSpan duration = TimeSpan.Zero;//TODO
                        switch (Unit)
                        {
                            case RepeaterUnit.Minute:
                            case RepeaterUnit.Hour:
                            case RepeaterUnit.Day:
                                duration = Unit.GetDuration();
                                break;
                            case RepeaterUnit.Week:
                                duration = result.Add(RepeaterUnit.Week).Round(RepeaterUnit.Week) - result;
                                break;
                            case RepeaterUnit.Month:
                                duration = result.Add(RepeaterUnit.Month) - result;
                                break;
                            case RepeaterUnit.Year:
                                duration = result.Add(RepeaterUnit.Year) - result;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        var period = new Period()
                        {
                            Start = result,
                            Duration = duration,
                        };
                        yield return period;
                        startTime = result;
                    }
                }

                if (Pattern.Period == null)
                {
                    if (Limit == RepeaterLimit.Unlimit)
                    {
                        yield break;
                    }
                    runer = limit;
                    limit = limitEnlarger.Invoke(limit);
                }
                else
                {
                    runer = runer.Add(Unit, Pattern.Period.Value);
                    if (Limit != RepeaterLimit.Unlimit && runer > limit)
                    {
                        runer = limit;
                        limit = limitEnlarger.Invoke(limit);
                    }
                }
            }
        }
    }

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

    public class RepeaterPattern
    {
        public List<int> Indexes { get; set; }
        public int? Period { get; set; }
        public int Offset { get; set; }
    }

    public enum RepeaterUnit
    {
        Minute,
        Hour,
        Day,
        Week,
        Month,
        Year
    }

    public enum RepeaterLimit
    {
        Hour,
        Day,
        Week,
        Month,
        Year,
        Unlimit
    }
}
