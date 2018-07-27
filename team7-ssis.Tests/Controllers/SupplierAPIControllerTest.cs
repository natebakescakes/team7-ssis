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
    public class SupplierAPIControllerTest
    {
        private ApplicationDbContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetAllSuppliers_ContainsResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new SupplierAPIController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),           
            };

            //Act
            IHttpActionResult actionResult = controller.Suppliers();
            var contentResult = actionResult as OkNegotiatedContentResult<List<SupplierViewModel>>;

            //Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
          
        }

        [TestMethod]
        public void GetSupplier_ContainResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new SupplierAPIController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            Supplier supplier = context.Supplier.First();

            //Act
            SupplierViewModel result = controller.GetSupplier(supplier.SupplierCode);

            //Assert
            Assert.AreEqual(supplier.Name, result.Name);
        }

        [TestMethod]
        public void GetPriceList_ContainsResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new SupplierAPIController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            Supplier supplier = context.Supplier.First();

            //Act
            IHttpActionResult actionResult = controller.GetPriceList(supplier.SupplierCode);
            var contentResult = actionResult as OkNegotiatedContentResult<List<ItemPriceViewModel>>;
            
            //Assert
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(supplier.SupplierCode, contentResult.Content.First().SupplierCode);

        }

    }
}
