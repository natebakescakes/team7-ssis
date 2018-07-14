using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class StatusRepositoryTests
    {
        ApplicationDbContext context;
        StatusRepository statusRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            statusRepository = new StatusRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = statusRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = statusRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = statusRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Status));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = statusRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeDescription()
        {
            // Arrange
            var status = statusRepository.FindById(1);
            var original = status.Description;
            status.Description = "UNIT TEST";

            // Act
            var result = statusRepository.Save(status);

            // Assert
            Assert.AreEqual("UNIT TEST", result.Description);

            // Tear Down
            status.Description = original;
            statusRepository.Save(status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var status = new Status
            {
                StatusId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = statusRepository.Save(status);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Status));

            // Delete saved object from DB
            // Act
            statusRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(statusRepository.FindById(999999));
        }
    }
}