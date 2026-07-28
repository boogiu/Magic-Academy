using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Resources;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.DayLoop
{
    public class DayLoopFSMTests
    {
        private static DayLoopFSM CreateFsm(out WorkPhaseHandler workPhaseHandler, GamePhase startingPhase = GamePhase.MorningReport)
        {
            workPhaseHandler = new WorkPhaseHandler(new ActionPointBudget());

            return new DayLoopFSM(
                new MorningReportHandler(),
                workPhaseHandler,
                new DayEndHandler(),
                startingPhase);
        }

        /// <summary>
        /// 하루는 항상 아침 보고 페이즈로 시작해야 한다는 게임 규칙을 보장
        /// </summary>
        [Test]
        public void CurrentPhase_DefaultsToMorningReport()
        {
            var fsm = CreateFsm(out _);

            Assert.AreEqual(GamePhase.MorningReport, fsm.CurrentPhase);
        }

        /// <summary>
        /// 완료 기준 핵심: Advance() 호출만으로 자동 페이즈(MorningReport, DayEnd)는
        /// 곧바로 다음 페이즈로 순환해야 함 (DayEnd -> MorningReport 순환 포함)
        /// </summary>
        [TestCase(GamePhase.MorningReport, GamePhase.Work)]
        [TestCase(GamePhase.DayEnd, GamePhase.MorningReport)]
        public void Advance_OnAutomaticPhase_MovesToNextPhaseImmediately(GamePhase startingPhase, GamePhase expectedPhase)
        {
            var fsm = CreateFsm(out _, startingPhase);

            fsm.Advance();

            Assert.AreEqual(expectedPhase, fsm.CurrentPhase);
        }

        /// <summary>
        /// Work는 행동력이 남아 있는 한, Advance()를 아무리 호출해도 머물러 있어야 함
        /// (DayLoopFSM은 행동력을 모르고, WorkPhaseHandler의 판단을 그대로 따름)
        /// </summary>
        [Test]
        public void Advance_OnWorkWithRemainingActionPoints_StaysOnWork()
        {
            var fsm = CreateFsm(out _, GamePhase.Work);

            fsm.Advance();
            fsm.Advance();

            Assert.AreEqual(GamePhase.Work, fsm.CurrentPhase);
        }

        /// <summary>
        /// 요구사항 핵심: 행동력이 모두 소진되면 별도 신호 없이도 다음 Advance()에서 DayEnd로 전환됨
        /// </summary>
        [Test]
        public void Advance_AfterActionPointsDepleted_MovesFromWorkToDayEnd()
        {
            var fsm = CreateFsm(out var workPhaseHandler, GamePhase.Work);
            workPhaseHandler.ActionPoints.Spend(ActionPointBudget.DEFAULT_MAXIMUM);

            fsm.Advance();

            Assert.AreEqual(GamePhase.DayEnd, fsm.CurrentPhase);
        }

        /// <summary>
        /// 요구사항 핵심: 행동력이 남아 있어도 명시적 종료를 요청하면 다음 Advance()에서 전환됨
        /// </summary>
        [Test]
        public void Advance_AfterEarlyEndRequested_MovesFromWorkToDayEnd()
        {
            var fsm = CreateFsm(out var workPhaseHandler, GamePhase.Work);
            workPhaseHandler.RequestEarlyEnd();

            fsm.Advance();

            Assert.AreEqual(GamePhase.DayEnd, fsm.CurrentPhase);
        }

        /// <summary>
        /// 전날 Work에서 행동력을 다 썼더라도, 다음 날 Work에 다시 들어오면
        /// 행동력이 최대치로 리필되어 있어야 함
        /// </summary>
        [Test]
        public void FullCycle_ReenteringWork_ActionPointsAreRefilled()
        {
            var fsm = CreateFsm(out var workPhaseHandler, GamePhase.Work);
            workPhaseHandler.ActionPoints.Spend(ActionPointBudget.DEFAULT_MAXIMUM);

            fsm.Advance(); // Work -> DayEnd
            fsm.Advance(); // DayEnd -> MorningReport
            fsm.Advance(); // MorningReport -> Work

            Assert.AreEqual(GamePhase.Work, fsm.CurrentPhase);
            Assert.AreEqual(ActionPointBudget.DEFAULT_MAXIMUM, workPhaseHandler.ActionPoints.Current);
        }

        /// <summary>
        /// Presentation이 CurrentPhase를 폴링하지 않고도 상태 변화를 구독으로 감지할 수 있다는 계약을 보장
        /// </summary>
        [Test]
        public void Advance_RaisesPhaseChangedWithNewPhase()
        {
            var fsm = CreateFsm(out _);
            GamePhase? raisedPhase = null;
            fsm.PhaseChanged += phase => raisedPhase = phase;

            fsm.Advance();

            Assert.AreEqual(GamePhase.Work, raisedPhase);
        }
    }
}
