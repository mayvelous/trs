using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using trs.backend.Services;
using trs.backend.Models;
using System.Collections.Generic;

namespace trs.backend.tests
{
    [TestClass]
    public class SimulationServiceTests
    {
        private ISimulationServices _svc;
        
        [TestInitialize()]
        public void SimulationServices_Initialize()
        {
            _svc = new SimulationServices(new RobotPosition());
        }

        [TestMethod()]
        public void PlacementConfigs_Should_Not_Null()
        {
            var result = _svc.ConfigurePlacements();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PlacementConfigs_Should_Have_4_Records()
        {
            var expected = 4;
            var result = _svc.ConfigurePlacements().Count;
            Assert.IsTrue(result == expected);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception))]
        public void Should_Get_Ex_When_InputStr_Is_NullOrEmpty()
        {
            bool isFirst = true;
            var result = _svc.GetValidatedCommand(string.Empty, ref isFirst);
            Assert.IsTrue(isFirst);
        }

        [TestMethod]
        public void Valid_Return_Type_Is_InputInfo()
        {
            bool isFirst = false;
            string cmdStr = "Move";
            var result = _svc.GetValidatedCommand(cmdStr, ref isFirst);
            Assert.IsInstanceOfType(result, typeof(InputInfo));
        }

        #region TODOs
        [TestMethod]
        public void Should_Be_Able_To_Parse_SpaceAndCommasStr()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Should_Be_A_Valid_First_Command()
        {
            Assert.Fail();
        }

        public void Should_Include_4_Vals_For_FirstCmd()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void Should_Be_A_Valid_Cmd_Key()
        {
            Assert.Fail();
        }


        [TestMethod()]
        public void PerformActionTest()
        {
            Assert.Fail();
        }
        #endregion
    }
}
