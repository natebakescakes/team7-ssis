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
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
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
            // Act
            var result = deliveryOrderDetailRepository.FindById("TEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DeliveryOrderDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = deliveryOrderDetailRepository.ExistsById("TEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(14);
            var deliveryOrderDetail = deliveryOrderDetailRepository.FindById("TEST", "E030");
            var original = deliveryOrderDetail.Status;
            deliveryOrderDetail.Status = status;

            // Act
            var result = deliveryOrderDetailRepository.Save(deliveryOrderDetail);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            deliveryOrderDetail.Status = original;
            deliveryOrderDetailRepository.Save(deliveryOrderDetail);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var deliveryOrderDetail = new DeliveryOrderDetail
            {
                DeliveryOrderNo = "TEST",
                ItemCode = "P030"
            };

            // Act
            var saveResult = deliveryOrderDetailRepository.Save(deliveryOrderDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(DeliveryOrderDetail));

            // Delete saved object from DB
            // Act
            deliveryOrderDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(deliveryOrderDetailRepository.FindById("TEST", "P030"));
        }
    }
}
