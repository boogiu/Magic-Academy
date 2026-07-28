using System;

namespace MagicAcademy.Core.DayLoop
{
    /// <summary>
    /// GamePhase에 대한 확장 기능을 제공
    /// </summary>
    public static class GamePhaseExtension
    {
        /// <summary>
        /// 해당 페이즈가 플레이어의 입력에 따라 움직이는 지 판별
        /// </summary>
        public static bool IsAutomatic(GamePhase gamePhase)
        {
            return gamePhase switch
            {
                GamePhase.MorningReport => true,
                GamePhase.Work => false,
                GamePhase.DayEnd => true,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(gamePhase),
                    gamePhase,
                    "정의되지 않은 게임 페이즈 입력됨")
            };
        }
    }
}
