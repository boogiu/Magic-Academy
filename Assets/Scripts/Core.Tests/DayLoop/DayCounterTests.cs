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
    }
}
