using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RequisitionControllerTests
    {
        static ApplicationDbContext context;
        static RequisitionController requisitionController;
        static DisbursementService disbursementService;
        static RetrievalService retrievalService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            context = new ApplicationDbContext();
            requisitionController = new RequisitionController();
            disbursementService = new DisbursementService(context);
            retrievalService = new RetrievalService(context);

            // Create needed Retrieval, Disbursement, and Disbursement Detail

            retrievalService.Save(new Retrieval
            {
                RetrievalId = "TEST2",
                CreatedDateTime = DateTime.Now
            });
            Disbursement d = new Disbursement
            {
                DisbursementId = "TEST2",
                CreatedDateTime = DateTime.Now,
                DisbursementDetails = new List<DisbursementDetail>()
            };
            d.DisbursementDetails.Add(new DisbursementDetail
            {
                ItemCode = "E032",
                PlanQuantity = 1
            });
            disbursementService.Save(d);
        }
        [ClassCleanup]
        public static void TestCleanup()
        {
            context.Retrieval.Remove(retrievalService.FindRetrievalById("TEST2"));
            context.Disbursement.Remove(disbursementService.FindDisbursementById("TEST2"));
            context.SaveChanges();
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
        /// <summary>
        /// Tests thhat RetrievalDetails view renders when a valid Retrieval ID AND Item Code is passed in
        /// </summary>
        [TestMethod]
        public void RetrievalDetailsTest()
        {
            // ARRANGE

            // ACT
            ActionResult result = requisitionController.RetrievalDetails("TEST2", "E032");

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
        }
    }
}
