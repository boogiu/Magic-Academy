using MagicAcademy.Core.Simulation;
using NUnit.Framework;

namespace MagicAcademy.Core.Tests.Simulation
{
    public class SimulationLogTests
    {
        /// <summary>
        /// 새로 만들어진 로그는 비어 있어야 함
        /// </summary>
        [Test]
        public void Entries_InitiallyEmpty()
        {
            var log = new SimulationLog();

            Assert.IsEmpty(log.Entries);
        }

        /// <summary>
        /// Log 호출 순서대로 메시지가 쌓여야 함
        /// </summary>
        [Test]
        public void Log_AppendsMessagesInOrder()
        {
            var log = new SimulationLog();

            log.Log("first");
            log.Log("second");

            Assert.AreEqual(new[] { "first", "second" }, log.Entries);
        }
    }
}
