using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
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
            // Arrange
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = purchaseOrderRepository.FindById("DOREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PurchaseOrder));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = purchaseOrderRepository.ExistsById("DOREPOTEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = purchaseOrderRepository.FindById("DOREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = purchaseOrderRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var purchaseOrder = new PurchaseOrder
            {
                PurchaseOrderNo = "DOREPOTEST",
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
            Assert.IsNull(purchaseOrderRepository.FindById("DOREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = purchaseOrderRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (purchaseOrderRepository.ExistsById("DOREPOTEST"))
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DOREPOTEST"));

        }
    }
}
