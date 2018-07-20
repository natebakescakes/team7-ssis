using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class NotificationServiceTest
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
            notificationRepository.Delete(result); //Delete test dummy object

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

            notificationRepository.Delete(result);  //Delete test dummy object
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
            notificationRepository.Delete(result); //Delete test dummy object


        }

        [TestMethod]
        public void FindNotificationsByUserTest()
        {
            //Arrange
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
            Notification notification = context.Notification.First();
            NotificationType type = notification.NotificationType;

            int expected = context.Notification
                .Where(x => x.NotificationType.NotificationTypeId==notification.NotificationType.NotificationTypeId)
                .Count();

            //Act
            var result = notificationService.FindNotificationByType(type).Count;

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}
