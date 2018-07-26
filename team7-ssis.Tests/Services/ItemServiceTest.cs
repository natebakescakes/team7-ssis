using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.Controllers;
using team7_ssis.Repositories;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Moq;

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
        public void FindAllActiveItemTest()
        {
            //Act
            var result = itemService.FindAllActiveItems();

            //Assert
            foreach(Item i in result)
            {
                Assert.AreEqual(1, i.Status.StatusId);
            }
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
        public void FindItemsByCategoryTest()
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
        public void FindItemQuantityLessThanReorderLevel()
        {
            //Act
            var result = itemService.FindItemQuantityLessThanReorderLevel();

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(Item));
            foreach (Item element in result)
            {
                Assert.IsTrue(element.Inventory.Quantity < element.ReorderLevel);
            }

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
            i.ItemCode = "CCC";
            i.CreatedDateTime = DateTime.Now;
            new ItemRepository(context).Save(i);

            //Act
            var result = itemService.SaveInventory(i,40);

            //Arrange
            Assert.AreEqual("CCC", result.ItemCode);
            //Assert.AreEqual(40, result.Quantity);
            itemRepository.Delete(i);
        }


        [TestMethod]
        public void DeleteItemTest()
        {
            //Arrage
            Item i = new Item();
            i.ItemCode = "DDD";
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
            i.ItemCode = "EEE";
            i.CreatedDateTime = DateTime.Now;
            itemService.Save(i, 20);

            //Act
            var result = itemService.UpdateQuantity(i, 30);

            //Assert
            Assert.AreEqual(30,result.Quantity);
            itemRepository.Delete(i);
        }

        [TestMethod]
        public void AddQuantityTest()
        {
            //Arrange
            Item i = new Item();
            i.ItemCode = "GGG";
            i.CreatedDateTime = DateTime.Now;
            itemService.Save(i, 40);

            //Act
            var result = itemService.AddQuantity(i, -10);

            //Assert
            Assert.AreEqual(30, result.Quantity);
            itemRepository.Delete(i);
        }

        [TestMethod]
        public void UploadItemImageTest()
        {
            //var file = MockRepository.GenerateStub<HttpPostedFileBase>();

            //file.Expect(f => f.ContentLength).Return(1);
            //file.Expect(f => f.FileName).Return("myFileName");
            //controller.Index(file);
        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            string[] ids = new string[]
           { "BBB","CCC","DDD","EEE","GGG","FFF" };

            foreach (string id in ids)
            {
               Item i = itemRepository.FindById(id);
                if (i != null)
                    itemRepository.Delete(i);
            }
        }

    }
}
