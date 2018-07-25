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
    public class ItemCategoryServiceTest
    {
        ApplicationDbContext context;
        ItemCategoryService itemCategoryService;
        ItemCategoryRepository itemCategoryRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemCategoryService = new ItemCategoryService(context);
            itemCategoryRepository = new ItemCategoryRepository(context);
        }


        [TestMethod]
        public void FindAllItemCategoryTest()
        {
            //Act
            var result = itemCategoryService.FindAllItemCategory();
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ItemCategory));
        }

        [TestMethod]
        public void FindItemCategoryByItemCategoryId()
        {
            //Arrange
            int test = 1;

            //Act
            var result = itemCategoryService.FindItemCategoryByItemCategoryId(test);

            //Assert
            Assert.AreEqual("Clip", result.Name);
        }

        [TestMethod]
        public void Save()
        {
            //Arrage
            ItemCategory ic = new ItemCategory();
            ic.Name = "Test";
            ic.Description = "Tests";
            ic.Status = new StatusRepository(context).FindById(1);
            ic.CreatedDateTime = DateTime.Now;

            //Act
            var result = itemCategoryService.Save(ic);

            //Assert
            Assert.AreEqual("Test", result.Name);
            itemCategoryRepository.Delete(ic);
        }

        [TestMethod]
        public void Delete()
        {
            //Arrage
            ItemCategory ic = new ItemCategory();
            ic.Name = "Test";
            ic.Description = "Tests";
            ic.Status = new StatusRepository(context).FindById(1);
            ic.CreatedDateTime = DateTime.Now;
            itemCategoryService.Save(ic);

            //Act
            var result = itemCategoryService.Delete(ic);

            //Assert
            Assert.AreEqual("Disabled", result.Status.Name);
            itemCategoryRepository.Delete(ic);
        }
    }
}
