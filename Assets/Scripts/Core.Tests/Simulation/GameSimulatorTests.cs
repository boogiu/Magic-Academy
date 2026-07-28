using System.Linq;
using MagicAcademy.Core.DayLoop;
using MagicAcademy.Core.Simulation;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.Simulation
{
    public class GameSimulatorTests
    {
        /// <summary>
        /// 완료 기준 핵심: Core만으로(=Unity 없이) 생성·실행 가능해야 함.
        /// 새로 만들어진 시뮬레이터는 1일차, MorningReport에서 시작해야 함
        /// </summary>
        [Test]
        public void NewSimulator_StartsAtDayOneMorningReport()
        {
            var simulator = new GameSimulator(new SimulationLog());

            Assert.AreEqual(1, simulator.CurrentDay);
            Assert.AreEqual(GamePhase.MorningReport, simulator.CurrentPhase);
        }

        /// <summary>
        /// Work 페이즈에서 Step 한 번은 행동력을 정확히 1만큼 소비해야 함
        /// </summary>
        [Test]
        public void Step_WhileInWork_ConsumesOneActionPoint()
        {
            var simulator = new GameSimulator(new SimulationLog());
            simulator.Step(); // MorningReport -> Work
            var actionPointsBeforeSpend = simulator.ActionPoints;

            simulator.Step(); // Work: 행동력 1 소비

            Assert.AreEqual(actionPointsBeforeSpend - 1, simulator.ActionPoints);
        }

        /// <summary>
        /// 요구사항 핵심: RunDays(count)는 실제 운영 결정 없이도 지정한 일수만큼 자동으로 진행되어야 함
        /// </summary>
        [Test]
        public void RunDays_AdvancesCurrentDayByCount()
        {
            var simulator = new GameSimulator(new SimulationLog());

            simulator.RunDays(3);

            Assert.AreEqual(4, simulator.CurrentDay);
        }

        /// <summary>
        /// RunDays가 끝나면 다음 하루의 Work가 시작된 상태(행동력 최대치)여야 함
        /// </summary>
        [Test]
        public void RunDays_EndsAtStartOfNextWorkPhase()
        {
            var simulator = new GameSimulator(new SimulationLog());

            simulator.RunDays(1);

            Assert.AreEqual(GamePhase.Work, simulator.CurrentPhase);
        }

        /// <summary>
        /// 완료 기준 핵심: 로그가 Debug.Log가 아니라 ISimulationLogger를 통해서만 나가야 함
        /// </summary>
        [Test]
        public void RunDays_RoutesPhaseTransitionsThroughLogger()
        {
            var log = new SimulationLog();
            var simulator = new GameSimulator(log);

            simulator.RunDays(1);

            Assert.IsNotEmpty(log.Entries);
            Assert.IsTrue(log.Entries.Any(entry => entry.Contains("DayEnd")));
            Assert.IsTrue(log.Entries.Any(entry => entry.Contains("MorningReport")));
        }

        /// <summary>
        /// Advance()는 순수 FSM 전진이므로 Work 페이즈에서 호출해도 행동력을 소비하면 안 됨
        /// </summary>
        [Test]
        public void Advance_WhileInWork_DoesNotConsumeActionPoints()
        {
            var simulator = new GameSimulator(new SimulationLog());
            simulator.Step(); // MorningReport -> Work
            var actionPointsBefore = simulator.ActionPoints;

            simulator.Advance();

            Assert.AreEqual(actionPointsBefore, simulator.ActionPoints);
            Assert.AreEqual(GamePhase.Work, simulator.CurrentPhase);
        }

        /// <summary>
        /// 디버그 패널의 "행동 시뮬레이션" 버튼 요구사항: Work 페이즈에서 지정한 비용만큼 행동력을 소비함
        /// </summary>
        [Test]
        public void PerformAction_WhileInWork_ConsumesGivenCost()
        {
            var simulator = new GameSimulator(new SimulationLog());
            simulator.Step(); // MorningReport -> Work
            var actionPointsBefore = simulator.ActionPoints;

            simulator.PerformAction(2);

            Assert.AreEqual(actionPointsBefore - 2, simulator.ActionPoints);
        }

        /// <summary>
        /// Work가 아닐 때 행동 시뮬레이션을 눌러도 상태가 깨지면 안 됨 (아무 효과 없이 무시)
        /// </summary>
        [Test]
        public void PerformAction_OutsideWork_HasNoEffect()
        {
            var simulator = new GameSimulator(new SimulationLog());

            simulator.PerformAction(1);

            Assert.AreEqual(GamePhase.MorningReport, simulator.CurrentPhase);
        }

        /// <summary>
        /// 디버그 패널의 "하루 강제 종료" 버튼 요구사항: 행동력이 남아 있어도 다음 Advance에서 DayEnd로 전환됨
        /// </summary>
        [Test]
        public void EndWorkEarly_WhileInWork_MovesToDayEndOnNextAdvance()
        {
            var simulator = new GameSimulator(new SimulationLog());
            simulator.Step(); // MorningReport -> Work

            simulator.EndWorkEarly();
            simulator.Advance();

            Assert.AreEqual(GamePhase.DayEnd, simulator.CurrentPhase);
        }

        /// <summary>
        /// Work가 아닐 때 조기 종료를 눌러도 상태가 깨지면 안 됨 (아무 효과 없이 무시)
        /// </summary>
        [Test]
        public void EndWorkEarly_OutsideWork_HasNoEffect()
        {
            var simulator = new GameSimulator(new SimulationLog());

            simulator.EndWorkEarly();
            simulator.Advance();

            Assert.AreEqual(GamePhase.Work, simulator.CurrentPhase);
        }
    }
}
