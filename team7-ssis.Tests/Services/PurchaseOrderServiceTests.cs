using System;
using team7_ssis.Models;
using team7_ssis.Repositories;
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
        PurchaseOrderRepository purchaseOrderRepository;
        PurchaseOrderDetailRepository purchaseOrderDetailRepository;
        StatusRepository statusRepository;
        ItemRepository itemRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemRepository = new ItemRepository(context);
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
        public void FindPurchaseOrderDetailsByIdTest()
        {
            //Arrange
            PurchaseOrderDetail p1 = new PurchaseOrderDetail();
            p1.PurchaseOrderNo = "TEST";
            p1.ItemCode = "E004";
            p1.Quantity = 50;

            PurchaseOrderDetail p2 = new PurchaseOrderDetail();
            p2.PurchaseOrderNo = "TEST";
            p2.ItemCode = "E007";
            p2.Quantity = 100;

            purchaseOrderDetailRepository.Save(p1);
            purchaseOrderDetailRepository.Save(p2);

            //Act
            var result = purchaseOrderService.FindPurchaseOrderDetailsById("TEST");
            var result2 = purchaseOrderService.FindPurchaseOrderDetailsById("DUMMYYYY");
            
            //Assert
            Assert.AreEqual(3, result.Count());
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrderDetail));
            result.ForEach(x => Assert.AreEqual("TEST", x.PurchaseOrderNo));

            Assert.AreEqual(result2.Any(), false);//since result2 is an empty collection and not null
            
            //teardown
            purchaseOrderDetailRepository.Delete(p1);
            purchaseOrderDetailRepository.Delete(p2);

        }
        

        [TestMethod]
        public void FindPurchaseOrderBySupplierTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "TEST2";
            p1.CreatedDateTime = DateTime.Now;
            p1.SupplierCode = "CHEP";
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


       


    }
}
