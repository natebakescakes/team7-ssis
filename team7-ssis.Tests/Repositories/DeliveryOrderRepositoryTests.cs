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
            // Arrange
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = deliveryOrderRepository.FindById("DOREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DeliveryOrder));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = deliveryOrderRepository.ExistsById("DOREPOTEST");

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
                DeliveryOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = deliveryOrderRepository.FindById("DOREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = deliveryOrderRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var deliveryOrder = new DeliveryOrder
            {
                DeliveryOrderNo = "DOREPOTEST",
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
            Assert.IsNull(deliveryOrderRepository.FindById("DOREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            deliveryOrderRepository.Save(new DeliveryOrder()
            {
                DeliveryOrderNo = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = deliveryOrderRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (deliveryOrderRepository.ExistsById("DOREPOTEST"))
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DOREPOTEST"));

        }
    }
}
