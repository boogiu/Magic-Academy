namespace MagicAcademy.Core.Resources
{
    /// <summary>
    /// ActionPointBudget.Spend의 성공/실패 및 실패 사유를 담는 결과값
    /// </summary>
    public readonly struct SpendResult
    {
        public bool Succeeded { get; }
        public SpendFailureReason FailureReason { get; }

        private SpendResult(bool succeeded, SpendFailureReason failureReason)
        {
            Succeeded = succeeded;
            FailureReason = failureReason;
        }

        public static SpendResult Success()
        {
            return new SpendResult(true, SpendFailureReason.None);
        }

        public static SpendResult Fail(SpendFailureReason reason)
        {
            return new SpendResult(false, reason);
        }
    }
}
