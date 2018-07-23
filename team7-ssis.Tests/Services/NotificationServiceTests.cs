using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class NotificationServiceTests
    {
        ApplicationDbContext context;
        NotificationService notificationService;
        NotificationRepository notificationRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            notificationRepository = new NotificationRepository(context);
            notificationService = new NotificationService(context);
        }
        [TestMethod]
        public void CreateDisbursementNotificationTest()
        {
            //Arrange
            Disbursement disbursement = context.Disbursement.First();
            Notification notification = context.Notification.First();
            int expected = 1;

            //Act
            var result = notificationService.CreateNotification(disbursement, notification.CreatedFor);


            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void CreateRequisitionNotificationTest()
        {
            //Arrange
            Requisition requisition = context.Requisition.First();
            Notification notification = context.Notification.First();
            int expected = 2;
            //Act
            var result = notificationService.CreateNotification(requisition, notification.CreatedFor);


            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void CreateStockAdjustmentNotificationTest()
        {
            //Arrange
            StockAdjustment stockAdjustment = context.StockAdjustment.First();
            Notification notification = context.Notification.First();
            int expected = 3;

            //Act
            var result = notificationService.CreateNotification(stockAdjustment, notification.CreatedFor);

            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void FindNotificationsByUserTest()
        {
            //Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });
            Notification notification = context.Notification.First();
            ApplicationUser user = notification.CreatedFor;

            int expected = context.Notification.Where(x => x.CreatedFor.Id == user.Id).Count();

            //Act
            var result = notificationService.FindNotificationsByUser(user).Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindNotificationsByTypeTest()
        {
            //Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            Notification notification = context.Notification.First();
            NotificationType type = notification.NotificationType;

            int expected = context.Notification
                .Where(x => x.NotificationType.NotificationTypeId == notification.NotificationType.NotificationTypeId)
                .Count();

            //Act
            var result = notificationService.FindNotificationByType(type).Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindUserByDepartment_NullInput()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationsByUser(null);

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void FindNotificationById_Exists()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationById(notificationId);

            // Assert
            Assert.AreEqual(notificationId, result.NotificationId);
        }

        [TestMethod]
        public void FindNotificationById_DoesNotExist_ReturnNull()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationById(0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ReadNotification_Valid()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            var expected = 15; // Read status
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                Status = new StatusService(context).FindStatusByStatusId(14),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.ReadNotification(notificationId);

            // Assert
            Assert.AreEqual(expected, result.Status.StatusId);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadNotification_AlreadyRead()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                Status = new StatusService(context).FindStatusByStatusId(15),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.ReadNotification(notificationId);
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
