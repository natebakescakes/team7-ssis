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
using System.Linq;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RequisitionAPIControllerTests
    {
        ApplicationDbContext context;
        RequisitionAPIController requisitionApiController;
        RequisitionRepository requisitionRepository;
        ItemService itemService;
        RetrievalService retrievalService;
        DisbursementService disbursementService;

        public RequisitionAPIControllerTests()
        {
            context = new ApplicationDbContext();
            requisitionApiController = new RequisitionAPIController();
            requisitionRepository = new RequisitionRepository(context);
            itemService = new ItemService(context);
            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);
        }
        [TestCleanup]
        public void TestCleanup()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                // CLEANUP
                context.Disbursement.RemoveRange(context.Disbursement.Where(x => x.DisbursementId != "TEST"));
                context.Requisition.RemoveRange(context.Requisition.Where(x => x.RequisitionId != "TEST"));
                context.Retrieval.RemoveRange(context.Retrieval.Where(x => x.RetrievalId != "TEST"));
                context.DisbursementDetail.RemoveRange(context.DisbursementDetail);
                context.SaveChanges();
            }
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
        /// <summary>
        /// Tests POST requests to `api/stationeryretrieval`
        /// </summary>
        [TestMethod]
        public void StationeryRetrievalPOSTTest()
        {
            // ARRANGE

            // create Retrieval with Corresponding Disbursement
            Retrieval r = new Retrieval
            {
                RetrievalId = "RETRIEVAL",
                CreatedDateTime = DateTime.Now,
                Disbursements = new List<Disbursement>()
            };
            Disbursement d = new Disbursement
            {
                DisbursementId = "DISBURSEMENT",
                CreatedDateTime = DateTime.Now,
                DisbursementDetails = new List<DisbursementDetail>()
            };
            DisbursementDetail dd = new DisbursementDetail();
            dd.PlanQuantity = 10;
            dd.ActualQuantity = 0;
            dd.Item = itemService.FindItemByItemCode("C001");

            d.DisbursementDetails.Add(dd);
            r.Disbursements.Add(d);

            Retrieval savedRetrieval = retrievalService.Save(r);
            Disbursement savedDisbursement = disbursementService.Save(d);

            // ACT
            // create mock data from DataTables
            StationeryRetrievalTableJSONViewModel viewModel = new StationeryRetrievalTableJSONViewModel();

            viewModel.RetrievalID = "RETRIEVAL";
            viewModel.Data = new List<StationeryRetrievalTableRowJSONViewModel>();
            StationeryRetrievalTableRowJSONViewModel d1 = new StationeryRetrievalTableRowJSONViewModel
            {
                AllRetrieved = true,
                ProductID = "C001"
            };
            viewModel.Data.Add(d1);

            // Call the API controller
            requisitionApiController.StationeryRetrieval(viewModel);

            // ASSERT
            // The Disbursement Detail should be updated accordingly
            //int expected = savedDisbursement.DisbursementDetails.First().ActualQuantity;

            int expected;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                expected = context.DisbursementDetail
                .Where(x => x.DisbursementId == "DISBURSEMENT")
                .Where(x => x.ItemCode == "C001").First().ActualQuantity;
            }
            
            Assert.AreEqual(expected, 10);
        }
    }
}
