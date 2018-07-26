using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class StockMovementServiceTests
    {
        ApplicationDbContext context;
        StockMovementService stockmovementService;
        StockMovementRepository stockmovementRepository;
        ItemService itemService;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            stockmovementRepository = new StockMovementRepository(context);
            stockmovementService = new StockMovementService(context);
            itemService = new ItemService(context);

        }
        [TestMethod]
        [Ignore]
        public void FindAllStockMovementServiceTest()
        {
            //Arrange
            int expected = context.StockMovement.ToList().Count;

            //Act
            var result = stockmovementService.FindAllStockMovement().Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Ignore]
        public void FindStockMovementByItemCodeTest()
        {
            //Arrange
            //StockMovement a = context.StockMovement.First();
            //int expected = context.StockMovement.Where(x => x.Item.ItemCode == a.Item.ItemCode).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByItemCode("E030");

            //Assert
            foreach(StockMovement i in result){
                Assert.AreEqual("E030", i.Item.ItemCode);
            }
        }

        [TestMethod]
        [Ignore]
        public void FindStockMovementByDisbursementIdTest()
        {
            //Arrange
            //Persist a dummy data object
            StockMovement a = new StockMovement();
            a.CreatedDateTime = DateTime.Now;
            a.DisbursementId = IdService.GetNewDisbursementId(context);
            a.Item = context.Item.First();
            stockmovementService.Save(a);

            int expected = context.StockMovement.Where(x => x.DisbursementId == a.DisbursementId).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByDisbursementId(a.DisbursementId).Count;
            stockmovementRepository.Delete(a);

            //Assert
            Assert.AreEqual(expected, result);

            
        }

        [TestMethod]
        [Ignore]
        public void FindStockMovementByStockAdjustmentIdTest()
        {
            //Arrange
            //Data is in existing db
            StockMovement a = context.StockMovement.First();
            int expected = context.StockMovement.Where(x => x.StockAdjustmentId == a.StockAdjustmentId).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByStockAdjustmentId(a.StockAdjustmentId).Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Ignore]
        public void SaveEditTest()
        {
            //save of new object is tested in FindStockMovementByDisbursementIdTest

            //Arrange
            StockMovement a = context.StockMovement.First();
            string expected = "TEST";
            a.DisbursementId = expected;
            
            //Act
            var result = stockmovementService.Save(a);

            //Assert
            Assert.AreEqual(expected, result.DisbursementId);


        }

        [TestMethod]
        [Ignore]
        public void CreateDisbursementStockMovementTest()
        {
            //Arrange
            DisbursementDetail detail = context.DisbursementDetail.First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
           var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.DisbursementId, result.DisbursementId);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }

        [TestMethod]
        [Ignore]
        public void CreateStockAdjustmentStockMovementTest()
        {
            //Arrange
            StockAdjustmentDetail detail = context.StockAdjustmentDetail.First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
            var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.StockAdjustmentId, result.StockAdjustmentId);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }
        [TestMethod]
        [Ignore]
        public void CreateDeliveryOrderStockMovementTest()
        {
            //Arrange
            DeliveryOrderDetail detail = context.DeliveryOrderDetail.First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
            var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.DeliveryOrderNo, result.DeliveryOrderNo);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            StockMovement a = context.StockMovement.Where(x => x.StockMovementId == 1).First();
            //Revert edit object
            a.DisbursementId = null;
            stockmovementService.Save(a);
        }

    }
}
