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
        public void Index()
        {
            // Arrange
            var controller = new ReportController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
