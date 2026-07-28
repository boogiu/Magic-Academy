using MagicAcademy.Core.Resources;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.Resources
{
    public class ActionPointBudgetTests
    {
        /// <summary>
        /// 새로 만들어진 예산은 항상 최대치에서 시작해야 함
        /// </summary>
        [Test]
        public void Current_DefaultsToMaximum()
        {
            var budget = new ActionPointBudget();

            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, budget.Current);
        }

        /// <summary>
        /// 정상적인 소비는 성공을 반환하고 잔여량에 정확히 반영되어야 함
        /// </summary>
        [Test]
        public void Spend_ValidAmount_SucceedsAndDecreasesCurrent()
        {
            var budget = new ActionPointBudget();

            var result = budget.Spend(2);

            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM - 2, budget.Current);
        }

        /// <summary>
        /// 방어 로직 핵심: 잔여량보다 많이 소비하려는 시도는 상태를 바꾸지 않고 실패해야 함
        /// </summary>
        [Test]
        public void Spend_MoreThanCurrent_FailsWithInsufficientActionPoints()
        {
            var budget = new ActionPointBudget();

            var result = budget.Spend(ActionPointBudget.DEFAULT_MAXIMUM + 1);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(SpendFailureReason.InsufficientActionPoints, result.FailureReason);
            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, budget.Current);
        }

        /// <summary>
        /// 방어 로직 핵심: 0 이하의 소비 시도는 상태를 바꾸지 않고 실패해야 함
        /// </summary>
        [TestCase(0)]
        [TestCase(-1)]
        public void Spend_NonPositiveAmount_FailsWithInvalidAmount(int amount)
        {
            var budget = new ActionPointBudget();

            var result = budget.Spend(amount);

            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual(SpendFailureReason.InvalidAmount, result.FailureReason);
            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, budget.Current);
        }

        /// <summary>
        /// CanSpend가 실제 Spend 결과와 일관되어야 함 (사전 확인 용도)
        /// </summary>
        [Test]
        public void CanSpend_ReflectsWhetherSpendWouldSucceed()
        {
            var budget = new ActionPointBudget();

            Assert.IsTrue(budget.CanSpend(ActionPointBudget.DEFAULT_MAXIMUM));
            Assert.IsFalse(budget.CanSpend(ActionPointBudget.DEFAULT_MAXIMUM + 1));
            Assert.IsFalse(budget.CanSpend(0));
        }

        /// <summary>
        /// 소진된 예산도 Refill 후에는 다시 최대치에서 시작해야 함 (하루가 바뀔 때 재사용됨)
        /// </summary>
        [Test]
        public void Refill_RestoresToMaximum()
        {
            var budget = new ActionPointBudget();
            budget.Spend(ActionPointBudget.DEFAULT_MAXIMUM);

            budget.Refill();

            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, budget.Current);
        }

        /// <summary>
        /// UI가 값을 폴링하지 않고도 변화를 구독으로 감지할 수 있다는 계약을 보장
        /// </summary>
        [Test]
        public void Spend_RaisesActionPointsChangedWithNewCurrent()
        {
            var budget = new ActionPointBudget();
            int? raisedValue = null;
            budget.ActionPointsChanged += value => raisedValue = value;

            budget.Spend(2);

            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM - 2, raisedValue);
        }
    }
}
