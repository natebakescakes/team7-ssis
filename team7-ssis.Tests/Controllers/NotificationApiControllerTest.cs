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
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class NotificationApiControllerTest
    {
        private ApplicationDbContext context;
        
        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
        }

        [TestMethod]
        [Ignore]
        public void GetNotifications_ContainsResult()
        {
            // Arrange
            var expected = "TEST";
            var controller = new NotificationApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                CurrentUserName = "root@admin.com"
            };

            // Act
            IHttpActionResult actionResult = controller.GetCurrentUser();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<NotificationViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(x => x.Contents).Contains(expected));
        }

        [TestMethod]
        public void GetNotifications_NotFound()
        {
            // Arrange
            var controller = new NotificationApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                CurrentUserName = "123@abc.com"
            };

            // Act
            IHttpActionResult actionResult = controller.GetCurrentUser();

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }
    }
}
