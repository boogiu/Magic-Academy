using MagicAcademy.Core.Resources;

namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 행동력을 소비하며 학교 운영을 결정하는 플레이어 입력 페이즈.
    /// 행동력이 소진되거나 명시적으로 종료를 요청하면 다음 페이즈로 전환 가능해짐
    /// </summary>
    public class WorkPhaseHandler : IDayPhaseHandler
    {
        private bool _earlyEndRequested;

        public ActionPointBudget ActionPoints { get; }

        public WorkPhaseHandler(ActionPointBudget actionPoints)
        {
            ActionPoints = actionPoints;
        }

        public void Enter()
        {
            _earlyEndRequested = false;
            ActionPoints.Refill();
        }

        public void Tick()
        {
        }

        public void Exit()
        {
        }

        public bool CanTransitionToNext()
        {
            return ActionPoints.Current <= 0 || _earlyEndRequested;
        }

        /// <summary>
        /// 행동력이 남아 있어도 하루 업무를 조기 종료함
        /// </summary>
        public void RequestEarlyEnd()
        {
            _earlyEndRequested = true;
        }
    }
}
