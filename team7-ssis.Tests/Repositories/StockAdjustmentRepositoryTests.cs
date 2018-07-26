using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
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
            int result = stockAdjustmentRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = stockAdjustmentRepository.FindById("DOREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(StockAdjustment));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = stockAdjustmentRepository.ExistsById("DOREPOTEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = stockAdjustmentRepository.FindById("DOREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = stockAdjustmentRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var stockAdjustment = new StockAdjustment
            {
                StockAdjustmentId = "DOREPOTEST",
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
            Assert.IsNull(stockAdjustmentRepository.FindById("DOREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = stockAdjustmentRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (stockAdjustmentRepository.ExistsById("DOREPOTEST"))
                stockAdjustmentRepository.Delete(stockAdjustmentRepository.FindById("DOREPOTEST"));

        }
    }
}
