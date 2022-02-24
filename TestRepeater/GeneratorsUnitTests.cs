using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PeriodicDates;

namespace TestRepeater;

public class GeneratorsUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void StaticPeriodGeneratorByMinute()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Minute);
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 1, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 2, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 3, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromMinutes(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByHour()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Hour);
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 1, 0, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 2, 0, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 3, 0, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromHours(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByDay()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Day);
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 0, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 2, 0, 0, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 3, 0, 0, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 4, 0, 0, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByWeek()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Week);
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 3, 0, 0, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 10, 0, 0, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 17, 0, 0, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 24, 0, 0, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));
    }

    [Test]
    [TestCase(RepeaterUnit.Month)]
    [TestCase(RepeaterUnit.Year)]
    public void StaticPeriodGeneratorNotSupported(RepeaterUnit unsupportedUnit)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new StaticPeriodGenerator(unsupportedUnit));
    }
}