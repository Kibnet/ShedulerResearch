using System.Drawing;

namespace PeriodicDates;

public class RepeaterRule
{
    public RepeaterUnit Unit { get; set; }
    public RepeaterLimit Limit { get; set; }
    public DateTime? UnlimiteStartDate { get; set; }
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
            yield return period.Begin;
        }
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
                limitCalculator = time => UnlimiteStartDate ?? position;
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
                        Begin = result,
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