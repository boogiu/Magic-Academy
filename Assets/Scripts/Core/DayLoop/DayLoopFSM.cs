using System;
using System.Collections.Generic;

namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// GamePhase 순서에 따라 IDayPhaseHandler 간 전환을 관리하는 단일 레벨 상태 기계.
    /// 행동력 등 개별 페이즈의 세부 규칙은 알지 못하며, 각 핸들러의 CanTransitionToNext에 위임함
    /// </summary>
    public class DayLoopFSM
    {
        private readonly Dictionary<GamePhase, IDayPhaseHandler> _handlers;
        private GamePhase _currentPhase;

        public GamePhase CurrentPhase => _currentPhase;

        public event Action<GamePhase> PhaseChanged;

        public DayLoopFSM(
            IDayPhaseHandler morningReportHandler,
            IDayPhaseHandler workPhaseHandler,
            IDayPhaseHandler dayEndHandler,
            GamePhase startingPhase = GamePhase.MorningReport)
        {
            _handlers = new Dictionary<GamePhase, IDayPhaseHandler>
            {
                { GamePhase.MorningReport, morningReportHandler },
                { GamePhase.Work, workPhaseHandler },
                { GamePhase.DayEnd, dayEndHandler },
            };
            _currentPhase = startingPhase;

            CurrentHandler.Enter();
        }

        private IDayPhaseHandler CurrentHandler => _handlers[_currentPhase];

        /// <summary>
        /// 현재 페이즈를 Tick하고, 전환 조건이 충족되면 다음 페이즈로 전환함
        /// </summary>
        public void Advance()
        {
            CurrentHandler.Tick();

            if (CurrentHandler.CanTransitionToNext())
            {
                TransitionToNext();
            }
        }

        private void TransitionToNext()
        {
            CurrentHandler.Exit();
            _currentPhase = GetNextPhase(_currentPhase);
            CurrentHandler.Enter();

            PhaseChanged?.Invoke(_currentPhase);
        }

        private static GamePhase GetNextPhase(GamePhase phase)
        {
            return phase switch
            {
                GamePhase.MorningReport => GamePhase.Work,
                GamePhase.Work => GamePhase.DayEnd,
                GamePhase.DayEnd => GamePhase.MorningReport,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(phase),
                    phase,
                    "정의되지 않은 게임 페이즈 입력됨")
            };
        }
    }
}
