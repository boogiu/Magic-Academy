using MagicAcademy.Core.Simulation;
using UnityEngine;

namespace MagicAcademy.Presentation.DayLoop
{
    /// <summary>
    /// ISimulationLogger를 Unity Console로 라우팅하는 구현체
    /// </summary>
    public class UnityConsoleSimulationLogger : ISimulationLogger
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
