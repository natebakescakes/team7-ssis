using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class ItemCategoryRepositoryTests
    {
        ApplicationDbContext context;
        ItemCategoryRepository itemCategoryRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            itemCategoryRepository = new ItemCategoryRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = itemCategoryRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = itemCategoryRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = itemCategoryRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ItemCategory));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = itemCategoryRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeUpdatedBy()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var itemCategory = itemCategoryRepository.FindById(2);
            var original = itemCategory.UpdatedBy;
            itemCategory.UpdatedBy = user;

            // Act
            var result = itemCategoryRepository.Save(itemCategory);

            // Assert
            Assert.AreEqual(user, result.UpdatedBy);

            // Tear Down
            itemCategory.UpdatedBy = original;
            itemCategoryRepository.Save(itemCategory);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var itemCategory = new ItemCategory
            {
                ItemCategoryId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = itemCategoryRepository.Save(itemCategory);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(ItemCategory));

            // Delete saved object from DB
            // Act
            itemCategoryRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(itemCategoryRepository.FindById(999999));
        }
    }
}