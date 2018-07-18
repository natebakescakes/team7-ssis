using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class ItemServiceTest
    {
        ApplicationDbContext context;
        ItemService itemService;
        ItemRepository itemRepository;
        InventoryRepository inventoryRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            itemRepository = new ItemRepository(context);
        }


        [TestMethod]
        public void FindAllItemTest()
        {
            //Act
            var result = itemService.FindAllItems();
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(Item));
        }

        [TestMethod]
        public void FindItemByItemCodeTest()
        {
            //Arrange
            string test = "C001";

            //Act
            var result = itemService.FindItemByItemCode(test);

            //Assert
            Assert.AreEqual("C001", result.ItemCode);

        }

        [TestMethod]
        public void FindItemsByCategory()
        {
            //Arrange
            ItemCategory i = new ItemCategory();
            i.ItemCategoryId = 2;

            //Act
            var result = itemService.FindItemsByCategory(i);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(Item));
        }

        [TestMethod]
        public void SaveItemTest()
        {
            //Arrage
            Item i = new Item();
            i.ItemCode = "BBB";
            i.CreatedDateTime = DateTime.Now;

            //Act
            var result = itemService.Save(i,20);

            //Assert
            Assert.AreEqual("BBB", result.ItemCode);
            Assert.AreEqual(20, result.Inventory.Quantity);
            itemRepository.Delete(i);
            
        }

        [TestMethod]
        public void SaveInventoryTest()
        {
            //Arrange
            Item i = new Item();
            i.ItemCode = "BBB";
            i.CreatedDateTime = DateTime.Now;
            new ItemRepository(context).Save(i);

            //Act
            var result = itemService.SaveInventory(i,40);

            //Arrange
            Assert.AreEqual("BBB", result.ItemCode);
            //Assert.AreEqual(40, result.Quantity);
            itemRepository.Delete(i);
        }


        [TestMethod]
        public void DeleteItemTest()
        {
            //Arrage
            Item i = new Item();
            i.ItemCode = "BBB";
            i.CreatedDateTime = DateTime.Now;
            itemService.Save(i, 20);

            //Act
            var result = itemService.DeleteItem(i);

            //Assert
            Assert.AreEqual("Disabled", result.Status.Name);
            itemRepository.Delete(i);
        }

        [TestMethod]
        public void UpdateQuantityTest()
        {
            //Arrange
            Item i = new Item();
            i.ItemCode = "BBB";
            i.CreatedDateTime = DateTime.Now;
            itemService.Save(i, 20);

            //Act
            var result = itemService.UpdateQuantity(i, 30);

            //Assert
            Assert.AreEqual(30,result.Quantity);
            itemRepository.Delete(i);
        }

    }
}
