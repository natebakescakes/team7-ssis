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
            //Arrage
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
            var result = itemService.Save(i);

            //Assert
            Assert.AreEqual("BBB", result.ItemCode);
            itemRepository.Delete(i);
        }

        [TestMethod]
        public void DeleteItemTest()
        {
            //Arrage
            Item i = new Item();
            i.ItemCode = "AAA";

            //Act
            var result = itemService.DeleteItem(i);

            //Assert
            Assert.AreEqual("Disabled", result.Status.Name);
        }

    }
}
