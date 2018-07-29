using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class NotificationControllerTest
    {
        private ApplicationDbContext context;
        private NotificationService notificationService;
        private NotificationRepository notificationRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            notificationService = new NotificationService(context);
            notificationRepository = new NotificationRepository(context);
        }

        [TestMethod]
        public void ReadNotification_Unread_ReadThenRedirect()
        {
            // Arrange
            var controller = new NotificationController()
            {
                Context = context
            };
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                NotificationType = new NotificationTypeRepository(context).FindById(1),
                Status = new StatusService(context).FindStatusByStatusId(14),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = controller.Read(notificationId);

            // Assert
            Assert.AreEqual(15, notificationService.FindNotificationById(notificationId).Status.StatusId);
            result.AssertActionRedirect().ToAction("Index");
        }

        [TestMethod]
        public void ReadNotification_Read_Redirect()
        {
            // Arrange
            var controller = new NotificationController()
            {
                Context = context
            };
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                NotificationType = new NotificationTypeRepository(context).FindById(1),
                Status = new StatusService(context).FindStatusByStatusId(15),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = controller.Read(notificationId);

            // Assert
            Assert.AreEqual(15, notificationService.FindNotificationById(notificationId).Status.StatusId);
            result.AssertActionRedirect().ToAction("Index");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var usedNotificationId = IdService.GetNewNotificationId(context) - 1;
            if (notificationRepository.ExistsById(usedNotificationId))
                notificationRepository.Delete(notificationRepository.FindById(usedNotificationId));
        }
    }
}
