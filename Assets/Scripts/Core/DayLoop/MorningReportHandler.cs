namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 전날 결과와 새로 발생한 사건을 보고받는 자동 페이즈. 읽기만 하므로 진입 즉시 전환 가능
    /// </summary>
    public class MorningReportHandler : IDayPhaseHandler
    {
        public void Enter()
        {
        }

        public void Tick()
        {
        }

        public void Exit()
        {
        }

        public bool CanTransitionToNext()
        {
            return true;
        }
    }
}
