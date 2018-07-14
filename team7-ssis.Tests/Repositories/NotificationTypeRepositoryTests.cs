using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class NotificationTypeRepositoryTests
    {
        ApplicationDbContext context;
        NotificationTypeRepository notificationTypeRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            notificationTypeRepository = new NotificationTypeRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = notificationTypeRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = notificationTypeRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = notificationTypeRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotificationType));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = notificationTypeRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeUpdatedBy()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var notificationType = notificationTypeRepository.FindById(2);
            var original = notificationType.UpdatedBy;
            notificationType.UpdatedBy = user;

            // Act
            var result = notificationTypeRepository.Save(notificationType);

            // Assert
            Assert.AreEqual(user, result.UpdatedBy);

            // Tear Down
            notificationType.UpdatedBy = original;
            notificationTypeRepository.Save(notificationType);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var notificationType = new NotificationType
            {
                NotificationTypeId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = notificationTypeRepository.Save(notificationType);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(NotificationType));

            // Delete saved object from DB
            // Act
            notificationTypeRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(notificationTypeRepository.FindById(999999));
        }
    }
}
