using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class InventoryRepositoryTests
    {
        ApplicationDbContext context;
        InventoryRepository inventoryRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            inventoryRepository = new InventoryRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = inventoryRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = inventoryRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = inventoryRepository.FindById("E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Inventory));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = inventoryRepository.ExistsById("E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeQuantity()
        {
            // Arrange
            var inventory = inventoryRepository.FindById("S020");
            var original = inventory.Quantity;
            inventory.Quantity = 999999;

            // Act
            var result = inventoryRepository.Save(inventory);

            // Assert
            Assert.AreEqual(999999, result.Quantity);

            // Tear Down
            inventory.Quantity = original;
            inventoryRepository.Save(inventory);
        }

        // Mini-Integration Test with Item
        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var itemRepository = new ItemRepository(context);
            var item = new Item
            {
                ItemCode = "XXXX",
                CreatedDateTime = DateTime.Now
            };

            var inventory = new Inventory
            {
                ItemCode = "XXXX"
            };

            // Act
            itemRepository.Save(item);
            var saveResult = inventoryRepository.Save(inventory);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Inventory));

            // Delete saved object from DB
            // Act
            inventoryRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(inventoryRepository.FindById("XXXX"));

            // Tear Down
            itemRepository.Delete(item);
        }
    }
}