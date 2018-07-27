using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class ItemCategoryControllerTest
    {
        ApplicationDbContext context;
        ItemRepository itemRepository;
        ItemCategoryService itemcategoryService;
        ItemCategoryRepository itemcategoryRepository;
       

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemRepository = new ItemRepository(context);
            itemcategoryService = new ItemCategoryService(context);
            itemcategoryRepository = new ItemCategoryRepository(context);

            //create new ItemCategory object and save into DB
            ItemCategory ic = itemcategoryRepository.Save(new ItemCategory() {
                ItemCategoryId = IdService.GetNewItemCategoryId(context),
                Name = "TEST",
                CreatedDateTime = DateTime.Now
            });
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            ItemCategoryController controller = new ItemCategoryController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddNewItemCategoryTest()
        {
            //Arrange
            //Instantiate controller
            ItemCategoryController controller = new ItemCategoryController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();

            //create new ViewModel to Save via controller
            ItemCategoryViewModel newItemCategory = new ItemCategoryViewModel()
            {
                ItemCategoryId = IdService.GetNewItemCategoryId(context),
                Name = "TEST"
                
            };

            //Act
            ActionResult result = controller.Save(newItemCategory);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditItemCategoryTest()
        {
            //Arrange
            string expected = "Testing EditSupplier";

            //Instantiate controller
            ItemCategoryController controller = new ItemCategoryController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();

            //Assemble a ItemCategory ViewModel from existing test object
            ItemCategory ic = context.ItemCategory.Where(x => x.Name == "TEST").First();
            ItemCategoryViewModel VM = new ItemCategoryViewModel()
            {
                ItemCategoryId = ic.ItemCategoryId,
                Name = ic.Name,
                Description = expected
                
            };

            //Act
            //pass ViewModel to controller
            controller.Save(VM);

            var result = itemcategoryRepository.FindById(VM.ItemCategoryId);

            //Assert
            //check that entry has been updated in db
            Assert.AreEqual(expected, result.Description);

        }

        [TestCleanup]
        public void TestCleanup()
        {
            List<ItemCategory> list = context.ItemCategory.Where(x => x.Name == "TEST").ToList();

            foreach(ItemCategory i in list)
            {
                itemcategoryRepository.Delete(i);
            }
        }
    }
}
