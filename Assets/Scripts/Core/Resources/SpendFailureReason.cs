namespace MagicAcademy.Core.Resources
{
    /// <summary>
    /// ActionPointBudget.Spend가 실패한 이유
    /// </summary>
    public enum SpendFailureReason
    {
        None = 0,
        InvalidAmount = 1,
        InsufficientActionPoints = 2,
    }
}
