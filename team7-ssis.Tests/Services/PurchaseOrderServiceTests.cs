using System;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using team7_ssis.Repositories;
using team7_ssis.Services;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class PurchaseOrderServiceTests
    {
        ApplicationDbContext context;
        PurchaseOrderService purchaseOrderService;
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;
        StatusRepository statusRepository;
        ItemRepository itemRepository;
        SupplierRepository supplierRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemRepository = new ItemRepository(context);
            supplierRepository = new SupplierRepository(context);
        }

        [TestMethod]
        public void FindAllPurchaseOrdersTest()
        {
            //Arrange
            int expected = context.PurchaseOrder.Count();

            //Act
            var result = purchaseOrderService.FindAllPurchaseOrders();

            //Assert
            Assert.AreEqual(expected,result.Count());
            CollectionAssert.AllItemsAreUnique(result);
            CollectionAssert.AllItemsAreNotNull(result);
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));
            
        }
        

        [TestMethod]
        public void FindPurchaseOrderByIdTest()
        {
            //Act
            var result = purchaseOrderService.FindPurchaseOrderById("DUMMYYYYY");
            var result2 = purchaseOrderService.FindPurchaseOrderById("TEST");

            //Assert
            Assert.AreEqual("CHEP", result2.SupplierCode);
            Assert.IsNull(result);

        }

        
        

        [TestMethod]
        public void FindPurchaseOrderBySupplierTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "TEST2";
            p1.CreatedDateTime = DateTime.Now;
            p1.Supplier = supplierRepository.FindById("CHEP");
            purchaseOrderRepository.Save(p1);
        
            //Act
            var result = purchaseOrderService.FindPurchaseOrderBySupplier("ALPA");
            var result2 = purchaseOrderService.FindPurchaseOrderBySupplier("CHEP");

            //Assert
            Assert.AreEqual(result.Any(),false);
            CollectionAssert.AllItemsAreInstancesOfType(result2, typeof(PurchaseOrder));
            result2.ForEach(x => Assert.AreEqual("CHEP", x.SupplierCode));

            //teardown
            purchaseOrderRepository.Delete(p1);
        }

        [TestMethod]
        public void FindPurchaseOrderBySupplierObjectTest()
        {
            //Arrange
            Supplier s = new Supplier();
            s.SupplierCode = "OMEG";

            //Act
            var result = purchaseOrderService.FindPurchaseOrderBySupplier(s);
            
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));
            result.ForEach(x => Assert.AreEqual("OMEG", x.SupplierCode));
        }

        [TestMethod]
        public void DeleteItemFromPurchaseOrderTest()
        {
            //Arrange
            PurchaseOrder p = new PurchaseOrder();
            p.PurchaseOrderNo = "DUMMY";
            p.CreatedDateTime = DateTime.Now;
            p.Status = statusRepository.FindById(11);

            PurchaseOrderDetail p1 = new PurchaseOrderDetail();
            p1.PurchaseOrderNo = "DUMMY";
            p1.Item= itemRepository.FindById("E004");
            p1.Quantity = 50;
            p1.Status = statusRepository.FindById(11);

            PurchaseOrderDetail p2 = new PurchaseOrderDetail();
            p2.PurchaseOrderNo = "DUMMY";
            p2.Item = itemRepository.FindById("E007");
            p2.Quantity = 100;
            p2.Status= statusRepository.FindById(11);

            PurchaseOrderDetail p3 = new PurchaseOrderDetail();
            p3.PurchaseOrderNo = "DUMMY";
            p3.Item = itemRepository.FindById("E030");
            p3.Quantity = 100;
            p3.Status = statusRepository.FindById(11);

            purchaseOrderRepository.Save(p);
            purchaseOrderDetailRepository.Save(p1);
            purchaseOrderDetailRepository.Save(p2);
            purchaseOrderDetailRepository.Save(p3);

            string[] itemCodes = new string[] {"E030"};

            //Act
            purchaseOrderService.DeleteItemFromPurchaseOrder(p, itemCodes);

            //Assert
            var expected = context.PurchaseOrderDetail.Where(x => x.PurchaseOrderNo == "DUMMY" && x.ItemCode == "E030");
            Assert.AreEqual(expected.Any(),false);

            //tear down
            purchaseOrderRepository.Delete(p);
        }

        [TestMethod]
        public void FindPurchaseOrderByStatusTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.Status = statusRepository.FindById(11);
            p1.PurchaseOrderNo = "DUMMYS1";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrder p2 = new PurchaseOrder();
            p2.Status = statusRepository.FindById(12);
            p2.PurchaseOrderNo = "DUMMYS2";
            p2.CreatedDateTime = DateTime.Now;

            PurchaseOrder p3 = new PurchaseOrder();
            p3.Status = statusRepository.FindById(13);
            p3.PurchaseOrderNo = "DUMMYS3";
            p3.CreatedDateTime = DateTime.Now;

            purchaseOrderRepository.Save(p1);
            purchaseOrderRepository.Save(p2);
            purchaseOrderRepository.Save(p3);

            int[] statusId = new int[] {11,12};

            //Act
            var result = purchaseOrderService.FindPurchaseOrderByStatus(statusId);

            //Assert
            result.ForEach(x => Assert.IsTrue(x.Status.StatusId == 11 || x.Status.StatusId == 12));
            Assert.AreEqual(result.Count(), 2);

            //teardown
            purchaseOrderRepository.Delete(p1);
            purchaseOrderRepository.Delete(p2);
            purchaseOrderRepository.Delete(p3);
        }

        [TestMethod]
        public void SaveTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "DUMMYSA1";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrder p2 = new PurchaseOrder();
            p2.Status = statusRepository.FindById(15);
            p2.PurchaseOrderNo = "DUMMYSA2";
            p2.CreatedDateTime = DateTime.Now;

            purchaseOrderRepository.Save(p2);

            //Act
            var result1 = purchaseOrderService.Save(p1);
            p2.Supplier = supplierRepository.FindById("ALPA");
            var result2 = purchaseOrderService.Save(p2);

            //Assert
            Assert.AreEqual(result1.PurchaseOrderNo,"DUMMYSA1");
            Assert.AreEqual(result2.SupplierCode, "ALPA");

            //teardown
            purchaseOrderRepository.Delete(p1);
            purchaseOrderRepository.Delete(p2);

        }

        [TestMethod]
        public void CreatePOForEachSupplierTest()
        {
            //Arrange
            List<Item> items = new List<Item>();
            items.Add(itemRepository.FindById("C001"));
            items.Add(itemRepository.FindById("E005"));
            items.Add(itemRepository.FindById("E006"));
            items.Add(itemRepository.FindById("E007"));
            items.Add(itemRepository.FindById("E008"));
            

            //Act
            var result = purchaseOrderService.CreatePOForEachSupplier(items);

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));
            Assert.AreEqual("CHEP", result.First().Supplier.SupplierCode);
            Assert.AreEqual(result.Count(),3);
        }

        [TestMethod]
        public void AddItemsToPurchaseOrdersTest()
        {
            //Arrange
            List<OrderItem> orderItems = new List<OrderItem>();

            OrderItem o1 = new OrderItem();
            o1.Item = itemRepository.FindById("E005");
            o1.Quantity = 50;

            OrderItem o2 = new OrderItem();
            o2.Item = itemRepository.FindById("E007");
            o2.Quantity = 10;

            OrderItem o3 = new OrderItem();
            o3.Item = itemRepository.FindById("C001");
            o3.Quantity = 15;

            orderItems.Add(o1);
            orderItems.Add(o2);
            orderItems.Add(o3);

            List<PurchaseOrder> poList = new List<PurchaseOrder>();

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "P1";
            p1.Supplier = supplierRepository.FindById("CHEP");

            PurchaseOrder p2 = new PurchaseOrder();
            p2.PurchaseOrderNo = "P2";
            p2.Supplier = supplierRepository.FindById("BANE");

            poList.Add(p1);
            poList.Add(p2);

            //Act
            var result=purchaseOrderService.AddItemsToPurchaseOrders(orderItems, poList);

            //Assert
            Assert.AreEqual(result.First().PurchaseOrderDetails[0].Quantity, 50);
            Assert.AreEqual(result.First().PurchaseOrderDetails[1].Quantity, 15);
            Assert.AreEqual("C001", result.First().PurchaseOrderDetails[1].Item.ItemCode);
            Assert.AreEqual(result.Count(), 2);

        }

        [TestMethod]
        public void IsPurchaseOrderCreatedTest()
        {
            //Arrange
            List<PurchaseOrder> poList = new List<PurchaseOrder>();

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "P11";
            p1.Supplier = supplierRepository.FindById("CHEP");

            PurchaseOrder p2 = new PurchaseOrder();
            p2.PurchaseOrderNo = "P21";
            p2.Supplier = supplierRepository.FindById("BANE");

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrderNo = "P11";
            pd1.Item = itemRepository.FindById("E005");
            pd1.Quantity = 50;
            pd1.Status = statusRepository.FindById(11);

            PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
            pd2.PurchaseOrderNo = "P21";
            pd2.Item = itemRepository.FindById("E007");
            pd2.Quantity = 100;
            pd2.Status = statusRepository.FindById(11);

            PurchaseOrderDetail pd3 = new PurchaseOrderDetail();
            pd3.PurchaseOrderNo = "P11";
            pd3.Item = itemRepository.FindById("C001");
            pd3.Quantity = 100;
            pd3.Status = statusRepository.FindById(11);

            p1.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            p2.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            p1.PurchaseOrderDetails.Add(pd1);
            p2.PurchaseOrderDetails.Add(pd2);
            p1.PurchaseOrderDetails.Add(pd3);

            poList.Add(p1);
            poList.Add(p2);

            

            Item i = itemRepository.FindById("C001");
            Item i2 = itemRepository.FindById("C004");

            //Act
            var result = purchaseOrderService.IsPurchaseOrderCreated(i, poList);
            var result2 = purchaseOrderService.IsPurchaseOrderCreated(i2, poList);

            //Assert
            Assert.IsTrue(result);
            Assert.IsFalse(result2);
        }





    }
}
