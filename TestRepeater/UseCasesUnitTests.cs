using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ShedulerResearch;

namespace TestRepeater
{
    public class UseCasesUnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //Каждый четверг", "0 10 * * 4", "время");
        [Test]
        public void EveryThursday()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 3 }, Period = 7, Offset = 0 }
            };
            var occurrences = repeater.GetNextOccurrences(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 6), occurrences[0]);
            Assert.AreEqual(new DateTime(2022, 1, 13), occurrences[1]);
            Assert.AreEqual(new DateTime(2022, 1, 20), occurrences[2]);
        }

        //Первый день месяца", "0 10 1 * *", "время");
        [Test]
        public void FirstDayOfTheMonth()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Start);
            Assert.AreEqual(new DateTime(2022, 2, 1), occurrences[1].Start);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[2].Start);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //Последний день месяца", "0 10 L * *", "время");
        [Test]
        public void LastDayOfTheMonth()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { -1 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 31), occurrences[0].Start);
            Assert.AreEqual(new DateTime(2022, 2, 28), occurrences[1].Start);
            Assert.AreEqual(new DateTime(2022, 3, 31), occurrences[2].Start);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //раз в год", "0 10 1 6 *", "время, месяц и число месяца");
        [Test]
        public void OneTimeOfTheYear()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Year,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Start);
            Assert.AreEqual(new DateTime(2023, 1, 1), occurrences[1].Start);
            Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[2].Start);
            Assert.AreEqual(new DateTime(2023, 1, 1)-new DateTime(2022, 1, 1), occurrences[0].Duration);
            Assert.AreEqual(new DateTime(2024, 1, 1)-new DateTime(2023, 1, 1), occurrences[1].Duration);
            Assert.AreEqual(new DateTime(2025, 1, 1)-new DateTime(2024, 1, 1), occurrences[2].Duration);
        }

        //раз в сутки", "0 10 * * *", "время");
        [Test]
        public void OneTimeOfTheDay()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Start);
            Assert.AreEqual(new DateTime(2022, 1, 2), occurrences[1].Start);
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[2].Start);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //раз в неделю", "0 10 * * 1", "время и день недели");
        [Test]
        public void OneTimeOfTheWeek()
        {
            var repeater = new AgileRepeater
            {
                Unit = RepeaterUnit.Week,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Start);
            Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[1].Start);
            Assert.AreEqual(new DateTime(2022, 1, 17), occurrences[2].Start);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));
        }

        //раз в месяц", "0 10 15 * *", "время и число месяца");
        //1 раз в два месяца", "0 10 15 1/2 *", "время, с какого месяца начать и число месяца");
        //1 раз в две недели", "0 10 2/14 * *", "время и день месяца. Не совсем корректно, на границе месяца");
        //1 раз в 4 дня", "0 10 */4 * *", "время и день месяца. Не совсем корректно, на границе месяца");
        //1 раз в смену", "0 2/8 * * *", "время с начала смены и длительность смены");
        //два раза в месяц", "0 10 1,15 * *", "дни месяца");
        //два раза в неделю", "0 10 * * 1,4", "дни недели");
        //три раза и тд", "0 10 * * 1,3,5", "дни недели");
        //чётная/нечётная месяц", "0 10 * 1/2 *", "время и чётность");
        //чётная/нечётная число", "0 10 1/2 * *", "время и чётность");
        //чётная/нечётная неделя", "0 10 * * 3", "время и день недели. Нужна фильтрация по номеру недели");
        //только будние дни", "0 10 * * 1-5");
        //только выходные дни", "0 10 * * 6-7");
        //только рабочие дни", "0 10 * * *", "ничего. Нужна фильтрация по производственному календарю");//
    }
}