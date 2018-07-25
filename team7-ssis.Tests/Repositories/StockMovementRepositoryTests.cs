using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class StockMovementRepositoryTests
    {
        ApplicationDbContext context;
        StockMovementRepository stockMovementRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockMovementRepository = new StockMovementRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = stockMovementRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = stockMovementRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            stockMovementRepository.Save(new StockMovement()
            {
                StockMovementId = 999999,
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = stockMovementRepository.FindById(999999);

            // Assert
            Assert.AreEqual(999999, result.StockMovementId);
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            stockMovementRepository.Save(new StockMovement()
            {
                StockMovementId = 999999,
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = stockMovementRepository.ExistsById(999999);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeOriginalQuantity()
        {
            // Arrange
            stockMovementRepository.Save(new StockMovement()
            {
                StockMovementId = 999999,
                CreatedDateTime = DateTime.Now,
                OriginalQuantity = 999999,
            });

            // Act
            var expected = stockMovementRepository.FindById(999999);
            expected.OriginalQuantity = 1;
            var result = stockMovementRepository.Save(expected);

            // Assert
            Assert.AreEqual(1, result.OriginalQuantity);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var stockMovement = new StockMovement
            {
                StockMovementId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = stockMovementRepository.Save(stockMovement);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(StockMovement));

            // Delete saved object from DB
            // Act
            stockMovementRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(stockMovementRepository.FindById(999999));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (stockMovementRepository.ExistsById(999999))
                stockMovementRepository.Delete(stockMovementRepository.FindById(999999));
        }
    }
}