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

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 1, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 2, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 0, 3, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromMinutes(1)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromMinutes(1), currentPeriod.Duration);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1, 0, 0, 10));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromMinutes(1), currentPeriod.Duration);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 12, 31, 23, 59, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 23, 58, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 23, 57, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 23, 56, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromMinutes(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByHour()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Hour);

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 1, 0, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 2, 0, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1, 3, 0, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromHours(1)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromHours(1), currentPeriod.Duration);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1, 0, 10, 10));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromHours(1), currentPeriod.Duration);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 12, 31, 23, 0, 0), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 22, 0, 0), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 21, 0, 0), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 31, 20, 0, 0), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromHours(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByDay()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Day);

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 2), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 4), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromDays(1), currentPeriod.Duration);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1, 10, 10, 10));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromDays(1), currentPeriod.Duration);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 12, 31), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 30), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 29), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 28), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByWeek()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Week);

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 17), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 24), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 3));
        Assert.AreEqual(new DateTime(2022, 1, 3), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromDays(7), currentPeriod.Duration);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 5, 10, 10, 10));
        Assert.AreEqual(new DateTime(2022, 1, 3), currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromDays(7), currentPeriod.Duration);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 3)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 12, 27), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 20), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 13), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2021, 12, 6), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));
    }

    [Test]
    public void StaticPeriodGeneratorByMonth()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Month);

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2022, 2, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2022, 4, 1), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddMonths(1)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(currentPeriod.Begin.AddMonths(1), currentPeriod.End);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 10, 10, 10, 10));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(currentPeriod.Begin.AddMonths(1), currentPeriod.End);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 12, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 11, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2021, 10, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2021, 9, 1), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddMonths(1)));
    }

    [Test]
    public void StaticPeriodGeneratorByYear()
    {
        var generator = new StaticPeriodGenerator(RepeaterUnit.Year);

        //Next
        var occurrences = generator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2023, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2025, 1, 1), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));

        //Current
        var currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 1, 1));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(currentPeriod.Begin.AddYears(1), currentPeriod.End);

        currentPeriod = generator.GetCurrentPeriod(new DateTime(2022, 10, 10, 10, 10, 10));
        Assert.AreEqual(new DateTime(2022, 1, 1), currentPeriod.Begin);
        Assert.AreEqual(currentPeriod.Begin.AddYears(1), currentPeriod.End);

        //Previous
        occurrences = generator.GetPreviousPeriods(new DateTime(2022, 1, 1)).Take(4).ToList();
        Assert.AreEqual(new DateTime(2021, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2020, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2019, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2018, 1, 1), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
    }

    [Test]
    [TestCase(DayOfWeek.Monday)]
    [TestCase(DayOfWeek.Tuesday)]
    [TestCase(DayOfWeek.Wednesday)]
    [TestCase(DayOfWeek.Thursday)]
    [TestCase(DayOfWeek.Friday)]
    [TestCase(DayOfWeek.Saturday)]
    [TestCase(DayOfWeek.Sunday)]
    public void DayOfWeekPeriodGenerator(DayOfWeek day)
    {
        var generator = new DayOfWeekPeriodGenerator(day);

        var initDay = new DateTime(2022, 1, 2).AddDays((int)day);

        //Current
        for (int i = 1; i < 6; i++)
        {
            var notCurrentPeriod = generator.GetCurrentPeriod(initDay.AddDays(i));
            Assert.AreEqual(initDay.AddDays(1), notCurrentPeriod.Begin);
            Assert.AreEqual(TimeSpan.FromDays(6), notCurrentPeriod.Duration);
            Assert.AreEqual(true, notCurrentPeriod.Excluded);
            var notCurrentOccurrences = generator.GetNextPeriods(initDay.AddDays(1)).Take(2).ToList();
            Assert.AreEqual(initDay.AddDays(7), notCurrentOccurrences[0].Begin);
            Assert.AreEqual(initDay.AddDays(14), notCurrentOccurrences[1].Begin);
        }
        var currentPeriod = generator.GetCurrentPeriod(initDay);
        Assert.AreEqual(initDay, currentPeriod.Begin);
        Assert.AreEqual(TimeSpan.FromDays(1), currentPeriod.Duration);
        Assert.AreEqual(false, currentPeriod.Excluded);

        //Next
        var occurrences = generator.GetNextPeriods(initDay).Take(4).ToList();
        Assert.AreEqual(initDay, occurrences[0].Begin);
        Assert.AreEqual(initDay.AddDays(7), occurrences[1].Begin);
        Assert.AreEqual(initDay.AddDays(14), occurrences[2].Begin);
        Assert.AreEqual(initDay.AddDays(21), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddDays(1)));

        //Previous
        occurrences = generator.GetPreviousPeriods(initDay).Take(4).ToList();
        Assert.AreEqual(initDay.AddDays(-7), occurrences[0].Begin);
        Assert.AreEqual(initDay.AddDays(-14), occurrences[1].Begin);
        Assert.AreEqual(initDay.AddDays(-21), occurrences[2].Begin);
        Assert.AreEqual(initDay.AddDays(-28), occurrences[3].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddDays(1)));

        occurrences = generator.GetPreviousPeriods(initDay.AddDays(1)).Take(1).ToList();
        Assert.AreEqual(initDay, occurrences[0].Begin);

        occurrences = generator.GetPreviousPeriods(initDay.AddDays(1).AddTicks(1)).Take(1).ToList();
        Assert.AreEqual(initDay, occurrences[0].Begin);
    }
    
    [Test]
    public void UnlimiteRepeaterGeneratorWithOffsetNoPeriod()
    {
        var yearGenerator = new StaticPeriodGenerator(RepeaterUnit.Year);
        var pattern = new RepeaterPattern { Indexes = new List<int> { 0, 2, 3 }, Offset = 2 };
        var unlimiteGenerator = new UnlimiteRepeaterGenerator(yearGenerator, pattern, new DateTime(2022, 1, 1));

        //Next
        //Equal
        var occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2022, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(3,occurrences.Count);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
        
        //After
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2025, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(2, occurrences.Count);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));

        //Before
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2000, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(3, occurrences.Count);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
    }

    [Test]
    public void UnlimiteRepeaterGeneratorWithNoOffsetNoPeriod()
    {
        var yearGenerator = new StaticPeriodGenerator(RepeaterUnit.Year);
        var pattern = new RepeaterPattern { Indexes = new List<int> { 0, 2, 3 } };
        var unlimiteGenerator = new UnlimiteRepeaterGenerator(yearGenerator, pattern, new DateTime(2022, 1, 1));

        //Next
        var occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2022, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2025, 1, 1), occurrences[2].Begin);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
        
        //After
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2023, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2025, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(2, occurrences.Count);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));

        //Before
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2000, 1, 1)).ToList();
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2025, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(3, occurrences.Count);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
    }

    [Test]
    public void UnlimiteRepeaterGeneratorWithOffsetAndPeriod()
    {
        var yearGenerator = new StaticPeriodGenerator(RepeaterUnit.Year);
        var pattern = new RepeaterPattern { Indexes = new List<int> { 0, 2, 3 }, Offset = 2, Period = 5};
        var unlimiteGenerator = new UnlimiteRepeaterGenerator(yearGenerator, pattern, new DateTime(2022, 1, 1));

        //Next
        var occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2022, 1, 1)).Take(7).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2029, 1, 1), occurrences[3].Begin);
        Assert.AreEqual(new DateTime(2031, 1, 1), occurrences[4].Begin);
        Assert.AreEqual(new DateTime(2032, 1, 1), occurrences[5].Begin);
        Assert.AreEqual(new DateTime(2034, 1, 1), occurrences[6].Begin);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
        
        //After
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2023, 1, 1)).Take(3).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[2].Begin);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));

        //Before

        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2018, 1, 1)).Take(6).ToList();
        Assert.AreEqual(new DateTime(2019, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[3].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[4].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[5].Begin);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));

        unlimiteGenerator = new UnlimiteRepeaterGenerator(yearGenerator, pattern, new DateTime(2027, 1, 1));
        
        occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2018, 1, 1)).Take(6).ToList();
        Assert.AreEqual(new DateTime(2019, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2021, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[3].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[4].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[5].Begin);
        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
    }    
    
    [Test]
    public void UnlimiteRepeaterGeneratorWithNoOffsetAndPeriod()
    {
        var yearGenerator = new StaticPeriodGenerator(RepeaterUnit.Year);
        var pattern = new RepeaterPattern { Indexes = new List<int> { 0, 2, 3 }, Period = 5};
        var unlimiteGenerator = new UnlimiteRepeaterGenerator(yearGenerator, pattern, new DateTime(2024, 1, 1));

        //Next
        var occurrences = unlimiteGenerator.GetNextPeriods(new DateTime(2024, 1, 1)).Take(7).ToList();
        Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[0].Begin);
        Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[1].Begin);
        Assert.AreEqual(new DateTime(2027, 1, 1), occurrences[2].Begin);
        Assert.AreEqual(new DateTime(2029, 1, 1), occurrences[3].Begin);
        Assert.AreEqual(new DateTime(2031, 1, 1), occurrences[4].Begin);
        Assert.AreEqual(new DateTime(2032, 1, 1), occurrences[5].Begin);
        Assert.AreEqual(new DateTime(2034, 1, 1), occurrences[6].Begin);

        Assert.IsTrue(occurrences.All(period => period.End == period.Begin.AddYears(1)));
    }

    //[Test]
    //[TestCase(RepeaterUnit.Year)]
    //public void StaticPeriodGeneratorNotSupported(RepeaterUnit unsupportedUnit)
    //{
    //    Assert.Throws<ArgumentOutOfRangeException>(() => new StaticPeriodGenerator(unsupportedUnit));
    //}
}