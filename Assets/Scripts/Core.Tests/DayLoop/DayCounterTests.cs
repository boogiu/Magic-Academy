using MagicAcademy.Core.DayLoop;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.DayLoop
{
    public class DayCounterTests
    {
        /// <summary>
        /// 게임은 1일차부터 시작해야 함
        /// </summary>
        [Test]
        public void CurrentDay_DefaultsToOne()
        {
            var counter = new DayCounter();

            Assert.AreEqual(1, counter.CurrentDay);
        }

        /// <summary>
        /// 게임은 1년 1월 1일부터 시작해야 함
        /// </summary>
        [Test]
        public void Calendar_DefaultsToYearOneMonthOneDayOne()
        {
            var counter = new DayCounter();

            Assert.AreEqual(1, counter.Year);
            Assert.AreEqual(1, counter.Month);
            Assert.AreEqual(1, counter.Day);
        }

        /// <summary>
        /// 요구사항 핵심: 한 달(30일)의 마지막 날에서 하루가 지나면 다음 달 1일로 넘어가야 함
        /// </summary>
        [Test]
        public void Calendar_AdvancingPastEndOfMonth_RollsOverToNextMonth()
        {
            var counter = new DayCounter();
            AdvanceDays(counter, DayCounter.DAYS_PER_MONTH); // 1년 1월 30일 -> 1년 2월 1일

            Assert.AreEqual(1, counter.Year);
            Assert.AreEqual(2, counter.Month);
            Assert.AreEqual(1, counter.Day);
        }

        /// <summary>
        /// 요구사항 핵심: 마지막 달(12월)의 마지막 날에서 하루가 지나면 다음 해 1월 1일로 넘어가야 함
        /// </summary>
        [Test]
        public void Calendar_AdvancingPastEndOfYear_RollsOverToNextYear()
        {
            var counter = new DayCounter();
            AdvanceDays(counter, DayCounter.DAYS_PER_MONTH * DayCounter.MONTHS_PER_YEAR); // 1년 12월 30일 -> 2년 1월 1일

            Assert.AreEqual(2, counter.Year);
            Assert.AreEqual(1, counter.Month);
            Assert.AreEqual(1, counter.Day);
        }

        /// <summary>
        /// 완료 기준 핵심: 하루 사이클이 완료될 때마다 정확히 1씩 증가해야 함
        /// </summary>
        [Test]
        public void AdvanceDay_IncrementsCurrentDayByOne()
        {
            var counter = new DayCounter();

            counter.AdvanceDay();

            Assert.AreEqual(2, counter.CurrentDay);
        }

        /// <summary>
        /// 여러 날이 지나도 누적이 정확해야 함
        /// </summary>
        [Test]
        public void AdvanceDay_CalledMultipleTimes_AccumulatesCorrectly()
        {
            var counter = new DayCounter();

            counter.AdvanceDay();
            counter.AdvanceDay();
            counter.AdvanceDay();

            Assert.AreEqual(4, counter.CurrentDay);
        }

        private static void AdvanceDays(DayCounter counter, int count)
        {
            for (int i = 0; i < count; i++)
            {
                counter.AdvanceDay();
            }
        }
    }
}
