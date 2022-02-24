namespace PeriodicDates;

public class StaticPeriodGenerator : IPeriodGenerator
{
    public RepeaterUnit Unit { get; }

    public StaticPeriodGenerator(RepeaterUnit unit)
    {
        switch (unit)
        {
            case RepeaterUnit.Minute:
            case RepeaterUnit.Hour:
            case RepeaterUnit.Day:
            case RepeaterUnit.Week:
            case RepeaterUnit.Month:
            case RepeaterUnit.Year:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(unit), unit, $"{Unit} not supported");
        }
        Unit = unit;
    }

    public Period GetCurrentPeriod(DateTime startTime)
    {
        DateTime position = startTime;
        position = position.Round(Unit);
        var period = new Period
        {
            Begin = position,
            End = position.Add(Unit),
        };
        return period;
    }

    public IEnumerable<Period> GetNextPeriods(DateTime startTime)
    {
        var current = GetCurrentPeriod(startTime);
        if (current.Begin >= startTime)
        {
            yield return current;
        }

        while (true)
        {
            current = new Period
            {
                Begin = current.End,
                End = current.End.Add(Unit),
            };
            yield return current;
        }
    }

    public IEnumerable<Period> GetPreviousPeriods(DateTime startTime)
    {
        var current = GetCurrentPeriod(startTime);
        if (current.End < startTime)
        {
            yield return current;
        }

        while (true)
        {
            current = new Period
            {
                Begin = current.Begin.Add(Unit, -1),
                End = current.Begin,
            };
            yield return current;
        }
    }
}