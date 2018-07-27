using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

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
            RetrievalAPIController retrievalAPIController = new RetrievalAPIController();

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
                // ACT
                retrievalAPIController.UpdateRetrievalForm(retId, deptId, itemCode, actual);

                // ASSERT
                int expected = 99;

                DisbursementDetail dd;
                using (context = new ApplicationDbContext())
                {
                    disbursementRepository = new DisbursementRepository(context);
                    dd = disbursementRepository
                        .FindById(disbId)
                        .DisbursementDetails
                        .Where(x => x.ItemCode == itemCode).First();
                }

                Assert.AreEqual(expected, dd.ActualQuantity);

            } finally {

                using (context = new ApplicationDbContext())
                {
                    Disbursement deleteD = context.Disbursement.Where(x => x.DisbursementId == disbId).First();
                    context.Disbursement.Remove(deleteD);

                    Retrieval deleteR = context.Retrieval.Where(x => x.RetrievalId == retId).First();
                    context.Retrieval.Remove(deleteR);
                }

            }



        }
    }
    
}
