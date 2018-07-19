using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        DeliveryOrderRepository deliveryorderRepository;

        [TestInitialize]
        public void TestInitialize()
        {

            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            deliveryorderRepository = new DeliveryOrderRepository(context);
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
            //Arrange
            DeliveryOrder deliveryorder = new DeliveryOrder();
            deliveryorder.DeliveryOrderNo = "D002";
            deliveryorder.CreatedDateTime = DateTime.Now;
            //Act
            //var result = deliveryOrderService.Save(deliveryorder);
            //Assert
           // Assert.AreEqual("D002", result.DeliveryOrderNo);
           // Assert.IsNotNull(context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "D002").First());
            //deliveryorderRepository.Delete(result);
        }
    }
}
