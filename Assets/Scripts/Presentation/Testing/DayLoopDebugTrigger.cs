using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Resources;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MagicAcademy.Presentation.Testing
{
    /// <summary>
    /// 정식 Presentation 브릿지가 만들어지기 전, DayLoopFSM 동작을 키 입력만으로
    /// 확인하기 위한 임시 디버그 컴포넌트.
    /// Space: Work 페이즈면 행동력 1 소비 후 Advance(), 그 외 페이즈면 바로 Advance().
    /// E: Work 페이즈에서 행동력이 남아 있어도 조기 종료.
    /// </summary>
    public class DayLoopDebugTrigger : MonoBehaviour
    {
        private DayLoopFSM _fsm;
        private WorkPhaseHandler _workPhaseHandler;
        private DayCounter _dayCounter;

        private void Awake()
        {
            _workPhaseHandler = new WorkPhaseHandler(new ActionPointBudget());
            _dayCounter = new DayCounter();
            _fsm = new DayLoopFSM(new MorningReportHandler(), _workPhaseHandler, new DayEndHandler(_dayCounter));
            _fsm.PhaseChanged += OnPhaseChanged;

            Debug.Log($"[DayLoop] 시작: {_dayCounter.CurrentDay}일차 {_fsm.CurrentPhase} (행동력 {_workPhaseHandler.ActionPoints.Current}/{_workPhaseHandler.ActionPoints.Maximum})");
        }

        private void OnDestroy()
        {
            _fsm.PhaseChanged -= OnPhaseChanged;
        }

        private void Update()
        {
            if (Keyboard.current == null)
            {
                return;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (_fsm.CurrentPhase == GamePhase.Work)
                {
                    var result = _workPhaseHandler.ActionPoints.Spend(1);
                    Debug.Log(result.Succeeded
                        ? $"[DayLoop] 행동력 소비: 남은 행동력 {_workPhaseHandler.ActionPoints.Current}"
                        : $"[DayLoop] 행동력 소비 실패: {result.FailureReason}");
                }

                _fsm.Advance();
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame && _fsm.CurrentPhase == GamePhase.Work)
            {
                _workPhaseHandler.RequestEarlyEnd();
                _fsm.Advance();
            }
        }

        private void OnPhaseChanged(GamePhase phase)
        {
            Debug.Log($"[DayLoop] 페이즈 전환: {_dayCounter.CurrentDay}일차 {phase}");
        }
    }
}
