using System;
using NUnit.Framework;
using PeriodicDates;

namespace TestRepeater;

public class PeriodUnitTests
{
    [SetUp]
    public void Setup()
    {
    }

    //[Test]
    //public void UnionTest()
    //{
    //    var period1 = new Period()
    //    {
    //        Begin = new DateTime(2022, 1, 1),
    //        End = new DateTime(2023, 1, 1)
    //    };
    //    var period2 = new Period()
    //    {
    //        Begin = new DateTime(2022, 5, 1),
    //        End = new DateTime(2023, 5, 1)
    //    };

    //    var union = period1.Union(period2);
    //    Assert.AreEqual(new DateTime(2022, 1, 1), union.Begin);
    //    Assert.AreEqual(new DateTime(2023, 5, 1), union.End);
    //}
}