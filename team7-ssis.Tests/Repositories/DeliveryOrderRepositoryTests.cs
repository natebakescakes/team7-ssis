using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DeliveryOrderRepositoryTests
    {
        ApplicationDbContext context;
        DeliveryOrderRepository deliveryOrderRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            deliveryOrderRepository = new DeliveryOrderRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = deliveryOrderRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = deliveryOrderRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = deliveryOrderRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DeliveryOrder));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = deliveryOrderRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(14);
            var deliveryOrder = deliveryOrderRepository.FindById("TEST");
            var original = deliveryOrder.Status;
            deliveryOrder.Status = status;

            // Act
            var result = deliveryOrderRepository.Save(deliveryOrder);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            deliveryOrder.Status = original;
            deliveryOrderRepository.Save(deliveryOrder);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var deliveryOrder = new DeliveryOrder
            {
                DeliveryOrderNo = "UNIT TEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = deliveryOrderRepository.Save(deliveryOrder);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(DeliveryOrder));

            // Delete saved object from DB
            // Act
            deliveryOrderRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(deliveryOrderRepository.FindById("UNIT TEST"));
        }
    }
}
