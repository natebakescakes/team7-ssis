using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class StockAdjustmentDetailRepositoryTests
    {
        ApplicationDbContext context;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = stockAdjustmentDetailRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = stockAdjustmentDetailRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = stockAdjustmentDetailRepository.FindById("TEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(StockAdjustmentDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = stockAdjustmentDetailRepository.ExistsById("TEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeOriginalQuantity()
        {
            // Arrange
            var stockAdjustmentDetail = stockAdjustmentDetailRepository.FindById("TEST", "E030");
            var original = stockAdjustmentDetail.OriginalQuantity;
            stockAdjustmentDetail.OriginalQuantity = 999999;

            // Act
            var result = stockAdjustmentDetailRepository.Save(stockAdjustmentDetail);

            // Assert
            Assert.AreEqual(999999, result.OriginalQuantity);

            // Tear Down
            stockAdjustmentDetail.OriginalQuantity = original;
            stockAdjustmentDetailRepository.Save(stockAdjustmentDetail);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var stockAdjustmentDetail = new StockAdjustmentDetail
            {
                StockAdjustmentId = "TEST",
                ItemCode = "P030"
            };

            // Act
            var saveResult = stockAdjustmentDetailRepository.Save(stockAdjustmentDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(StockAdjustmentDetail));

            // Delete saved object from DB
            // Act
            stockAdjustmentDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(stockAdjustmentDetailRepository.FindById("TEST", "P030"));
        }
    }
}
