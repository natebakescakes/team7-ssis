using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RequisitionAPIControllerTests
    {
        static ApplicationDbContext context;
        static RequisitionAPIController requisitionApiController;
        static RequisitionRepository requisitionRepository;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            context = new ApplicationDbContext();
            requisitionApiController = new RequisitionAPIController();
            requisitionRepository = new RequisitionRepository(context);
        }
        [ClassCleanup]
        public static void TestCleanup()
        {

        }

        /// <summary>
        /// Tests that StationeryDisbursement view renders when a valid Retrieval ID is passed in
        /// </summary>
        [TestMethod]
        public void CreateRequisitionTest()
        {
            // ARRANGE
            List<CreateRequisitionJSONViewModel> data = new List<CreateRequisitionJSONViewModel>();
            data.Add(new CreateRequisitionJSONViewModel
            {
                ItemCode = "C001",
                Qty = 10
            });

            // ACT
            IHttpActionResult result = requisitionApiController.CreateRequisition(data);

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));

            // CLEANUP
            var contentResult = (OkNegotiatedContentResult<string>) result;
            Requisition r = requisitionRepository.FindById(contentResult.Content);
            requisitionRepository.Delete(r);
            
        }
    }
}
