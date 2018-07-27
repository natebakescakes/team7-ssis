using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class ItemCategoryAPIControllerTest
    {
        private ApplicationDbContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetAllItemCategories_ContainsResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new ItemCategoryAPIController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            //Act
            IHttpActionResult actionResult = controller.ItemCategories();
            var contentResult = actionResult as OkNegotiatedContentResult<List<ItemCategoryViewModel>>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
        }

        [TestMethod]
        public void GetItemCategory_ContainResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new ItemCategoryAPIController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            ItemCategory ic = context.ItemCategory.First();

            //Act
            ItemCategoryViewModel result = controller.GetItemCategory(ic.ItemCategoryId.ToString());

            //Assert
            Assert.AreEqual(ic.Name, result.Name);
        }
    }
}
