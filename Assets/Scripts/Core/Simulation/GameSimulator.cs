using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Resources;

namespace MagicAcademy.Core.Simulation
{
    /// <summary>
    /// Unity 없이 하루 루프를 실행하는 최상위 진입점. 향후 헤드리스 밸런스 시뮬레이션의 기반이 됨.
    /// 아직 실제 운영 결정 로직(수업 배정 등)이 없어, Work 페이즈에서는 매 Step마다 행동력 1을 소비해
    /// 하루가 자동으로 굴러가게 함
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
        /// 시뮬레이션을 한 스텝 진행함. Work 페이즈면 행동력 1을 소비한 뒤 FSM을 전진시킴
        /// </summary>
        public void Step()
        {
            if (_fsm.CurrentPhase == GamePhase.Work)
            {
                var result = _workPhaseHandler.ActionPoints.Spend(1);

                if (result.Succeeded)
                {
                    _logger.Log($"[Day {CurrentDay}] 행동력 소비: 남은 행동력 {ActionPoints}");
                }
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

        private void OnPhaseChanged(GamePhase phase)
        {
            _logger.Log($"[Day {CurrentDay}] 페이즈 전환: {phase}");
        }
    }
}
