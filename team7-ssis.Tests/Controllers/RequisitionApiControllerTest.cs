using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RequisitionApiControllerTest
    {
        private ApplicationDbContext context;
        private RequisitionRepository requisitionRepository;
        private RetrievalRepository retrievalRepository;
        private DisbursementRepository disbursementRepository;
        private DepartmentRepository departmentRepository;

        public RequisitionApiControllerTest()
        {
            context = new ApplicationDbContext();
            requisitionRepository = new RequisitionRepository(context);
            retrievalRepository = new RetrievalRepository(context);
            disbursementRepository = new DisbursementRepository(context);
            departmentRepository = new DepartmentRepository(context);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RAPICONTROLTEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RAPICONTROLTEST",
                        ItemCode = "E030",
                        Quantity = 1,
                    }
                },
                CreatedBy = new UserRepository(context).FindByEmail("EnglishEmp@email.com"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RAPICONTROLTEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RAPICONTROLTEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RAPICONTROLTEST",
                        ItemCode = "E030",
                        ActualQuantity = 1,
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });
        }

        [TestMethod]
        public void GetRelatedRequisitions_ContainsResult()
        {
            // Arrange
            var expectedId = "RAPICONTROLTEST";
            var expectedQuantity = 1;
            var controller = new RequisitionAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            // Act
            IHttpActionResult actionResult = controller.GetRelatedRequisitions(new EmailViewModel()
            {
                Email = "EnglishHead@email.com",
            });

            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<RequisitionMobileViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(d => d.RequisitionId).Contains(expectedId));
            Assert.IsTrue(contentResult.Content.Select(d => d.RequisitionDetails.Select(dd => dd.Qty)).FirstOrDefault().Contains(expectedQuantity));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (disbursementRepository.ExistsById("RAPICONTROLTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("RAPICONTROLTEST"));
            if (requisitionRepository.ExistsById("RAPICONTROLTEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("RAPICONTROLTEST"));
            if (retrievalRepository.ExistsById("RAPICONTROLTEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("RAPICONTROLTEST"));
        }
    }
}
