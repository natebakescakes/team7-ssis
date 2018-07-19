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
using System.Web;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DeliveryOrderServiceTest
    {
        ApplicationDbContext context;
        DeliveryOrderService deliveryOrderService;
        ItemService itemService;
        DeliveryOrderRepository deliveryOrderRepository;
        PurchaseOrderRepository purchaseOrderRepository;
        //  DeliveryOrderDetail deliveryOrderDetail;
        InventoryRepository inventoryRepository;
        ItemRepository itemRepository;
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;
        StockMovementRepository stockMovementRepository;
        StatusRepository statusRepository;

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
        public void FindDeliveryOrderByPurchaseOrderNoValidTest()
        {
            //Arrange
            string expected = "TEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo(expected);
            //Assert
            Assert.AreEqual(expected, result.PurchaseOrder.PurchaseOrderNo);
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
            Assert.AreEqual(expected, result.PurchaseOrder.PurchaseOrderNo);
        }

        [TestMethod]

        public void FindDeliveryOrderBySupplierValidTest()
        {
            //Arrange
            string expected = "CHEP";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderBySupplier(expected);
            //Assert
            Assert.AreEqual(expected, result.Supplier.SupplierCode);
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
            Assert.AreEqual(expected, result.Supplier.SupplierCode);
        }

        [TestMethod]
        public void SaveTest()
        {
            // Arrange

           
            PurchaseOrder po = purchaseOrderRepository.FindById("TEST");
            DeliveryOrder d1 = new DeliveryOrder();
            d1.DeliveryOrderNo = "DDDD";
            d1.PurchaseOrder = po;
            d1.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail dod1 = new DeliveryOrderDetail();
            dod1.DeliveryOrderNo = "DDDD";
            dod1.ItemCode = itemRepository.FindById("C003").ItemCode;
            dod1.PlanQuantity = 100;
            dod1.ActualQuantity = 50;


            List<DeliveryOrderDetail> list= new List<DeliveryOrderDetail>();
            list.Add(dod1);
            d1.DeliveryOrderDetails = list;


            // Act
            var result = deliveryOrderService.Save(d1);
            deliveryOrderRepository.Delete(d1);
            Assert.IsNotNull(context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "DDDD").First());
            po.Status = statusRepository.FindById(15);

            //Assert
            Assert.AreEqual("DDDD", result.DeliveryOrderNo);
           

            // Assert.AreEqual("CHEP",result.Supplier.SupplierCode);

            //clean
           
            // purchaseOrderRepository.Delete(po);
        }

        [TestMethod]
        public void SaveInventoryTest()
        {
            //Arrange
            Item i= itemRepository.FindById("C002");

            //Act
            var result = deliveryOrderService.SaveInventory(i, 40);

            //Arrange
            Assert.AreEqual("C002", result.ItemCode);
        }

        [TestMethod]
        public void SaveStockMovementTest()
        {
            //Arrange
            Item i = itemRepository.FindById("C002");

            //Act
            var result = deliveryOrderService.SaveStockMovement(i, 40);
            //Clean
            stockMovementRepository.Delete(result);

            //Arrange
            Assert.AreEqual("C002", result.Item.ItemCode);

           
           // itemRepository.Delete(i);
        }

        [TestMethod]

        public void SaveDOFileToDeliveryOrderTest()
        {
            // Arrange
            string filename = @"C:\Valli\MyFirstProgram.txt";

            //Act
            String result = deliveryOrderService.SaveDOFileToDeliveryOrder(filename);

            // define string expectedPath
            //Path.GetFullPath(HttpContext.Current.Server.MapPath("/DOFiles"));
            //Path.GetFullPath(HttpContext.Current.Server.MapPath(filelocation));

            //Assert
            //Assert.AreEqual(fileName, result);
            bool fileExists = File.Exists(result);
            Assert.IsTrue(fileExists);
        }
    }
}
