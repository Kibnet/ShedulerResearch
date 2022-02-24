namespace PeriodicDates;

public class DayOfWeekPeriodGenerator : IPeriodGenerator
{
    public DayOfWeek DayOfWeek { get; }

    public DayOfWeekPeriodGenerator(DayOfWeek dayOfWeek)
    {
        DayOfWeek = dayOfWeek;
    }

    public Period GetCurrentPeriod(DateTime startTime)
    {
        DateTime position = startTime;
        position = position.Round(RepeaterUnit.Day);
        Period period;
        if (position.DayOfWeek == DayOfWeek)
        {
            period = new Period
            {
                Begin = position,
                End = position.AddDays(1),
            };
        }
        else
        {
            var delta = position.DayOfWeek - DayOfWeek;
            if (delta <0)
            {
                delta += 7;
            }
            period = new Period
            {
                Begin = position.AddDays(1-delta),
                End = position.AddDays(7-delta),
                Excluded = true
            };
        }
        
        return period;
    }

    public IEnumerable<Period> GetNextPeriods(DateTime startTime)
    {
        var current = GetCurrentPeriod(startTime);
        if (current.Excluded == false)
        {
            yield return current;
        }
        else
        {
            current = new Period
            {
                Begin = current.End,
                End = current.End.AddDays(1),
            };
        }

        while (true)
        {
            current = new Period
            {
                Begin = current.Begin.AddDays(7),
                End = current.End.AddDays(7),
            };
            yield return current;
        }
    }

    public IEnumerable<Period> GetPreviousPeriods(DateTime startTime)
    {
        throw new NotImplementedException();
    }
}