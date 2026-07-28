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
            var handler = new DayEndHandler();

            Assert.IsTrue(handler.CanTransitionToNext());
        }
    }
}
