using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
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
            int result = notificationRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            notificationRepository.Save(new Notification()
            {
                NotificationId = 999999,
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = notificationRepository.FindById(999999);

            // Assert
            Assert.AreEqual(999999, result.NotificationId);
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            notificationRepository.Save(new Notification()
            {
                NotificationId = 999999,
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = notificationRepository.ExistsById(999999);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeNotificationType()
        {
            // Arrange
            var expected = new NotificationTypeRepository(context).FindById(1);
            notificationRepository.Save(new Notification()
            {
                NotificationId = 999999,
                NotificationType = new NotificationTypeRepository(context).FindById(2),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = notificationRepository.FindById(999999);
            result.NotificationType = expected;
            notificationRepository.Save(result);

            // Assert
            Assert.AreEqual(expected, result.NotificationType);
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

        [TestCleanup]
        public void TestCleanup()
        {
            if (notificationRepository.ExistsById(999999))
                notificationRepository.Delete(notificationRepository.FindById(999999));
        }
    }
}
