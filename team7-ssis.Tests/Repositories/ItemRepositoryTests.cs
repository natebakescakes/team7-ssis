using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class ItemRepositoryTests
    {
        ApplicationDbContext context;
        ItemRepository itemRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            itemRepository = new ItemRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = itemRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = itemRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = itemRepository.FindById("E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Item));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = itemRepository.ExistsById("E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeItemCategory()
        {
            // Arrange
            var itemCategory = new ItemCategoryRepository(context).FindById(1);
            var item = itemRepository.FindById("E030");
            var original = item.ItemCategory;
            item.ItemCategory = itemCategory;

            // Act
            var result = itemRepository.Save(item);

            // Assert
            Assert.AreEqual(itemCategory, result.ItemCategory);

            // Tear Down
            item.ItemCategory = original;
            itemRepository.Save(item);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var item = new Item
            {
                ItemCode = "XXXX",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = itemRepository.Save(item);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Item));

            // Delete saved object from DB
            // Act
            itemRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(itemRepository.FindById("XXXX"));
        }
    }
}