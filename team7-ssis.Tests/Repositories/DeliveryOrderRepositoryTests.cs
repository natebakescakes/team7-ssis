using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;

namespace team7_ssis.Repository.Tests
{
    [TestClass]
    public class DeliveryOrderRepositoryTests
    {
        [TestMethod]
        public void CountTestNotNull()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var deliveryOrderRepository = new DeliveryOrderRepository();

            // Act
            int result = deliveryOrderRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }
    }
}
