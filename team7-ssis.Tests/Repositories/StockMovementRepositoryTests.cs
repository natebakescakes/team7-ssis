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
            // Act
            var result = stockMovementRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(StockMovement));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = stockMovementRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeOriginalQuantity()
        {
            // Arrange
            var stockMovement = stockMovementRepository.FindById(1);
            var original = stockMovement.OriginalQuantity;
            stockMovement.OriginalQuantity = 999999;

            // Act
            var result = stockMovementRepository.Save(stockMovement);

            // Assert
            Assert.AreEqual(999999, result.OriginalQuantity);

            // Tear Down
            stockMovement.OriginalQuantity = original;
            stockMovementRepository.Save(stockMovement);
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
    }
}