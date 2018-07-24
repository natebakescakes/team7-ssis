using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RequisitionControllerTests
    {
        static RequisitionController requisitionController;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            requisitionController = new RequisitionController();
        }

        /// <summary>
        /// Tests that View renders when a valid Retrieval ID is passed in
        /// </summary>
        [TestMethod]
        public void StationeryRetrievalTest()
        {
            // ARRANGE

            // ACT
            ActionResult result = requisitionController.StationeryRetrieval("TEST");

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult) result;
        }
    }
}
