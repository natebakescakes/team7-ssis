using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DeliveryOrderServiceTest
    {
        ApplicationDbContext context;
        DeliveryOrderService deliveryOrderService;
        DeliveryOrderRepository deliveryOrderRepository;
        PurchaseOrderRepository purchaseOrderRepository;
        StatusRepository statusRepository;
        InventoryRepository inventoryRepository;
        ItemRepository itemRepository;
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;
        StockMovementRepository stockMovementRepository;
       
        [TestInitialize]
        public void TestInitialize()
        {

            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            deliveryOrderRepository = new DeliveryOrderRepository(context);
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            inventoryRepository = new InventoryRepository(context);
            itemRepository = new ItemRepository(context);
            deliveryOrderDetailRepository = new DeliveryOrderDetailRepository(context);
            stockMovementRepository = new StockMovementRepository(context);
            statusRepository = new StatusRepository(context);
        }

        [TestMethod]
        public void FindAllDeliveryOrdersTest()
        {
            //Arrange
            int expected = context.DeliveryOrder.Count();
            //Act
            var result = deliveryOrderService.FindAllDeliveryOrders().Count();
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        [Ignore]
        public void FindDeliveryOrderByIdValidTest()
        {
            //Arrange
            string expected = "TEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderById(expected);
            //Assert
            Assert.AreEqual(expected, result.DeliveryOrderNo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindDeliveryOrderByIdExceptionalTest()
        {
            //Arrange
            string expected = "BEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderById(expected);
            //Assert
            Assert.AreEqual(expected, result.DeliveryOrderNo);
        }

        [TestMethod]
        [Ignore]
        public void FindDeliveryOrderByPurchaseOrderNoValidTest()
        {
            //Arrange
            string expected = "TEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo(expected);
            //Assert
           // Assert.AreEqual(expected, result.PurchaseOrder.PurchaseOrderNo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindDeliveryOrderByPurchaseOrderNoExceptionalTest()
        {
            //Arrange
            string expected = "BEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo(expected);
            //Assert
          //  Assert.AreEqual(expected, result.PurchaseOrder.PurchaseOrderNo);
        }

        [TestMethod]
        [Ignore]

        public void FindDeliveryOrderBySupplierValidTest()
        {
            //Arrange
            string expected = "CHEP";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderBySupplier(expected);
            //Assert
          //  Assert.AreEqual(expected, result.Supplier.SupplierCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindDeliveryOrderBySupplierExceptionalTest()
        {
            //Arrange
            string expected = "CHEAP";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderBySupplier(expected);
            //Assert
         //   Assert.AreEqual(expected, result.Supplier.SupplierCode);
        }

        [TestMethod]
        [Ignore]
        public void SaveTest()
        {
            // Arrange
           
            PurchaseOrder po = purchaseOrderRepository.FindById("TEST");
            DeliveryOrder d1 = new DeliveryOrder
            {
                DeliveryOrderNo = "DDDD",
                PurchaseOrder = po,
                CreatedDateTime = DateTime.Now
            };

            DeliveryOrderDetail dod1 = new DeliveryOrderDetail
            {
                DeliveryOrderNo = "DDDD",
                ItemCode = itemRepository.FindById("E030").ItemCode,
                PlanQuantity = 100,
                ActualQuantity = 50,
                Status = statusRepository.FindById(0)
            };

            Item i = itemRepository.FindById("E030");

            List<DeliveryOrderDetail> list = new List<DeliveryOrderDetail>
            {
                dod1
            };
            d1.DeliveryOrderDetails = list;

            // Act
            var result = deliveryOrderService.Save(d1);
            var result1 = stockMovementRepository.FindById(2);

            //Assert
            Assert.AreEqual("DDDD", result.DeliveryOrderNo);
            Assert.IsInstanceOfType(result, typeof(DeliveryOrder));

            //clean
           deliveryOrderRepository.Delete(d1);
            po.Status = statusRepository.FindById(15);
            purchaseOrderRepository.Save(po);
            stockMovementRepository.Delete(result1);
        }

        [TestMethod]
        public void SaveInventoryTest()
        {
            //Arrange
            Item i= itemRepository.FindById("E030");

            //Act
            var result = deliveryOrderService.SaveInventory(i, 50);
            Inventory inv = inventoryRepository.FindById("E030");
            inv.Quantity = 0;
            inventoryRepository.Save(inv);

            //Arrange
            Assert.AreEqual("E030", result.ItemCode);
        }

        [TestMethod]
        [Ignore]
        public void SaveStockMovementTest()
        {
            //Arrange
            Item i = itemRepository.FindById("E030");

            PurchaseOrder po = purchaseOrderRepository.FindById("TEST");

            DeliveryOrder d1 = new DeliveryOrder
            {
                DeliveryOrderNo = "DDDD",
                PurchaseOrder = po,
                CreatedDateTime = DateTime.Now
            };

            DeliveryOrderDetail dod1 = new DeliveryOrderDetail
            {
                DeliveryOrder = d1,
                Item = i,
                PlanQuantity = 100,
                ActualQuantity = 50
            };

            List<DeliveryOrderDetail> list = new List<DeliveryOrderDetail>
            {
                dod1
            };
            d1.DeliveryOrderDetails = list;
            new DeliveryOrderRepository(context).Save(d1);
            new DeliveryOrderDetailRepository(context).Save(dod1);
          

            //Act
            var result = deliveryOrderService.SaveStockMovement(dod1,i, 50);

            //Arrange
            Assert.AreEqual("E030",result.Item.ItemCode);

            //Clean
            
             stockMovementRepository.Delete(result);
             deliveryOrderDetailRepository.Delete(dod1);
             deliveryOrderRepository.Delete(d1);
        }

        //public void TestClean()
        //{

        //}

     //   [TestMethod]

    //    public void SaveDOFileToDeliveryOrderTest()
    //    {
    //        // Arrange
    //        string filename = @"C:\Valli\MyFirstProgram.txt";

    //        //Act
    //        String result = deliveryOrderService.SaveDOFileToDeliveryOrder(filename);

    //        // define string expectedPath
    //        //Path.GetFullPath(HttpContext.Current.Server.MapPath("/DOFiles"));
    //        //Path.GetFullPath(HttpContext.Current.Server.MapPath(filelocation));

    //        //Assert
    //        //Assert.AreEqual(fileName, result);
    //        bool fileExists = File.Exists(result);
    //        Assert.IsTrue(fileExists);
    //    }
    }
}
