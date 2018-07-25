using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class UserApiControllerTest
    {
        private ApplicationDbContext context;
        
        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetSupervisors_ContainsResult()
        {
            // Arrange
            var userService = new UserService(context);
            var user = userService.FindUserByEmail("root@admin.com");
            user.Department = new DepartmentService(context).FindDepartmentByDepartmentCode("ENGL");
            userService.Save(user);
            
            var expected = "Admin ";
            var controller = new UserApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetSupervisorsFromDepartment("ENGL");
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<EmailNameViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(x => x.Name).Contains(expected));
        }

        [TestMethod]
        public void GetSupervisors_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserApiController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetSupervisorsFromDepartment("XXXX");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUsersFromDepartment_ContainsResult()
        {
            // Arrange
            var userService = new UserService(context);
            var user = userService.FindUserByEmail("root@admin.com");
            user.Department = new DepartmentService(context).FindDepartmentByDepartmentCode("ENGL");
            userService.Save(user);

            var expected = "Admin ";
            var controller = new UserApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetUsersFromDepartment("ENGL");
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<EmailNameViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(x => x.Name).Contains(expected));
        }

        [TestMethod]
        public void GetUsersFromDepartment_ReturnsNotFound()
        {
            // Arrange
            var controller = new UserApiController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetUsersFromDepartment("XXXX");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        [Ignore]
        public void GetRoles_ContainsResult()
        {
            // Arrange
            var userService = new UserService(context);
            var email = "root@admin.com";

            var expected = "Admin";
            var controller = new UserApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetRoles(new EmailNameViewModel()
            {
                Email = email,
            });
            var contentResult = actionResult as OkNegotiatedContentResult<RoleViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expected, contentResult.Content.Roles.FirstOrDefault());
        }

        [TestMethod]
        public void GetRoles_ReturnsNotFound()
        {
            // Arrange
            var email = "rooooooooooot@admin.com";
            var controller = new UserApiController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            // Act
            IHttpActionResult actionResult = controller.GetRoles(new EmailNameViewModel()
            {
                Email = email,
            });

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
