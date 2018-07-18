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
    public class ItemPriceServiceTest
    {
        ApplicationDbContext context;
        ItemPriceService itemPriceService;
        ItemPriceRepository itemPriceRepository;
        ItemRepository itemRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemPriceService = new ItemPriceService(context);
            itemPriceRepository = new ItemPriceRepository(context);
            itemRepository = new ItemRepository(context);
        }

        [TestMethod]
        public void FindAllItemPriceTest()
        {
            //Act
            var result = itemPriceService.FindAllItemPrice();
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemPrice));

        }

        [TestMethod]
        public void FindItemPriceByItemCodeTest()
        {
            //Arrange
            string test = "C001";

            //Act
            var result = itemPriceService.FindItemPriceByItemCode(test);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemPrice));
            foreach (ItemPrice element in result)
            {
                Assert.AreEqual("C001", element.ItemCode);
            }
        }

        [TestMethod]
        public void FindItemPriceBySupplierCodeTest()
        {
            //Arrange
            string test = "ALPA";

            //Act
            var result = itemPriceService.FindItemPriceBySupplierCode(test);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemPrice));
            foreach (ItemPrice element in result)
            {
                Assert.AreEqual("ALPA", element.SupplierCode);
            }
        }

        [TestMethod]
        public void FindItemPriceByPrioritySequenceTest()
        {
            //Arrange
            int test = 3;

            //Act
            var result = itemPriceService.FindItemPriceByPrioritySequence(3);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemPrice));
            foreach (ItemPrice element in result)
            {
                Assert.AreEqual(3, element.PrioritySequence);
            }
        }

        [TestMethod]
        public void SaveItemPriceTest()
        {
            //Arrange
            Item it = new Item();
            it.ItemCode = "BBB";
            it.CreatedDateTime = DateTime.Now;
            new ItemRepository(context).Save(it);

            ItemPrice i = new ItemPrice();
            i.ItemCode = it.ItemCode;
            i.SupplierCode = "ALPA";
            i.PrioritySequence = 3;
            i.Price = 20.30M;
            i.CreatedDateTime = DateTime.Now;

            //Act
            var result = itemPriceService.Save(i);

            //Arrange
            Assert.AreEqual("BBB", result.ItemCode);
            Assert.AreEqual("ALPA", result.SupplierCode);
            Assert.AreEqual(3, result.PrioritySequence);
            Assert.AreEqual(20.30M, result.Price);
            
            //clean
            itemPriceRepository.Delete(i);
            itemRepository.Delete(it);
        }

        [TestMethod]
        public void DeleteItemPriceTest()
        {
            //Arrage
            Item it = new Item();
            it.ItemCode = "BBB";
            it.CreatedDateTime = DateTime.Now;
            new ItemRepository(context).Save(it);

            ItemPrice p = new ItemPrice();
            p.ItemCode = it.ItemCode;

            //Act
            var result = itemPriceService.DeleteItemPrice(p);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemPrice));
            foreach (ItemPrice element in result)
            {
                Assert.AreEqual("Disabled", element.Status.Name);
            }
            itemRepository.Delete(it);
        }

    }
}
