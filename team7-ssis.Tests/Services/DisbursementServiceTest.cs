using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DisbursementServiceTest
    {
        ApplicationDbContext context;
        DisbursementService disbursementService;
        DisbursementRepository disbursementRepository;
        ItemRepository itemRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            disbursementRepository = new DisbursementRepository(context);
            disbursementService = new DisbursementService(context);
            itemRepository = new ItemRepository(context);
        }
        [TestMethod]
        public void FindAllDisbursementsTest()
        {
            //Arrange
            int expected = context.Disbursement.Count();
            //Act
            var result = disbursementService.FindAllDisbursements().Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindDisbursementByIdTest()
        {
            //Arrange
            Disbursement newDisbursement = context.Disbursement.First();

            //Act
            var result = disbursementService.FindDisbursementById(newDisbursement.DisbursementId);

            //Assert
            Assert.AreEqual(newDisbursement.DisbursementId, result.DisbursementId);

        }

        [TestMethod]
        public void SaveTest()
        {
            //Arrange
            Disbursement newDisbursement = new Disbursement();
            newDisbursement.DisbursementId = IdService.GetNewDisbursementId(context);
            newDisbursement.CreatedDateTime = DateTime.Now;
            string expected = newDisbursement.DisbursementId;

            //Act
            var result = disbursementService.Save(newDisbursement);
            disbursementRepository.Delete(newDisbursement);
            //Assert
            Assert.AreEqual(expected, result.DisbursementId);

            // CLEANUP
            context.Disbursement.Remove(result);
            context.SaveChanges();
        }

        [TestMethod]
        public void ConfirmCollectionTest()
        {
            //Arrange
            Disbursement disbursement = disbursementRepository.FindById("TEST");
            disbursement.Retrieval = context.Retrieval.First();
            disbursementService.Save(disbursement);
            Disbursement expected = disbursementService.ConfirmCollection(disbursement.DisbursementId);

            //Act
            Disbursement result = context.Disbursement.Where(x => x.DisbursementId == "TEST").First();
            
            //Asert
            Assert.IsNotNull(result.CollectedBy);
            Assert.IsNotNull(result.CollectedDateTime);
        }

        [TestMethod]
         public void FindDisbursementsByRetrievalIdTest()
        {
            //Arrange

            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();
          
            Disbursement a = new Disbursement();
            a.DisbursementId = IdService.GetNewDisbursementId(context);
            a.Retrieval = retrieval;
            a.CreatedDateTime = DateTime.Now;
            disbursementService.Save(a);

            Disbursement b = new Disbursement();
            b.DisbursementId = IdService.GetNewDisbursementId(context);
            b.Retrieval = retrieval;
            b.CreatedDateTime = DateTime.Now;     
            disbursementService.Save(b);

            int expected = 2;

            //Act
            var result = disbursementService.FindDisbursementsByRetrievalId(retrieval.RetrievalId).Count;

            //Assert
            Assert.AreEqual(expected, result);

            //Delete dummy test objects in TestCleanUp
           
        }


        [TestMethod]
        public void UpdateActualQuantityForDisbursementDetailTest()
        {
            //Arrange
            Disbursement newDisbursement = context.Disbursement.Where(x=>x.DisbursementId=="TEST").First();
      
            DisbursementDetail disbursementDetail = context.DisbursementDetail
                .Where(x => x.DisbursementId == newDisbursement.DisbursementId).First();

            int expected = 5;

            //Act
            var result = disbursementService.UpdateActualQuantityForDisbursementDetail(newDisbursement.DisbursementId, disbursementDetail.ItemCode, expected);
            disbursementService.UpdateActualQuantityForDisbursementDetail(newDisbursement.DisbursementId, disbursementDetail.ItemCode, disbursementDetail.ActualQuantity);


            //Assert
            Assert.AreEqual(expected, result.DisbursementDetails.First().ActualQuantity);
          

        }

        [TestCleanup]
        public void TestCleanup()
        {
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();

            List<Disbursement> list = disbursementService.FindDisbursementsByRetrievalId(retrieval.RetrievalId);
            foreach(Disbursement d in list)
            {
                //Delete dummy test objects
                disbursementRepository.Delete(d);
              
            }

        }
    }
}
