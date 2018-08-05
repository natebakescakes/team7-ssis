using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
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
        public void GetNotifications_ContainsResult()
        {
            // Arrange
            new NotificationRepository(context).Save(new Notification()
            {
                NotificationId = 999999,
                NotificationType = new NotificationTypeRepository(context).FindById(1),
                Contents = "TEST",
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedFor = new UserService(context).FindUserByEmail("root@admin.com"),
                CreatedDateTime = DateTime.Now,
            });
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

        [TestMethod]
        public void SendNotificationTest()
        {
            //Arrange
            new NotificationRepository(context).Save(new Notification()
            {
                NotificationId = 777777,
                NotificationType = new NotificationTypeRepository(context).FindById(1),
                Contents = "TEST",
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedFor = new UserService(context).FindUserByEmail("root@admin.com"),
                CreatedDateTime = DateTime.Now,
            });

            var controller = new NotificationApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                CurrentUserName = "root@admin.com"
            };

            //Act
            IHttpActionResult actionResult = controller.SendNotification("777777");

            //Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void SendEmailTest()
        {
            //Arrange
            new NotificationRepository(context).Save(new Notification()
            {
                NotificationId = 777777,
                NotificationType = new NotificationTypeRepository(context).FindById(1),
                Contents = "TEST",
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedFor = new UserService(context).FindUserByEmail("root@admin.com"),
                CreatedDateTime = DateTime.Now,
            });

            var controller = new NotificationApiController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                CurrentUserName = "root@admin.com"
            };


            //Act
           
            IHttpActionResult actionResult = controller.SendEmail("777777");

            //Assert
            Assert.IsNotNull(actionResult);
            
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var notificationRepository = new NotificationRepository(context);

            if (notificationRepository.ExistsById(999999))
                notificationRepository.Delete(notificationRepository.FindById(999999));

            if (notificationRepository.ExistsById(777777))
                notificationRepository.Delete(notificationRepository.FindById(777777));
        }
    }
}
