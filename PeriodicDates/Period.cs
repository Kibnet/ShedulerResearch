namespace PeriodicDates;

public struct Period
{
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get=> End - Begin; set => End = Begin.Add(value); }
    public bool Excluded { get; set; }
}

//public static class PeriodExtensions
//{
//    public IEnumerable<Period> Intersections
//}