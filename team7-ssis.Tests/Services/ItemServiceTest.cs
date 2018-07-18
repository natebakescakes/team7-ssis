using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class ItemServiceTest
    {
        ApplicationDbContext context;
        ItemService itemService;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
        }


        [TestMethod]
        public void FindAllItemTest()
        {
            //Arrange
            int expected = context.Item.Count();
            //Act
            var result = itemService.FindAllItems().Count();
            //Assert
            Assert.AreEqual(expected, result);
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
       
    }
}
