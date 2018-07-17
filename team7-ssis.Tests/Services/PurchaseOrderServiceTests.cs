using System;
using team7_ssis.Models;
using team7_ssis.Services;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class PurchaseOrderServiceTests
    {
        ApplicationDbContext context;
        PurchaseOrderService purchaseOrderService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
        }

        [TestMethod]
        public void FindAllPurchaseOrdersTest()
        {
            //Arrange
            int expected = context.PurchaseOrder.Count();
            //Act
            var result = purchaseOrderService.FindAllPurchaseOrders().Count();
            //Assert
            Assert.AreEqual(expected, result);
        }
        

        [TestMethod]
        public void FindPurchaseOrderByIdTest()
        {
            //Arrange
            PurchaseOrder p= new PurchaseOrder();
            p.PurchaseOrderNo = "TEST";

            //Act
            var result = purchaseOrderService.FindPurchaseOrderById("TEST");

            //Assert
            Assert.AreEqual(p.PurchaseOrderNo, result.PurchaseOrderNo);
        }

        [TestMethod]
        public void FindPurchaseOrderDetailsByIdTest()
        {
            //Arrange
            var expected = context.PurchaseOrderDetail.Where(x => x.PurchaseOrderNo == "TEST").Count();

            //Act
            var result = purchaseOrderService.FindPurchaseOrderDetailsById("TEST").Count();

            //Assert
            Assert.AreEqual(expected, result);
        }
        

        [TestMethod]
        public void FindPurchaseOrderBySupplierTest()
        {
            //Arrange
            var expected = context.PurchaseOrder.Where(x => x.SupplierCode == "OMEG").Count();

            //Act
            var result = purchaseOrderService.FindPurchaseOrderBySupplier("OMEG").Count();

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindPurchaseOrderBySupplierObjectTest()
        {
            //Arrange
            //var expected = context.PurchaseOrder.Where(x => x.SupplierCode == "OMEG").Count();

            //Act
            Supplier s = new Supplier();
            s.SupplierCode = "OMEG";

            var result = purchaseOrderService.FindPurchaseOrderBySupplier(s);
            
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));
            
            result.ForEach(x => Assert.AreEqual("OMEG", x.SupplierCode));
        }

    }
}
