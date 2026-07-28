using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Resources;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.DayLoop
{
    public class WorkPhaseHandlerTests
    {
        /// <summary>
        /// 행동력이 남아 있고 조기 종료 요청도 없으면 아직 다음 페이즈로 넘어갈 수 없어야 함
        /// </summary>
        [Test]
        public void CanTransitionToNext_WithRemainingActionPoints_ReturnsFalse()
        {
            var handler = new WorkPhaseHandler(new ActionPointBudget());

            Assert.IsFalse(handler.CanTransitionToNext());
        }

        /// <summary>
        /// 요구사항 핵심: 행동력이 모두 소진되면 다음 페이즈로 전환 가능해야 함
        /// </summary>
        [Test]
        public void CanTransitionToNext_AfterActionPointsDepleted_ReturnsTrue()
        {
            var budget = new ActionPointBudget();
            var handler = new WorkPhaseHandler(budget);

            budget.Spend(ActionPointBudget.DEFAULT_MAXIMUM);

            Assert.IsTrue(handler.CanTransitionToNext());
        }

        /// <summary>
        /// 요구사항 핵심: 행동력이 남아 있어도 명시적 종료 요청이 있으면 전환 가능해야 함
        /// </summary>
        [Test]
        public void CanTransitionToNext_AfterEarlyEndRequested_ReturnsTrue()
        {
            var handler = new WorkPhaseHandler(new ActionPointBudget());

            handler.RequestEarlyEnd();

            Assert.IsTrue(handler.CanTransitionToNext());
        }

        /// <summary>
        /// 전날 다 쓴 행동력도 Work에 다시 진입하면 최대치로 리필되어야 함
        /// </summary>
        [Test]
        public void Enter_RefillsActionPoints()
        {
            var budget = new ActionPointBudget();
            var handler = new WorkPhaseHandler(budget);
            budget.Spend(ActionPointBudget.DEFAULT_MAXIMUM);

            handler.Enter();

            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, budget.Current);
        }

        /// <summary>
        /// 전날의 조기 종료 요청이 다음 날로 이어지면 안 됨 (Enter마다 초기화되어야 함)
        /// </summary>
        [Test]
        public void Enter_ResetsEarlyEndRequest()
        {
            var handler = new WorkPhaseHandler(new ActionPointBudget());
            handler.RequestEarlyEnd();

            handler.Enter();

            Assert.IsFalse(handler.CanTransitionToNext());
        }
    }
}
