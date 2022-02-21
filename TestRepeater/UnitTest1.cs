using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ShedulerResearch;

namespace TestRepeater
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Simple2Minutes()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Hour,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 2, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 4, 0), occurrences[2]);
        }

        [Test]
        public void Simple2MinutesWithOffset()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Hour,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 1 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 1, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 3, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 5, 0), occurrences[2]);
        }

        [Test]
        public void Simple7MinutesHasHourLimit()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Hour,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 7, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1, 0, 56, 0)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 56, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 1, 0, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 1, 7, 0), occurrences[2]);
        }

        [Test]
        public void Simple7MinutesHasDayLimit()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Day,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 7, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1, 0, 56, 0)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 56, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 1, 3, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 1, 10, 0), occurrences[2]);

            occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1, 23, 50, 0)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 23, 55, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 2, 0, 0, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 2, 0, 7, 0), occurrences[2]);
        }

        [Test]
        public void Simple2MinutesWithSeconds()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Hour,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1, 0, 0, 12)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 2, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 4, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 6, 0), occurrences[2]);
        }

        [Test]
        public void ComplexPatternMinutesWithSeconds()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Minute,
                Limit = RepeaterLimit.Hour,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0,3,4,7 }, Period = 8, Offset = 2 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1, 0, 0, 12)).Take(10).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 2, 0), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 5, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 6, 0), occurrences[2]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 9, 0), occurrences[3]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 10, 0), occurrences[4]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 13, 0), occurrences[5]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 14, 0), occurrences[6]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 17, 0), occurrences[7]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 18, 0), occurrences[8]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 0, 21, 0), occurrences[9]);
        }

        [Test]
        public void Simple2Hours()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Hour,
                Limit = RepeaterLimit.Day,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 2, 0, 0), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 1, 4, 0, 0), occurrences[2]);
        }

        [Test]
        public void Simple2Days()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 5), occurrences[2]);
        }

        [Test]
        public void Simple2Month()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Month,
                Limit = RepeaterLimit.Year,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 5, 1), occurrences[2]);
        }

        [Test]
        public void Simple2Year()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Year,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0]);
            Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[1]);
            Assert.AreEqual(new DateTime(2026, 1, 1), occurrences[2]);
        }
    }
}