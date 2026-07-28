namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 게임 시작 이후 진행된 날짜 수를 추적함. 향후 학기(20일) 판정의 기반이 됨
    /// </summary>
    public class DayCounter
    {
        public const int STARTING_DAY = 1;

        public int CurrentDay { get; private set; }

        public DayCounter()
        {
            CurrentDay = STARTING_DAY;
        }

        public void AdvanceDay()
        {
            CurrentDay++;
        }
    }
}
