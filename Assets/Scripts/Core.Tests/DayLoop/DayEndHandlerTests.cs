using MagicAcademy.Core.DayLoop;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.DayLoop
{
    public class DayEndHandlerTests
    {
        /// <summary>
        /// 정산 로직이 아직 없는 자동 페이즈이므로 진입 즉시 다음 페이즈로 전환 가능해야 함.
        /// 정산 로직이 추가되면 이 값은 정산 완료 여부에 따라 달라져야 함
        /// </summary>
        [Test]
        public void CanTransitionToNext_AlwaysReturnsTrue()
        {
            var handler = new DayEndHandler(new DayCounter());

            Assert.IsTrue(handler.CanTransitionToNext());
        }

        /// <summary>
        /// 요구사항 핵심: DayEnd를 벗어나 다음 MorningReport로 넘어가는 시점에
        /// DayCounter가 전진해야 함
        /// </summary>
        [Test]
        public void Exit_AdvancesDayCounter()
        {
            var dayCounter = new DayCounter();
            var handler = new DayEndHandler(dayCounter);

            handler.Exit();

            Assert.AreEqual(2, dayCounter.CurrentDay);
        }
    }
}
