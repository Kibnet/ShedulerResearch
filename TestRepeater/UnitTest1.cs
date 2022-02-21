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
    }
}