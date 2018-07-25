using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class ReportControllerTest
    {
        [TestMethod]
        public void DepartmentUsageTest()
        {
            // Arrange
            var controller = new ReportController();

            // Act
            ViewResult result = controller.DepartmentUsage() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void StoreOperationsTest()
        {
            // Arrange
            var controller = new ReportController();

            // Act
            ViewResult result = controller.StoreOperations() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
