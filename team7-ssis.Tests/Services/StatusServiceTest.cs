using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class StatusServiceTest
    {
        private ApplicationDbContext context;
        private StatusService statusService;
        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            statusService = new StatusService(context);
        }

        [TestMethod]
        public void FindByIdTest()
        {
            // Arrange
            var expected = "Enabled";

            // Act
            var result = statusService.FindStatusByStatusId(1);

            // Assert
            Assert.AreEqual(expected, result.Name);
        }
    }
}
