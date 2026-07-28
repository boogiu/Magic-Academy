using System;

namespace MagicAcademy.Core.Resources
{
    /// <summary>
    /// 하루 단위로 소비되는 행동력 자원. Maximum은 교장 스킬 등으로 향후 확장될 수 있어
    /// 인스턴스별로 고정값을 가짐 (현재는 확장 로직 없음)
    /// </summary>
    public class ActionPointBudget
    {
        public const int DEFAULT_MAXIMUM = 6;

        public int Maximum { get; }
        public int Current { get; private set; }

        public event Action<int> ActionPointsChanged;

        public ActionPointBudget(int maximum = DEFAULT_MAXIMUM)
        {
            Maximum = maximum;
            Current = maximum;
        }

        public bool CanSpend(int amount)
        {
            return amount > 0 && amount <= Current;
        }

        /// <summary>
        /// 행동력을 소비함. 유효하지 않은 양이거나 잔여량이 부족하면 실패를 반환함
        /// </summary>
        public SpendResult Spend(int amount)
        {
            if (amount <= 0)
            {
                return SpendResult.Fail(SpendFailureReason.InvalidAmount);
            }

            if (amount > Current)
            {
                return SpendResult.Fail(SpendFailureReason.InsufficientActionPoints);
            }

            Current -= amount;
            ActionPointsChanged?.Invoke(Current);

            return SpendResult.Success();
        }

        /// <summary>
        /// 하루 시작 시 행동력을 최대치로 초기화함
        /// </summary>
        public void Refill()
        {
            Current = Maximum;
            ActionPointsChanged?.Invoke(Current);
        }
    }
}
