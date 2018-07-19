using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class StockMovementTest
    {
        ApplicationDbContext context;
        StockMovementService stockmovementService;
        StockMovementRepository stockmovementRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            stockmovementRepository = new StockMovementRepository(context);
            stockmovementService = new StockMovementService(context);

        }
        [TestMethod]
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
        public void FindStockMovementByItemCodeTest()
        {
            //Arrange
            StockMovement a = context.StockMovement.First();
            int expected = context.StockMovement.Where(x => x.Item.ItemCode == a.Item.ItemCode).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByItemCode(a.Item.ItemCode).Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
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

            //Revert edit object
            result.DisbursementId = null;
            stockmovementService.Save(result);

  
        }
        [TestCleanup]
        public void TestCleanUp()
        {

        }

    }
}
