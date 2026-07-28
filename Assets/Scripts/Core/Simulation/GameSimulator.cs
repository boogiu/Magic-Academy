using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Resources;

namespace MagicAcademy.Core.Simulation
{
    /// <summary>
    /// Unity 없이 하루 루프를 실행하는 최상위 진입점. 향후 헤드리스 밸런스 시뮬레이션과
    /// Presentation 디버그 패널(issue #25) 양쪽에서 재사용됨
    /// </summary>
    public class GameSimulator
    {
        private readonly ISimulationLogger _logger;
        private readonly DayLoopFSM _fsm;
        private readonly WorkPhaseHandler _workPhaseHandler;
        private readonly DayCounter _dayCounter;

        public GamePhase CurrentPhase => _fsm.CurrentPhase;
        public int CurrentDay => _dayCounter.CurrentDay;
        public int ActionPoints => _workPhaseHandler.ActionPoints.Current;

        public GameSimulator(ISimulationLogger logger)
        {
            _logger = logger;
            _dayCounter = new DayCounter();
            _workPhaseHandler = new WorkPhaseHandler(new ActionPointBudget());

            _fsm = new DayLoopFSM(new MorningReportHandler(), _workPhaseHandler, new DayEndHandler(_dayCounter));
            _fsm.PhaseChanged += OnPhaseChanged;
        }

        /// <summary>
        /// FSM을 순수하게 한 스텝 진행함 (자원 소비 없이 전환 조건만 확인)
        /// </summary>
        public void Advance()
        {
            _fsm.Advance();
        }

        /// <summary>
        /// 아직 실제 운영 결정 로직(수업 배정 등)이 없어, 임의 비용의 행동을 수행한 것으로 간주하고
        /// Work 페이즈의 행동력을 소비함. Work 페이즈가 아니면 아무 효과 없이 로그만 남김
        /// </summary>
        public void PerformAction(int cost = 1)
        {
            if (_fsm.CurrentPhase != GamePhase.Work)
            {
                _logger.Log($"[Day {CurrentDay}] Work 페이즈가 아니어서 행동을 수행할 수 없음");
                return;
            }

            SpendActionPoints(cost);
        }

        /// <summary>
        /// Work 페이즈의 행동력이 남아 있어도 하루 업무를 조기 종료함.
        /// Work 페이즈가 아니면 아무 효과 없이 로그만 남김
        /// </summary>
        public void EndWorkEarly()
        {
            if (_fsm.CurrentPhase != GamePhase.Work)
            {
                _logger.Log($"[Day {CurrentDay}] Work 페이즈가 아니어서 조기 종료를 요청할 수 없음");
                return;
            }

            _workPhaseHandler.RequestEarlyEnd();
            _logger.Log($"[Day {CurrentDay}] 하루 업무 조기 종료 요청");
        }

        /// <summary>
        /// 시뮬레이션을 한 스텝 진행함. Work 페이즈면 행동력 1을 소비한 뒤 FSM을 전진시킴.
        /// 실제 운영 결정 로직 없이도 RunDays가 자동으로 완주할 수 있게 하는 최소 정책
        /// </summary>
        public void Step()
        {
            if (_fsm.CurrentPhase == GamePhase.Work)
            {
                SpendActionPoints(1);
            }

            _fsm.Advance();
        }

        /// <summary>
        /// 지정한 일수만큼 자동으로 진행함
        /// </summary>
        public void RunDays(int count)
        {
            int targetDay = _dayCounter.CurrentDay + count;

            while (_dayCounter.CurrentDay < targetDay)
            {
                Step();
            }
        }

        private void SpendActionPoints(int cost)
        {
            var result = _workPhaseHandler.ActionPoints.Spend(cost);

            _logger.Log(result.Succeeded
                ? $"[Day {CurrentDay}] 행동력 소비: 남은 행동력 {ActionPoints}"
                : $"[Day {CurrentDay}] 행동력 소비 실패: {result.FailureReason}");
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            _logger.Log($"[Day {CurrentDay}] 페이즈 전환: {phase}");
        }
    }
}
