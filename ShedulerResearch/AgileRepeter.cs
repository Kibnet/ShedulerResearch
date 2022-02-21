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
            DateTime position = startTime;
            switch (Unit)
            {
                case RepeaterUnit.Minute:
                    {
                        position = new DateTime(position.Year, position.Month, position.Day, position.Hour, position.Minute, 0);
                        if (position < startTime)
                        {
                            position = position.AddMinutes(1);
                        }
                        break;
                    }
                case RepeaterUnit.Hour:
                    {
                        position = new DateTime(position.Year, position.Month, position.Day, position.Hour, 0, 0);
                        if (position < startTime)
                        {
                            position = position.AddHours(1);
                        }
                        break;
                    }
                case RepeaterUnit.Day:
                    break;
                case RepeaterUnit.Week:
                    break;
                case RepeaterUnit.Month:
                    break;
                case RepeaterUnit.Year:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Func<DateTime, DateTime> limitCalculator;
            Func<DateTime, DateTime> limitEnlarger;
            switch (Limit)
            {
                case RepeaterLimit.Hour:
                    limitCalculator = time =>
                        new DateTime(position.Year, position.Month, position.Day, position.Hour, 0, 0);
                    limitEnlarger = time => time.AddHours(1);
                    break;
                case RepeaterLimit.Day:
                    limitCalculator = time =>
                        new DateTime(position.Year, position.Month, position.Day, 0, 0, 0);
                    limitEnlarger = time => time.AddDays(1);
                    break;
                case RepeaterLimit.Week:
                    limitCalculator = time =>
                        new DateTime(position.Year, position.Month, position.Day, position.Hour, 0, 0);
                    limitEnlarger = time => time.AddDays(7);
                    break;
                case RepeaterLimit.Month:
                    limitCalculator = time =>
                        new DateTime(position.Year, position.Month, 0, 0, 0, 0);
                    limitEnlarger = time => time.AddMonths(1);
                    break;
                case RepeaterLimit.Year:
                    limitCalculator = time =>
                        new DateTime(position.Year, 0, 0, 0, 0, 0);
                    limitEnlarger = time => time.AddYears(1);
                    break;
                case RepeaterLimit.Unlimit:
                default:
                    limitCalculator = time =>
                        UnlimitStartDate ?? position;
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
                        yield return result;
                        startTime = result;
                    }
                }

                if (Pattern.Period == null)
                {
                    yield break;
                }
                runer = runer.Add(Unit, Pattern.Period.Value);
                if (Limit != RepeaterLimit.Unlimit && runer > limit)
                {
                    runer = limit;
                    limit = limitEnlarger.Invoke(limit);
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
                    return date.AddDays(7);
                case RepeaterUnit.Month:
                    return date.AddMonths(value);
                case RepeaterUnit.Year:
                    return date.AddYears(value);
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
