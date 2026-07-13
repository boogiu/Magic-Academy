namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 게임 내 하루를 구성하는 진행 페이즈
    /// </summary>
    public enum GamePhase
    {
        /// <summary>
        /// 전날의 결과와 새로 발생한 사건을 보고 받는 페이즈
        /// </summary>
        MorningReport = 0,
        /// <summary>
        /// 행동력을 소비하여 학교 운영 결정을 내리는 페이즈
        /// </summary
        Work = 1,
        /// <summary>
        /// 수업, 재정, 미처리 업무와 야간 사건을 자동으로 정산하는 페이즈
        /// </summary>
        DayEnd = 2,
    }
}
