using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class StockAdjustmentRepositoryTests
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = stockAdjustmentRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = stockAdjustmentRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = stockAdjustmentRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(StockAdjustment));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = stockAdjustmentRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeRemarks()
        {
            // Arrange
            var stockAdjustment = stockAdjustmentRepository.FindById("TEST");
            var original = stockAdjustment.Remarks;
            stockAdjustment.Remarks = "UNIT TEST";

            // Act
            var result = stockAdjustmentRepository.Save(stockAdjustment);

            // Assert
            Assert.AreEqual("UNIT TEST", result.Remarks);

            // Tear Down
            stockAdjustment.Remarks = original;
            stockAdjustmentRepository.Save(stockAdjustment);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var stockAdjustment = new StockAdjustment
            {
                StockAdjustmentId = "UNIT TEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = stockAdjustmentRepository.Save(stockAdjustment);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(StockAdjustment));

            // Delete saved object from DB
            // Act
            stockAdjustmentRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(stockAdjustmentRepository.FindById("UNIT TEST"));
        }
    }
}