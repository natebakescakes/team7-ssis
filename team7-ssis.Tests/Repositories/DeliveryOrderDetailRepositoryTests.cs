using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DeliveryOrderDetailRepositoryTests
    {
        ApplicationDbContext context;
        DeliveryOrderRepository deliveryOrderRepository;
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            deliveryOrderRepository = new DeliveryOrderRepository(context);
            deliveryOrderDetailRepository = new DeliveryOrderDetailRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = deliveryOrderDetailRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = deliveryOrderDetailRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            var expected = new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DODREPOTEST",
                ItemCode = "E030",
            };
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            deliveryOrderDetailRepository.Save(expected);

            // Act
            var result = deliveryOrderDetailRepository.FindById("DODREPOTEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DeliveryOrderDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            var expected = new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DODREPOTEST",
                ItemCode = "E030",
            };
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            deliveryOrderDetailRepository.Save(expected);

            // Act
            var result = deliveryOrderDetailRepository.ExistsById("DODREPOTEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            deliveryOrderDetailRepository.Save(new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DODREPOTEST",
                Status = status,
                ItemCode = "E030",
            });

            // Arrange - Get Existing
            var expected = deliveryOrderDetailRepository.FindById("DODREPOTEST", "E030");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = deliveryOrderDetailRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void Save_InstanceOfType()
        {
            // Arrange
            var deliveryOrderDetail = new DeliveryOrderDetail
            {
                DeliveryOrderNo = "DODREPOTEST",
                ItemCode = "P030"
            };
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            
            // Act
            var saveResult = deliveryOrderDetailRepository.Save(deliveryOrderDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(DeliveryOrderDetail));
        }

        [TestMethod]
        public void Delete_CannotFind()
        {
            // Arrange
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DODREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            var saveResult = deliveryOrderDetailRepository.Save(new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DODREPOTEST",
                ItemCode = "E030",
            });

            // Act
            deliveryOrderDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(deliveryOrderDetailRepository.FindById("DODREPOTEST", "E030"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (deliveryOrderDetailRepository.ExistsById("DODREPOTEST", "E030"))
                deliveryOrderDetailRepository.Delete(deliveryOrderDetailRepository.FindById("DODREPOTEST", "E030"));

            if (deliveryOrderDetailRepository.ExistsById("DODREPOTEST", "P030"))
                deliveryOrderDetailRepository.Delete(deliveryOrderDetailRepository.FindById("DODREPOTEST", "P030"));

            if (deliveryOrderRepository.ExistsById("DODREPOTEST"))
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DODREPOTEST"));

        }
    }
}
