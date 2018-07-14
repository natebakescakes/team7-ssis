using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class NotificationRepostioryTests
    {
        ApplicationDbContext context;
        NotificationRepository notificationRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            notificationRepository = new NotificationRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = notificationRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = notificationRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = notificationRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Notification));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = notificationRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeNotificationType()
        {
            // Arrange
            var notificationType = new NotificationTypeRepository(context).FindById(1);
            var notification = notificationRepository.FindById(1);
            var original = notification.NotificationType;
            notification.NotificationType = notificationType;

            // Act
            var result = notificationRepository.Save(notification);

            // Assert
            Assert.AreEqual(notificationType, result.NotificationType);

            // Tear Down
            notification.NotificationType = original;
            notificationRepository.Save(notification);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var notification = new Notification
            {
                NotificationId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = notificationRepository.Save(notification);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Notification));

            // Delete saved object from DB
            // Act
            notificationRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(notificationRepository.FindById(999999));
        }
    }
}
