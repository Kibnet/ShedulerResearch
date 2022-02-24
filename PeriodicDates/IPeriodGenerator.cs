namespace PeriodicDates;

public interface IPeriodGenerator
{
    Period GetCurrentPeriod(DateTime startTime);
    IEnumerable<Period> GetNextPeriods(DateTime startTime);
    IEnumerable<Period> GetPreviousPeriods(DateTime startTime);
}