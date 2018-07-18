using System;
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
            Disbursement newDisbursement = new Disbursement();
            newDisbursement.DisbursementId = IdService.GetNewDisbursementId(context);
            newDisbursement.CreatedDateTime = DateTime.Now;
            disbursementRepository.Save(newDisbursement);
            string id = newDisbursement.DisbursementId;

            //Act
            var result = disbursementService.FindDisbursementById(id);

            //Assert
            Assert.AreEqual(id, result.DisbursementId);

            //Delete dummy test object
            disbursementRepository.Delete(result);
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

            //Assert
            Assert.AreEqual(expected, result.DisbursementId);
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


            //Delete dummy test objects
            disbursementRepository.Delete(a);
            disbursementRepository.Delete(b);

        }


        [TestMethod]
        public void UpdateActualQuantityForDisbursementDetailTest()
        {
            //Arrange
            Disbursement newDisbursement = context.Disbursement.First();
            Item item = itemRepository.FindById("C001");
           
            newDisbursement.DisbursementDetails.Add(new DisbursementDetail {
                Item = item,             
                ActualQuantity =3});
            disbursementService.Save(newDisbursement);
            int expected = 5;

            //Act
           var result = disbursementService.UpdateActualQuantityForDisbursementDetail(newDisbursement.DisbursementId, item.ItemCode, expected);

            //Assert
            Assert.AreEqual(expected, result.DisbursementDetails.First().ActualQuantity);

            //Delete dummy test object
            disbursementRepository.Delete(newDisbursement);
        }
    }
}
