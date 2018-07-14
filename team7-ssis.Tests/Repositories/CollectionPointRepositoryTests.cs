using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class CollectionPointRepositoryTests
    {
        ApplicationDbContext context;
        CollectionPointRepository collectionPointRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            collectionPointRepository = new CollectionPointRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = collectionPointRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = collectionPointRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = collectionPointRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CollectionPoint));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = collectionPointRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeUpdatedBy()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var collectionPoint = collectionPointRepository.FindById(2);
            var original = collectionPoint.ClerkInCharge;
            collectionPoint.ClerkInCharge = user;

            // Act
            var result = collectionPointRepository.Save(collectionPoint);

            // Assert
            Assert.AreEqual(user, result.ClerkInCharge);

            // Tear Down
            collectionPoint.ClerkInCharge = original;
            collectionPointRepository.Save(collectionPoint);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var collectionPoint = new CollectionPoint
            {
                CollectionPointId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = collectionPointRepository.Save(collectionPoint);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(CollectionPoint));

            // Delete saved object from DB
            // Act
            collectionPointRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(collectionPointRepository.FindById(999999));
        }
    }
}
