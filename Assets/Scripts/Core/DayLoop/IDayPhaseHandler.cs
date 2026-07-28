namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// DayLoopFSM이 관리하는 개별 페이즈의 진입·처리·종료·전환 조건을 정의
    /// </summary>
    public interface IDayPhaseHandler
    {
        void Enter();
        void Tick();
        void Exit();
        bool CanTransitionToNext();
    }
}
