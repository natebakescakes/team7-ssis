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
        /// Tests that StationeryRetrieval view renders when a valid Retrieval ID is passed in
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

        /// <summary>
        /// Tests that StationeryDisbursement view renders when a valid Retrieval ID is passed in
        /// </summary>
        [TestMethod]
        public void StationeryDisbursementTest()
        {
            // ARRANGE

            // ACT
            ActionResult result = requisitionController.StationeryDisbursement("TEST");

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
        }
        /// <summary>
        /// Tests that RequisitionDetails view renders when a valid Requisition ID is passed in
        /// </summary>
        [TestMethod]
        public void RequisitionDetailsTest()
        {
            // ARRANGE

            // ACT
            ActionResult result = requisitionController.RequisitionDetails("TEST");

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
        }
    }
}
