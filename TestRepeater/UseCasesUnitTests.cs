using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PeriodicDates;

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
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 3 }, Period = null, Offset = 0 }
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
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 1), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //Последний день месяца", "0 10 L * *", "время");
        [Test]
        public void LastDayOfTheMonth()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { -1 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 31), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 28), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 3, 31), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //раз в год", "0 10 1 6 *", "время, месяц и число месяца");
        [Test]
        public void OneTimeOfTheYear()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Year,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2023, 1, 1), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2024, 1, 1), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2023, 1, 1) - new DateTime(2022, 1, 1), occurrences[0].Duration);
            Assert.AreEqual(new DateTime(2024, 1, 1) - new DateTime(2023, 1, 1), occurrences[1].Duration);
            Assert.AreEqual(new DateTime(2025, 1, 1) - new DateTime(2024, 1, 1), occurrences[2].Duration);
        }

        //раз в сутки", "0 10 * * *", "время");
        [Test]
        public void OneTimeOfTheDay()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 2), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //раз в неделю", "0 10 * * 1", "время и день недели");
        [Test]
        public void OneTimeOfTheWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Week,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 17), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));
        }

        //раз в месяц", "0 10 15 * *", "время и число месяца");
        [Test]
        public void OneTimeOfTheMonth()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Month,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 1, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 1), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 1) - new DateTime(2022, 1, 1), occurrences[0].Duration);
            Assert.AreEqual(new DateTime(2022, 3, 1) - new DateTime(2022, 2, 1), occurrences[1].Duration);
            Assert.AreEqual(new DateTime(2022, 4, 1) - new DateTime(2022, 3, 1), occurrences[2].Duration);
        }

        //1 раз в два месяца", "0 10 15 1/2 *", "время, с какого месяца начать и число месяца");
        [Test]
        public void OneTimeOfThe2Months()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Month,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 5, 1), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 1) - new DateTime(2022, 1, 1), occurrences[0].Duration);
            Assert.AreEqual(new DateTime(2022, 4, 1) - new DateTime(2022, 3, 1), occurrences[1].Duration);
            Assert.AreEqual(new DateTime(2022, 6, 1) - new DateTime(2022, 5, 1), occurrences[2].Duration);
        }

        //1 раз в две недели", "0 10 2/14 * *", "время и день месяца. Не совсем корректно, на границе месяца");
        [Test]
        public void OneTimeOfThe2Weeks()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Week,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 17), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 31), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(7)));
        }

        //1 раз в 4 дня", "0 10 */4 * *", "время и день месяца. Не совсем корректно, на границе месяца");
        [Test]
        public void OneTimeOfThe4Days()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 4, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 5), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 9), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //1 раз в смену", "0 2/8 * * *", "время с начала смены и длительность смены");
        [Test]
        public void OneTimeOfThe8Hours()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Hour,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 8, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 1, 8, 0, 0), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 1, 16, 0, 0), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromHours(1)));
        }

        //два раза в месяц", "0 10 1,15 * *", "дни месяца");
        [Test]
        public void TwoTimesOfTheMonth()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Month,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0, 14 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 15), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 2, 1), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //два раза в неделю", "0 10 * * 1,4", "дни недели");
        [Test]
        public void TwoTimesOfTheWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0, 3 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 6), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[2].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //три раза и тд", "0 10 * * 1,3,5", "дни недели");
        [Test]
        public void ThreeTimesOfTheWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0, 2, 4 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(6).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 5), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 7), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[3].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 12), occurrences[4].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 14), occurrences[5].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //чётная/нечётная месяц", "0 10 * 1/2 *", "время и чётность");
        [Test]
        public void NotEvenTimesOfTheMonth()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Month,
                Limit = RepeaterLimit.Year,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 3, 1), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 5, 1), occurrences[2].Begin);
        }

        //чётная/нечётная число", "0 10 1/2 * *", "время и чётность");
        [Test]
        public void NotEvenDay()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 5), occurrences[2].Begin);
        }
        //чётная/нечётная неделя", "0 10 * * 3", "время и день недели. Нужна фильтрация по номеру недели");
        [Test]
        public void NotEvenWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Week,
                Limit = RepeaterLimit.Unlimit,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0 }, Period = 2, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(3).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 17), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 31), occurrences[2].Begin);
        }

        //только будние дни", "0 10 * * 1-5");
        [Test]
        public void WorkDaysOfTheWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 0, 1, 2, 3, 4 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(6).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 3), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 4), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 5), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 6), occurrences[3].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 7), occurrences[4].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 10), occurrences[5].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //только выходные дни", "0 10 * * 6-7");
        [Test]
        public void NotWorkDaysOfTheWeek()
        {
            var repeater = new RepeaterRule
            {
                Unit = RepeaterUnit.Day,
                Limit = RepeaterLimit.Week,
                Pattern = new RepeaterPattern { Indexes = new List<int> { 5, 6 }, Period = null, Offset = 0 }
            };
            var occurrences = repeater.GetNextPeriods(new DateTime(2022, 1, 1)).Take(6).ToList();
            Assert.AreEqual(new DateTime(2022, 1, 1), occurrences[0].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 2), occurrences[1].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 8), occurrences[2].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 9), occurrences[3].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 15), occurrences[4].Begin);
            Assert.AreEqual(new DateTime(2022, 1, 16), occurrences[5].Begin);
            Assert.IsTrue(occurrences.All(period => period.Duration == TimeSpan.FromDays(1)));
        }

        //только рабочие дни", "0 10 * * *", "ничего. Нужна фильтрация по производственному календарю");//
    }
}