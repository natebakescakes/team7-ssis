using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class PurchaseOrderRepositoryTests
    {
        ApplicationDbContext context;
        PurchaseOrderRepository purchaseOrderRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            purchaseOrderRepository = new PurchaseOrderRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = purchaseOrderRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = purchaseOrderRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = purchaseOrderRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PurchaseOrder));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = purchaseOrderRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(15);
            var purchaseOrder = purchaseOrderRepository.FindById("TEST");
            var original = purchaseOrder.Status;
            purchaseOrder.Status = status;

            // Act
            var result = purchaseOrderRepository.Save(purchaseOrder);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            purchaseOrder.Status = original;
            purchaseOrderRepository.Save(purchaseOrder);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderNo = "UNIT TEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = purchaseOrderRepository.Save(purchaseOrder);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(PurchaseOrder));

            // Delete saved object from DB
            // Act
            purchaseOrderRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(purchaseOrderRepository.FindById("UNIT TEST"));
        }
    }
}