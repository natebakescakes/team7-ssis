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
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            purchaseOrderRepository = new PurchaseOrderRepository(context);
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
            // Arrange
            var expected = new PurchaseOrderDetail()
            {
                PurchaseOrderNo = "PODREPOTEST",
                ItemCode = "E030",
            };
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "PODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            purchaseOrderDetailRepository.Save(expected);

            // Act
            var result = purchaseOrderDetailRepository.FindById("PODREPOTEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(PurchaseOrderDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            var expected = new PurchaseOrderDetail()
            {
                PurchaseOrderNo = "PODREPOTEST",
                ItemCode = "E030",
            };
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "PODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            purchaseOrderDetailRepository.Save(expected);

            // Act
            var result = purchaseOrderDetailRepository.ExistsById("PODREPOTEST", "E030");

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
                PurchaseOrderNo = "PODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            purchaseOrderDetailRepository.Save(new PurchaseOrderDetail()
            {
                PurchaseOrderNo = "PODREPOTEST",
                Status = status,
                ItemCode = "E030",
            });

            // Arrange - Get Existing
            var expected = purchaseOrderDetailRepository.FindById("PODREPOTEST", "E030");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = purchaseOrderDetailRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void Save_InstanceOfType()
        {
            // Arrange
            var purchaseOrderDetail = new PurchaseOrderDetail
            {
                PurchaseOrderNo = "PODREPOTEST",
                ItemCode = "P030"
            };
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "PODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var saveResult = purchaseOrderDetailRepository.Save(purchaseOrderDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(PurchaseOrderDetail));
        }

        [TestMethod]
        public void Delete_CannotFind()
        {
            // Arrange
            purchaseOrderRepository.Save(new PurchaseOrder()
            {
                PurchaseOrderNo = "PODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            var saveResult = purchaseOrderDetailRepository.Save(new PurchaseOrderDetail()
            {
                PurchaseOrderNo = "PODREPOTEST",
                ItemCode = "E030",
            });

            // Act
            purchaseOrderDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(purchaseOrderDetailRepository.FindById("PODREPOTEST", "E030"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (purchaseOrderDetailRepository.ExistsById("PODREPOTEST", "E030"))
                purchaseOrderDetailRepository.Delete(purchaseOrderDetailRepository.FindById("PODREPOTEST", "E030"));

            if (purchaseOrderDetailRepository.ExistsById("PODREPOTEST", "P030"))
                purchaseOrderDetailRepository.Delete(purchaseOrderDetailRepository.FindById("PODREPOTEST", "P030"));

            if (purchaseOrderRepository.ExistsById("PODREPOTEST"))
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PODREPOTEST"));

        }
    }
}
