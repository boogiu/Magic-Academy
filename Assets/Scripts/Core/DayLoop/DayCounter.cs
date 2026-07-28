namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// 게임 시작 이후 진행된 날짜 수를 추적함. 향후 학기(20일) 판정의 기반이 되며,
    /// 30일/개월 · 12개월/년(1년 = 360일)의 단순 판타지 달력으로 년/월/일을 환산해 노출함
    /// </summary>
    public class DayCounter
    {
        public const int STARTING_DAY = 1;
        public const int DAYS_PER_MONTH = 30;
        public const int MONTHS_PER_YEAR = 12;

        /// <summary>
        /// 게임 시작 이후 총 경과일 (1부터 시작)
        /// </summary>
        public int CurrentDay { get; private set; }

        /// <summary>년 (1부터 시작)</summary>
        public int Year => 1 + (ZeroBasedDayIndex / (DAYS_PER_MONTH * MONTHS_PER_YEAR));

        /// <summary>해당 년의 몇 번째 달인지 (1부터 시작)</summary>
        public int Month => 1 + (ZeroBasedDayIndex / DAYS_PER_MONTH) % MONTHS_PER_YEAR;

        /// <summary>해당 달의 며칠째인지 (1부터 시작)</summary>
        public int Day => 1 + ZeroBasedDayIndex % DAYS_PER_MONTH;

        private int ZeroBasedDayIndex => CurrentDay - 1;

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
