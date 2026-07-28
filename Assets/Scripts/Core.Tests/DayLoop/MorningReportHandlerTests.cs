using MagicAcademy.Core.DayLoop;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.DayLoop
{
    public class MorningReportHandlerTests
    {
        /// <summary>
        /// 읽기만 하는 자동 페이즈이므로 진입 즉시 다음 페이즈로 전환 가능해야 함
        /// </summary>
        [Test]
        public void CanTransitionToNext_AlwaysReturnsTrue()
        {
            var handler = new MorningReportHandler();

            Assert.IsTrue(handler.CanTransitionToNext());
        }
    }
}
