using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class IdServiceTests
    {
        ApplicationDbContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetNewCollectionPointIdTest()
        {
            // Arrange
            int expected = context.CollectionPoint.OrderByDescending(x => x.CollectionPointId).FirstOrDefault().CollectionPointId + 1;
            
            // Act
            var result = IdService.GetNewCollectionPointId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNewDelegationIdTest()
        {
            // Arrange
            int expected = context.Delegation.OrderByDescending(x => x.DelegationId).FirstOrDefault().DelegationId + 1;

            // Act
            var result = IdService.GetNewDelegationId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void GetNewDeliveryOrderNoTest()
        {
            // Arrange
            string expectedPrefix = $"DO-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewDeliveryOrderNo(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 9));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod()]
        public void GetNewDisbursementIdTest()
        {
            // Arrange
            string expectedPrefix = $"DSB-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewDisbursementId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewItemCategoryIdTest()
        {
            // Arrange
            int expected = context.ItemCategory.OrderByDescending(x => x.ItemCategoryId)
                .FirstOrDefault().ItemCategoryId + 1;

            // Act
            var result = IdService.GetNewItemCategoryId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNewNotificationIdTest()
        {
            // Arrange
            int expected = context.Notification.OrderByDescending(x => x.NotificationId)
                .FirstOrDefault().NotificationId + 1;

            // Act
            var result = IdService.GetNewNotificationId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNewPurchaseOrderNoTest()
        {
            // Arrange
            string expectedPrefix = $"PO-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewPurchaseOrderNo(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 9));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewRequisitionIdTest()
        {
            // Arrange
            string expectedPrefix = $"REQ-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewRequisitionId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewRetrievalIdTest()
        {
            // Arrange
            string expectedPrefix = $"RET-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewRetrievalId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewStockAdjustmentIdTest()
        {
            // Arrange
            string expectedPrefix = $"ADJ-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewStockAdjustmentId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewStockMovementIdTest()
        {
            // Arrange
            int expected = context.StockMovement.OrderByDescending(x => x.StockMovementId)
                .FirstOrDefault().StockMovementId + 1;

            // Act
            var result = IdService.GetNewStockMovementId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
