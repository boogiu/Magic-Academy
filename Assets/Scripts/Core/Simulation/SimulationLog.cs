using System.Collections.Generic;

namespace MagicAcademy.Core.Simulation
{
    /// <summary>
    /// 페이즈 전환·행동력 변화 등의 로그를 문자열 리스트로 수집하는 기본 ISimulationLogger 구현
    /// </summary>
    public class SimulationLog : ISimulationLogger
    {
        private readonly List<string> _entries = new List<string>();

        public IReadOnlyList<string> Entries => _entries;

        public void Log(string message)
        {
            _entries.Add(message);
        }
    }
}
