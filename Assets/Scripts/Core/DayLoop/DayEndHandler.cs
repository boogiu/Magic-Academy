namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 수업·재정·미처리 업무·야간 사건을 정산하는 자동 페이즈.
    /// 정산 로직은 아직 구현되지 않아 진입 즉시 전환 가능하며,
    /// 하루가 끝나 다음 MorningReport로 넘어갈 때 DayCounter를 전진시킴
    /// </summary>
    public class DayEndHandler : IDayPhaseHandler
    {
        private readonly DayCounter _dayCounter;

        public DayEndHandler(DayCounter dayCounter)
        {
            _dayCounter = dayCounter;
        }

        public void Enter()
        {
        }

        public void Tick()
        {
        }

        public void Exit()
        {
            _dayCounter.AdvanceDay();
        }

        public bool CanTransitionToNext()
        {
            return true;
        }
    }
}
