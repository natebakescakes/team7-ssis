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
    public class SupplierControllerTest
    {
        ApplicationDbContext context;
        SupplierService supplierService;
        SupplierRepository supplierRepository;
        ItemRepository itemRepository;
        ItemPriceRepository ipRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            supplierRepository = new SupplierRepository(context);
            supplierService = new SupplierService(context);
            itemRepository = new ItemRepository(context);
            ipRepository = new ItemPriceRepository(context);

            //saving a test supplier object in db
           Supplier supplier =  supplierRepository.Save(new Supplier() {
                SupplierCode = "TEST",
                Name="TEST",
                CreatedDateTime =DateTime.Now

            });

            //saving a test item object in db
            
            Item item = itemRepository.Save(new Item() {
                ItemCode = "TEST",
                CreatedDateTime = DateTime.Now,
                
            });

            //saving a test item price object in db
            ipRepository.Save(new ItemPrice()
            {
                Item = item,
                CreatedDateTime = DateTime.Now,
                Price=1,
                Supplier = supplier
                
            });

            
        }

        [TestMethod]
        public void Index()
        {
            // Arrange
            SupplierController controller = new SupplierController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddNewSupplierTest()
        {
            //Arrange
            //Instantiate controller
            SupplierController controller = new SupplierController() {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();
           

            //create new ViewModel to Save via controller
            SupplierViewModel newSupplier = new SupplierViewModel() {
                SupplierCode ="DEMO",
                Name = "TEST"
                
            };

            //Act
            ActionResult result = controller.Save(newSupplier);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditSupplierTest()
        {
            //Arrange
            string expected = "Testing EditSupplier";

            //Instantiate controller
            SupplierController controller = new SupplierController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();
           
            //Assemble a Supplier ViewModel from existing test object
            Supplier supplier = supplierService.FindSupplierById("TEST");
            SupplierViewModel VM = new SupplierViewModel()
            {
                SupplierCode = supplier.SupplierCode,
                Name=supplier.Name,

                //Edit Address of ViewModel
                Address = expected
            };


            //Act
            //pass ViewModel to controller
            controller.Save(VM);

            var result = supplierService.FindSupplierById("TEST");

            //Assert
            //check that entry has been updated in db
            Assert.AreEqual(expected, result.Address);
        }

        [TestMethod]
        public void ShowSupplierPriceListTest()
        {
            //Arrange
            Supplier supplier = context.Supplier.First();
            string id = supplier.SupplierCode;

            //Instantiate controller
            SupplierController controller = new SupplierController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();


            //Act
            var result = controller.SupplierPriceList(id) as ViewResult;
            var model = (SupplierViewModel)result.ViewData.Model;

            //Assert
            Assert.AreEqual(supplier.Name, model.Name);
        }

        [TestMethod]
        public void UpdatePriceListTest()
        {
            //Arrange
            decimal expected = 7.77M;

            //Instantiate controller
            SupplierController controller = new SupplierController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();


            //get an itemprice object from db
            ItemPrice itemPrice = context.ItemPrice.Where(x => x.ItemCode == "TEST").First();

            //assemble itemprice viewmodel
            ItemPriceViewModel VM = new ItemPriceViewModel()
            {
                ItemCode = itemPrice.ItemCode,
                SupplierCode = itemPrice.SupplierCode,

                //change price of itemprice viewmodel
                Price = expected

             };


            //Act
            controller.UpdateItemPrice(VM);
            var result = context.ItemPrice.Where(x => x.ItemCode == "TEST").First();

            //Assert
            Assert.AreEqual(expected, result.Price);

        }

        [TestCleanup]
        public void CleanUp()
        {
            List<Item> itemList = context.Item.Where(x => x.ItemCode == "TEST").ToList();
            foreach(Item i in itemList)
            {
                itemRepository.Delete(i);
            }

            List<Supplier> slist = context.Supplier.Where(x => x.Name == "TEST").ToList();
            foreach(Supplier s in slist)
            {
                supplierRepository.Delete(s);
            }


        }
    }
}
