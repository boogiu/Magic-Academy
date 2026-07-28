namespace MagicAcademy.Core.Simulation
{
    /// <summary>
    /// 시뮬레이션 로그를 외부로 내보내는 인터페이스.
    /// Presentation에서는 Unity Console로, 헤드리스 실행에서는 파일 등으로 라우팅할 수 있음
    /// </summary>
    public interface ISimulationLogger
    {
        void Log(string message);
    }
}
