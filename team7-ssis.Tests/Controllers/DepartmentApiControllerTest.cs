using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class DepartmentApiControllerTest
    {
        private ApplicationDbContext context;

        public DepartmentApiControllerTest()
        {
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetDepartmentOptions_Valid()
        {
            // Arrange
            var expectedRepresentative = "Commerce Representative";
            var expectedEmployeeEmail = "CommerceEmp@email.com";
            var expectedDepartment = "Commerce Dept";

            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            // Act
            IHttpActionResult actionResult = controller.GetDepartmentOptions(new EmailViewModel()
            {
                Email = "CommerceHead@email.com",
            });

            var contentResult = actionResult as OkNegotiatedContentResult<DepartmentOptionsViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedDepartment, contentResult.Content.Department);
            Assert.AreEqual(expectedRepresentative, contentResult.Content.Representative);
            Assert.IsTrue(contentResult.Content.Employees.Select(e => e.Email).Contains(expectedEmployeeEmail));
        }
    }
}
