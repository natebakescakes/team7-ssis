using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class StockAdjustmentDetailRepositoryTests
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
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
            int result = stockAdjustmentDetailRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            var expected = new StockAdjustmentDetail()
            {
                StockAdjustmentId = "SADREPOTEST",
                ItemCode = "E030",
            };
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "SADREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            stockAdjustmentDetailRepository.Save(expected);

            // Act
            var result = stockAdjustmentDetailRepository.FindById("SADREPOTEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(StockAdjustmentDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            var expected = new StockAdjustmentDetail()
            {
                StockAdjustmentId = "SADREPOTEST",
                ItemCode = "E030",
            };
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "SADREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            stockAdjustmentDetailRepository.Save(expected);

            // Act
            var result = stockAdjustmentDetailRepository.ExistsById("SADREPOTEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var afterQuantity = 1;
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "SADREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            stockAdjustmentDetailRepository.Save(new StockAdjustmentDetail()
            {
                StockAdjustmentId = "SADREPOTEST",
                AfterQuantity = afterQuantity,
                ItemCode = "E030",
            });

            // Arrange - Get Existing
            var expected = stockAdjustmentDetailRepository.FindById("SADREPOTEST", "E030");
            expected.AfterQuantity = 999;

            // Act
            var result = stockAdjustmentDetailRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.AfterQuantity, result.AfterQuantity);
        }

        [TestMethod]
        public void Save_InstanceOfType()
        {
            // Arrange
            var stockAdjustmentDetail = new StockAdjustmentDetail
            {
                StockAdjustmentId = "SADREPOTEST",
                ItemCode = "P030"
            };
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "SADREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var saveResult = stockAdjustmentDetailRepository.Save(stockAdjustmentDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(StockAdjustmentDetail));
        }

        [TestMethod]
        public void Delete_CannotFind()
        {
            // Arrange
            stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "SADREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            var saveResult = stockAdjustmentDetailRepository.Save(new StockAdjustmentDetail()
            {
                StockAdjustmentId = "SADREPOTEST",
                ItemCode = "E030",
            });

            // Act
            stockAdjustmentDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(stockAdjustmentDetailRepository.FindById("SADREPOTEST", "E030"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (stockAdjustmentDetailRepository.ExistsById("SADREPOTEST", "E030"))
                stockAdjustmentDetailRepository.Delete(stockAdjustmentDetailRepository.FindById("SADREPOTEST", "E030"));

            if (stockAdjustmentDetailRepository.ExistsById("SADREPOTEST", "P030"))
                stockAdjustmentDetailRepository.Delete(stockAdjustmentDetailRepository.FindById("SADREPOTEST", "P030"));

            if (stockAdjustmentRepository.ExistsById("SADREPOTEST"))
                stockAdjustmentRepository.Delete(stockAdjustmentRepository.FindById("SADREPOTEST"));

        }
    }
}
