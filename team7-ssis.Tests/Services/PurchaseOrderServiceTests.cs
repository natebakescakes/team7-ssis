using System;
using team7_ssis.Models;
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
            p1.PurchaseOrderNo = "DUMMY";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrder p2 = new PurchaseOrder();
            p2.Status = statusRepository.FindById(12);
            p2.PurchaseOrderNo = "DUMMY2";
            p2.CreatedDateTime = DateTime.Now;

            PurchaseOrder p3 = new PurchaseOrder();
            p3.Status = statusRepository.FindById(13);
            p3.PurchaseOrderNo = "DUMMY3";
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
            p1.PurchaseOrderNo = "DUMMY";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrder p2 = new PurchaseOrder();
            p2.Status = statusRepository.FindById(15);
            p2.PurchaseOrderNo = "DUMMY2";
            p2.CreatedDateTime = DateTime.Now;

            purchaseOrderRepository.Save(p2);

            //Act
            var result1 = purchaseOrderService.Save(p1);
            p2.Supplier = supplierRepository.FindById("ALPA");
            var result2 = purchaseOrderService.Save(p2);

            //Assert
            Assert.AreEqual(result1.PurchaseOrderNo,"DUMMY");
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

        public void AddItemsToPurchaseOrdersTest()
        {

        }





    }
}
