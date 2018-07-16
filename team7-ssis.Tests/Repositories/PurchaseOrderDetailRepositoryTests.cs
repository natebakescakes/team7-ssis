using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class PurchaseOrderDetailRepositoryTests
    {
        ApplicationDbContext context;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = purchaseOrderDetailRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = purchaseOrderDetailRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = purchaseOrderDetailRepository.FindById("TEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PurchaseOrderDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = purchaseOrderDetailRepository.ExistsById("TEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeQuantity()
        {
            // Arrange
            var purchaseOrderDetail = purchaseOrderDetailRepository.FindById("TEST", "E030");
            var original = purchaseOrderDetail.Quantity;
            purchaseOrderDetail.Quantity = 999999;

            // Act
            var result = purchaseOrderDetailRepository.Save(purchaseOrderDetail);

            // Assert
            Assert.AreEqual(999999, result.Quantity);

            // Tear Down
            purchaseOrderDetail.Quantity = original;
            purchaseOrderDetailRepository.Save(purchaseOrderDetail);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var purchaseOrderDetail = new PurchaseOrderDetail
            {
                PurchaseOrderNo = "TEST",
                ItemCode = "P030"
            };

            // Act
            var saveResult = purchaseOrderDetailRepository.Save(purchaseOrderDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(PurchaseOrderDetail));

            // Delete saved object from DB
            // Act
            purchaseOrderDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(purchaseOrderDetailRepository.FindById("TEST", "P030"));
        }
    }
}
