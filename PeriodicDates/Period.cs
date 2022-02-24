﻿namespace PeriodicDates;

public struct Period
{
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public TimeSpan Duration { get=> End - Begin; set => End = Begin.Add(value); }
}