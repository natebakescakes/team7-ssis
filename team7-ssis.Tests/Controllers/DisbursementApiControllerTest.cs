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
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            RequisitionRepository requisitionRepository = new RequisitionRepository(context);
            RetrievalRepository retrievalRepository = new RetrievalRepository(context);
            DisbursementRepository disbursementRepository = new DisbursementRepository(context);
            DepartmentRepository departmentRepository = new DepartmentRepository(context);

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

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            RequisitionRepository requisitionRepository = new RequisitionRepository(context);
            RetrievalRepository retrievalRepository = new RetrievalRepository(context);
            DisbursementRepository disbursementRepository = new DisbursementRepository(context);
            DepartmentRepository departmentRepository = new DepartmentRepository(context);

            if (requisitionRepository.ExistsById("DAPICONTROLTEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("DAPICONTROLTEST"));
            if (disbursementRepository.ExistsById("DAPICONTROLTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("DAPICONTROLTEST"));
            if (retrievalRepository.ExistsById("DAPICONTROLTEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("DAPICONTROLTEST"));
        }
    }
}
