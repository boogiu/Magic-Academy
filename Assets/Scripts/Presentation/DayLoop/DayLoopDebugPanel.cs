using MagicAcademy.Core.Simulation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MagicAcademy.Presentation.DayLoop
{
    /// <summary>
    /// Core의 GameSimulator를 최소한의 UI(텍스트+버튼)로 시각화하는 임시 디버그 패널.
    /// UI 아키텍처가 Phase 2에서 확정되면 정식 UI로 대체됨
    /// </summary>
    public class DayLoopDebugPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statusText;
        [SerializeField] private Button _nextStepButton;
        [SerializeField] private Button _performActionButton;
        [SerializeField] private Button _endWorkEarlyButton;

        private GameSimulator _simulator;

        private void Awake()
        {
            _simulator = new GameSimulator(new UnityConsoleSimulationLogger());

            _nextStepButton.onClick.AddListener(OnNextStepClicked);
            _performActionButton.onClick.AddListener(OnPerformActionClicked);
            _endWorkEarlyButton.onClick.AddListener(OnEndWorkEarlyClicked);

            RefreshStatusText();
        }

        private void OnDestroy()
        {
            _nextStepButton.onClick.RemoveListener(OnNextStepClicked);
            _performActionButton.onClick.RemoveListener(OnPerformActionClicked);
            _endWorkEarlyButton.onClick.RemoveListener(OnEndWorkEarlyClicked);
        }

        private void OnNextStepClicked()
        {
            _simulator.Advance();
            RefreshStatusText();
        }

        private void OnPerformActionClicked()
        {
            _simulator.PerformAction(1);
            RefreshStatusText();
        }

        private void OnEndWorkEarlyClicked()
        {
            _simulator.EndWorkEarly();
            RefreshStatusText();
        }

        private void RefreshStatusText()
        {
            _statusText.text = $"{_simulator.CurrentDay}일차 · {_simulator.CurrentPhase} · 행동력 {_simulator.ActionPoints}";
        }
    }
}
