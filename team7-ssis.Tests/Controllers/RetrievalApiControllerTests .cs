using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RetrievalApiControllerTests
    {
        ApplicationDbContext context;
        RetrievalController retrievalController;
        DepartmentService departmentService;
        RetrievalRepository retrievalRepository;
        DisbursementRepository disbursementRepository;
        ItemRepository ItemRepository;

        public RetrievalApiControllerTests()
        {
            context = new ApplicationDbContext();
            retrievalController = new RetrievalController();
            departmentService = new DepartmentService(context);
            retrievalRepository = new RetrievalRepository(context);
            disbursementRepository = new DisbursementRepository(context);
            ItemRepository = new ItemRepository(context);
            disbursementRepository = new DisbursementRepository(context);
        }

        /// <summary>
        /// Tests that Disbursement Detail that is tied to a Retrieval is updated.
        /// </summary>
        [TestMethod]
        public void UpdateRetrievalFormTest()
        {
            // ARRANGE
            RetrievalAPIController retrievalAPIController = new RetrievalAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            string retId = "RETRIEVAL";
            string deptId = "COMM";
            string itemCode = "C001";
            string disbId = "DISBURSEMENT";
            int actual = 99;

            Retrieval r = new Retrieval
            {
                RetrievalId = retId,
                CreatedDateTime = DateTime.Now
            };
            Disbursement d = new Disbursement
            {
                DisbursementId = disbId,
                Department = departmentService.FindDepartmentByDepartmentCode(deptId),
                Retrieval = r,
                CreatedDateTime = DateTime.Now,
                DisbursementDetails = new List<DisbursementDetail>
                    {
                        new DisbursementDetail
                        {
                            Item = ItemRepository.FindById(itemCode),
                            ActualQuantity = 0
                        }
                    }
            };
            retrievalRepository.Save(r);
            disbursementRepository.Save(d);

            try
            {
                SaveJson json = new SaveJson
                {
                    ItemCode = itemCode,
                    RetId = retId,
                    List = new List<BreakdownByDepartment>
                    {
                        new BreakdownByDepartment
                        {
                            DeptId = deptId,
                            Actual = actual
                        }
                    }
                };

                // ACT
                retrievalAPIController.UpdateRetrievalForm(json);
                

                // ASSERT
                int expected = 99;

                DisbursementDetail dd;
                //using (context = new ApplicationDbContext())
                //{
                    disbursementRepository = new DisbursementRepository(context);
                    dd = disbursementRepository
                        .FindById(disbId)
                        .DisbursementDetails
                        .Where(x => x.ItemCode == itemCode).First();
                //}

                Assert.AreEqual(expected, dd.ActualQuantity);

            }
            finally
            {

                //using (context = new ApplicationDbContext())
                //{
                    Disbursement deleteD = context.Disbursement.Where(x => x.DisbursementId == disbId).First();
                    context.Disbursement.Remove(deleteD);

                    Retrieval deleteR = context.Retrieval.Where(x => x.RetrievalId == retId).First();
                    context.Retrieval.Remove(deleteR);
                    context.SaveChanges();
                //}

            }

        }

        [TestMethod]
        public void GetRetrievals_ContainsResult()
        {
            // Arrange
            var expectedId = "RETCONTROLTEST";
            var expectedQuantity = 999999;
            retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = expectedId,
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(12),
                Disbursements = new List<Disbursement>()
                {
                    new Disbursement()
                    {
                        DisbursementId = expectedId,
                        Department = new DepartmentService(context).FindDepartmentByDepartmentCode("ENGL"),
                        CreatedDateTime = DateTime.Now,
                        Status = new StatusService(context).FindStatusByStatusId(7),
                        DisbursementDetails = new List<DisbursementDetail>()
                        {
                            new DisbursementDetail()
                            {
                                DisbursementId = expectedId,
                                Item = new ItemService(context).FindItemByItemCode("E030"),
                                Bin = "E78",
                                PlanQuantity = 30,
                                ActualQuantity = expectedQuantity,
                                Status = new StatusService(context).FindStatusByStatusId(17),
                                ItemCode = "E030",
                            }
                        }
                    }
                }
            });

            var controller = new RetrievalAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.GetRetrievals();

            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<RetrievalMobileViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(d => d.RetrievalId).Contains(expectedId));
            Assert.IsTrue(contentResult.Content.Select(d => d.RetrievalDetails.Select(rd => rd.ActualQuantity)).FirstOrDefault().Contains(expectedQuantity));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var disbursementRepository = new DisbursementRepository(context);
            var retrievalRepository = new RetrievalRepository(context);

            if (disbursementRepository.ExistsById("RETCONTROLTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("RETCONTROLTEST"));
            if (retrievalRepository.ExistsById("RETCONTROLTEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("RETCONTROLTEST"));
        }
    }

}
