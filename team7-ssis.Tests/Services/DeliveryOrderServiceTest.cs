﻿using System;
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
        DeliveryOrderRepository deliveryOrderRepository;
        PurchaseOrderRepository purchaseOrderRepository;
        DeliveryOrderDetail deliveryOrderDetail;
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
            dod1.Status = statusRepository.FindById(0);

            List<DeliveryOrderDetail> list= new List<DeliveryOrderDetail>();
            list.Add(dod1);
            d1.DeliveryOrderDetails = list;

            // Act
            var result = deliveryOrderService.Save(d1);

            //Assert
            Assert.AreEqual("DDDD", result.DeliveryOrderNo);
            Assert.IsInstanceOfType(result, typeof(DeliveryOrder));

            //clean
            deliveryOrderRepository.Delete(d1);
            po.Status = statusRepository.FindById(15);

        }

        [TestMethod]
        public void SaveInventoryTest()
        {
            //Arrange
            Item i= itemRepository.FindById("C002");

            //Act
            var result = deliveryOrderService.SaveInventory(i, 40);
            Inventory inv = inventoryRepository.FindById("C002");
            inv.Quantity = 0;


            //Arrange
            Assert.AreEqual("C002", result.ItemCode);
        }

        [TestMethod]
        public void SaveStockMovementTest()
        {
            //Arrange
            Item i = itemRepository.FindById("E030");
            PurchaseOrder po = purchaseOrderRepository.FindById("TEST");

            DeliveryOrder d1 = new DeliveryOrder();
            d1.DeliveryOrderNo = "DDDD";
            d1.PurchaseOrder = po;
            d1.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail dod1 = new DeliveryOrderDetail();
            dod1.DeliveryOrder = d1;
            dod1.Item = itemRepository.FindById("E030");
            dod1.PlanQuantity = 100;
            dod1.ActualQuantity = 50;

            List<DeliveryOrderDetail> list = new List<DeliveryOrderDetail>();
            list.Add(dod1);
            d1.DeliveryOrderDetails = list;
            new DeliveryOrderRepository(context).Save(d1);
            new DeliveryOrderDetailRepository(context).Save(dod1);
          

            //Act
            var result = deliveryOrderService.SaveStockMovement(dod1,i, 40);

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
