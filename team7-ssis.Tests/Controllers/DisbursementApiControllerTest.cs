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
    public class DisbursementApiControllerTest
    {
        private ApplicationDbContext context;
        private RequisitionRepository requisitionRepository;
        private RetrievalRepository retrievalRepository;
        private DisbursementRepository disbursementRepository;
        private DepartmentRepository departmentRepository;

        public DisbursementApiControllerTest()
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
                RequisitionId = "DAPICONTROLTEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "DAPICONTROLTEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DAPICONTROLTEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "DAPICONTROLTEST",
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
        public void GetAllDisbursements_ContainsResult()
        {
            // Arrange
            var expectedId = "DAPICONTROLTEST";
            var expectedQuantity = 1;
            var controller = new DisbursementAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            // Act
            IHttpActionResult actionResult = controller.GetAllDisbursements();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<DisbursementMobileViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(d => d.DisbursementId).Contains(expectedId));
            Assert.IsTrue(contentResult.Content.Select(d => d.DisbursementDetails.Select(dd => dd.Qty)).FirstOrDefault().Contains(expectedQuantity));
        }

        [TestMethod]
        public void ConfirmCollect_BadRequest()
        {
            // Arrange
            var controller = new DisbursementAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var disbursement = disbursementRepository.FindById("DAPICONTROLTEST");
            disbursement.Status = new StatusService(context).FindStatusByStatusId(10);
            disbursementRepository.Save(disbursement);

            // Act
            IHttpActionResult actionResult = controller.ConfirmCollection(new DisbursementIdViewModel()
            {
                DisbursementId = "DAPICONTROLTEST",
            });

            // Assert
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void ConfirmCollect_Valid()
        {
            // Arrange
            var expected = new StatusService(context).FindStatusByStatusId(10);
            var controller = new DisbursementAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.ConfirmCollection(new DisbursementIdViewModel()
            {
                DisbursementId = "DAPICONTROLTEST",
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Success");
            var result = new DisbursementRepository(context).FindById("DAPICONTROLTEST");
            Assert.AreEqual(expected.Name, result.Status.Name);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (disbursementRepository.ExistsById("DAPICONTROLTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("DAPICONTROLTEST"));
            if (requisitionRepository.ExistsById("DAPICONTROLTEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("DAPICONTROLTEST"));
            if (retrievalRepository.ExistsById("DAPICONTROLTEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("DAPICONTROLTEST"));
        }
    }
}
