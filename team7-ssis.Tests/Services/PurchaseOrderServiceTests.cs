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
        ItemPriceRepository itemPriceRepository;
        SupplierRepository supplierRepository;
        DeliveryOrderRepository deliveryOrderRepository;
        DeliveryOrderDetailRepository deliveryOrderDetailRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            purchaseOrderRepository = new PurchaseOrderRepository(context);
            purchaseOrderDetailRepository = new PurchaseOrderDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemRepository = new ItemRepository(context);
            itemPriceRepository = new ItemPriceRepository(context);
            supplierRepository = new SupplierRepository(context);
            deliveryOrderRepository = new DeliveryOrderRepository(context);
            deliveryOrderDetailRepository = new DeliveryOrderDetailRepository(context);
        }

        [TestMethod]
        public void FindAllPurchaseOrdersTest()
        {
            //Arrange
            int expected = context.PurchaseOrder.Count();

            //Act
            var result = purchaseOrderService.FindAllPurchaseOrders();

            //Assert
            Assert.AreEqual(expected, result.Count());
            CollectionAssert.AllItemsAreUnique(result);
            CollectionAssert.AllItemsAreNotNull(result);
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));

        }


        [TestMethod]
        
        public void FindPurchaseOrderByIdTest()
        {
            //Arrange
            PurchaseOrder p = new PurchaseOrder();
            p.PurchaseOrderNo = "PUR-1";
            p.Supplier = supplierRepository.FindById("CHEP");
            p.CreatedDateTime = DateTime.Now;
            p.Status = statusRepository.FindById(11);

            purchaseOrderRepository.Save(p);

            //Act
            var result = purchaseOrderService.FindPurchaseOrderById("PUR-1");
            //var result2 = purchaseOrderService.FindPurchaseOrderById("TEST");

            //Assert
            Assert.AreEqual("CHEP", result.SupplierCode);
            //Assert.IsNull(result2);

        }




        [TestMethod]
        public void FindPurchaseOrderBySupplierTest()
        {
            //Arrange
            Supplier s1 = new Supplier();
            s1.SupplierCode = "SUP1";
            s1.CreatedDateTime = DateTime.Now;
            supplierRepository.Save(s1);

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "PUR-3";
            p1.CreatedDateTime = DateTime.Now;
            p1.Supplier = s1;
            purchaseOrderRepository.Save(p1);

            //Act
            var result = purchaseOrderService.FindPurchaseOrderBySupplier("SUP1");
            var result1 = purchaseOrderService.FindPurchaseOrderBySupplier("SUP2");
            var result2 = purchaseOrderService.FindPurchaseOrderBySupplier("CHEP");

            //Assert
            Assert.AreEqual(result1.Any(), false);
            Assert.AreEqual(result.Any(), true);
            CollectionAssert.AllItemsAreInstancesOfType(result2, typeof(PurchaseOrder));
            result.ForEach(x => Assert.AreEqual("SUP1", x.SupplierCode));
            result2.ForEach(y => Assert.AreEqual("CHEP", y.SupplierCode));


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
            p1.Item = itemRepository.FindById("E004");
            p1.Quantity = 50;
            p1.Status = statusRepository.FindById(11);

            PurchaseOrderDetail p2 = new PurchaseOrderDetail();
            p2.PurchaseOrderNo = "DUMMY";
            p2.Item = itemRepository.FindById("E007");
            p2.Quantity = 100;
            p2.Status = statusRepository.FindById(11);

            PurchaseOrderDetail p3 = new PurchaseOrderDetail();
            p3.PurchaseOrderNo = "DUMMY";
            p3.Item = itemRepository.FindById("E030");
            p3.Quantity = 100;
            p3.Status = statusRepository.FindById(11);

            purchaseOrderRepository.Save(p);
            purchaseOrderDetailRepository.Save(p1);
            purchaseOrderDetailRepository.Save(p2);
            purchaseOrderDetailRepository.Save(p3);

            string[] itemCodes = new string[] { "E030" };

            //Act
            purchaseOrderService.DeleteItemFromPurchaseOrder(p, itemCodes);

            //Assert
            var expected = context.PurchaseOrderDetail.Where(x => x.PurchaseOrderNo == "DUMMY" && x.ItemCode == "E030");
            Assert.AreEqual(expected.Any(), false);

            //tear down
            //purchaseOrderRepository.Delete(p);
        }

        [TestMethod]
        public void FindPurchaseOrderByStatusTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.Status = statusRepository.FindById(14);
            p1.PurchaseOrderNo = "DUMMYS1";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrder p2 = new PurchaseOrder();
            p2.Status = statusRepository.FindById(15);
            p2.PurchaseOrderNo = "DUMMYS2";
            p2.CreatedDateTime = DateTime.Now;

            PurchaseOrder p3 = new PurchaseOrder();
            p3.Status = statusRepository.FindById(13);
            p3.PurchaseOrderNo = "DUMMYS3";
            p3.CreatedDateTime = DateTime.Now;

            purchaseOrderRepository.Save(p1);
            purchaseOrderRepository.Save(p2);
            purchaseOrderRepository.Save(p3);

            int[] statusId = new int[] { 14, 15 };

            //Act
            var result = purchaseOrderService.FindPurchaseOrderByStatus(statusId);

            //Assert
            result.ForEach(x => Assert.IsTrue(x.Status.StatusId == 14 || x.Status.StatusId == 15));
            Assert.AreEqual(result.Count(), 2);

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
            Assert.AreEqual(result1.PurchaseOrderNo, "DUMMYSA1");
            Assert.AreEqual(result2.SupplierCode, "ALPA");

            //teardown
            //purchaseOrderRepository.Delete(p1);
            //purchaseOrderRepository.Delete(p2);

        }

        [TestMethod]
       
        public void CreatePOForEachSupplierTest()
        {
            //Arrange
            List<Supplier> supplier = new List<Supplier>();
            supplier.Add(supplierRepository.FindById("CHEP"));
            supplier.Add(supplierRepository.FindById("ALPA"));

            //Act
            var result = purchaseOrderService.CreatePOForEachSupplier(supplier);
            foreach(PurchaseOrder p in result)
            {
                p.ApprovedDateTime = new DateTime(1993, 7, 9);
                purchaseOrderRepository.Save(p);
            }

            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(PurchaseOrder));
            result.ForEach(x => Assert.IsTrue(x.Supplier.SupplierCode == "CHEP" || x.Supplier.SupplierCode == "ALPA"));
            result.ForEach(y => Assert.IsTrue(y.Status.StatusId == 11));
            Assert.AreEqual(result.Count(), 2);
        }

      

        //[TestMethod]
        //public void IsPurchaseOrderCreatedTest()
        //{
        //    //Arrange
        //    List<PurchaseOrder> poList = new List<PurchaseOrder>();

        //    PurchaseOrder p1 = new PurchaseOrder();
        //    p1.PurchaseOrderNo = "P11";
        //    p1.Supplier = supplierRepository.FindById("CHEP");

        //    PurchaseOrder p2 = new PurchaseOrder();
        //    p2.PurchaseOrderNo = "P21";
        //    p2.Supplier = supplierRepository.FindById("BANE");

        //    PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
        //    pd1.PurchaseOrderNo = "P11";
        //    pd1.Item = itemRepository.FindById("E005");
        //    pd1.Quantity = 50;
        //    pd1.Status = statusRepository.FindById(11);

        //    PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
        //    pd2.PurchaseOrderNo = "P21";
        //    pd2.Item = itemRepository.FindById("E007");
        //    pd2.Quantity = 100;
        //    pd2.Status = statusRepository.FindById(11);

        //    PurchaseOrderDetail pd3 = new PurchaseOrderDetail();
        //    pd3.PurchaseOrderNo = "P11";
        //    pd3.Item = itemRepository.FindById("C001");
        //    pd3.Quantity = 100;
        //    pd3.Status = statusRepository.FindById(11);

        //    p1.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
        //    p2.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

        //    p1.PurchaseOrderDetails.Add(pd1);
        //    p2.PurchaseOrderDetails.Add(pd2);
        //    p1.PurchaseOrderDetails.Add(pd3);

        //    poList.Add(p1);
        //    poList.Add(p2);



        //    Item i = itemRepository.FindById("C001");
        //    Item i2 = itemRepository.FindById("C004");

        //    //Act
        //    var result = purchaseOrderService.IsPurchaseOrderCreated(i, poList);
        //    var result2 = purchaseOrderService.IsPurchaseOrderCreated(i2, poList);

        //    //Assert
        //    Assert.IsTrue(result);
        //    Assert.IsFalse(result2);
        //}



        [TestMethod]
        public void FindUnitPriceByPurchaseOrderDetailTest()
        {
            //Arrange

            Item i = new Item();
            i.ItemCode = "ITEM";
            i.ReorderLevel = 100;
            i.ReorderQuantity = 500;
            i.CreatedDateTime = DateTime.Now;
           


            ItemPrice itemP = new ItemPrice();
            itemP.Item = i;
            itemP.ItemCode = i.ItemCode;
            itemP.Supplier = supplierRepository.FindById("CHEP");
            itemP.Supplier.SupplierCode = "CHEP";
            itemP.Price = 5.0M;
            itemP.PrioritySequence = 1;
            itemP.CreatedDateTime = DateTime.Now;

            

            itemRepository.Save(i);

            itemPriceRepository.Save(itemP);

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "PO1";
            p1.Supplier = supplierRepository.FindById("CHEP");
            p1.SupplierCode = "CHEP";
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrder = p1;
            pd1.PurchaseOrderNo = "PO1";
            pd1.Item = itemRepository.FindById("ITEM");
            pd1.ItemCode = i.ItemCode;
            pd1.Quantity = 50;
            pd1.Status = statusRepository.FindById(11);

            p1.PurchaseOrderDetails = new List<PurchaseOrderDetail>();
            p1.PurchaseOrderDetails.Add(pd1);


            purchaseOrderRepository.Save(p1);
            purchaseOrderDetailRepository.Save(pd1);


            var result = purchaseOrderService.FindUnitPriceByPurchaseOrderDetail(pd1);
            //var result = itemRepository.FindById(i.ItemCode);

            //Assert
            Assert.AreEqual(result, 5.0M);


        }

        [TestMethod]

        public void FindTotalAmountByPurchaseOrderDetailTest()
        {
            //Arrange

            Item i5 = new Item();
            i5.ItemCode = "ITM2";
            i5.ReorderLevel = 100;
            i5.ReorderQuantity = 500;
            i5.CreatedDateTime = DateTime.Now;
            

            ItemPrice itemP5 = new ItemPrice();
            itemP5.Item = i5;
            itemP5.ItemCode = i5.ItemCode;
            itemP5.Supplier = supplierRepository.FindById("CHEP");
            itemP5.Supplier.SupplierCode = "CHEP";
            itemP5.Price = 5.0M;
            itemP5.PrioritySequence = 1;
            itemP5.CreatedDateTime = DateTime.Now;

            //i5.ItemPrices.Add(itemP5);
            itemRepository.Save(i5);

            itemPriceRepository.Save(itemP5);

            PurchaseOrder p5 = new PurchaseOrder();
            p5.PurchaseOrderNo = "PO2";
            p5.Status = statusRepository.FindById(11);
            p5.Supplier = supplierRepository.FindById("CHEP");
            p5.SupplierCode = "CHEP";
            p5.CreatedDateTime = DateTime.Now;

            PurchaseOrderDetail pd5 = new PurchaseOrderDetail();
            pd5.PurchaseOrder = p5;
            pd5.PurchaseOrderNo = "PO2";
            pd5.Item = itemRepository.FindById("ITM2");
            pd5.ItemCode = i5.ItemCode;
            pd5.Quantity = 10;
            pd5.Status = statusRepository.FindById(11);

            p5.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            p5.PurchaseOrderDetails.Add(pd5);


            purchaseOrderRepository.Save(p5);
            purchaseOrderDetailRepository.Save(pd5);

            var unitPrice = purchaseOrderService.FindUnitPriceByPurchaseOrderDetail(pd5);

            //Act
            var total = purchaseOrderService.FindTotalAmountByPurchaseOrderDetail(pd5);

            //Assert
            Assert.AreEqual(50.0M, total);
        }

        [TestMethod]
        public void FindReceivedQuantityByPurchaseOrderDetailTest()
        {

            Item i = new Item();
            i.ItemCode = "IT3";
            i.ReorderLevel = 100;
            i.ReorderQuantity = 500;
            i.CreatedDateTime = DateTime.Now;

            itemRepository.Save(i);

            Item i2 = new Item();
            i2.ItemCode = "IT4";
            i2.ReorderLevel = 100;
            i2.ReorderQuantity = 500;
            i2.CreatedDateTime = DateTime.Now;

            itemRepository.Save(i2);
            

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "PO3";
            p1.Status = statusRepository.FindById(12);
            p1.Status.StatusId = 12;
            p1.Supplier = supplierRepository.FindById("CHEP");
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrder = p1;
            pd1.PurchaseOrderNo = "PO3";
            pd1.Item = itemRepository.FindById("IT3");
            pd1.ItemCode = i.ItemCode;
            pd1.Quantity = 15;
            pd1.Status = statusRepository.FindById(12);

            PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
            pd2.PurchaseOrder = p1;
            pd2.PurchaseOrderNo = "PO3";
            pd2.Item = itemRepository.FindById("IT4");
            pd2.ItemCode = i2.ItemCode;
            pd2.Quantity = 50;
            pd2.Status = statusRepository.FindById(12);

            p1.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            p1.PurchaseOrderDetails.Add(pd1);
            p1.PurchaseOrderDetails.Add(pd2);


            purchaseOrderRepository.Save(p1);
            purchaseOrderDetailRepository.Save(pd1);
            purchaseOrderDetailRepository.Save(pd2);

            DeliveryOrder d1 = new DeliveryOrder();
            d1.DeliveryOrderNo = "DO1";
            d1.PurchaseOrder = p1;
            d1.PurchaseOrder.PurchaseOrderNo = "PO3";
            d1.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail do1 = new DeliveryOrderDetail();
            do1.DeliveryOrderNo = "DO1";
            do1.Item = itemRepository.FindById("IT3");
            do1.ItemCode = i.ItemCode;
            do1.ActualQuantity = 10;
            do1.PlanQuantity = 15;

            DeliveryOrderDetail do2 = new DeliveryOrderDetail();
            do2.DeliveryOrderNo = "DO1";
            do2.Item = itemRepository.FindById("IT4");
            do2.ItemCode = i2.ItemCode;
            do2.ActualQuantity = 10;
            do2.PlanQuantity = 50;

            d1.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            d1.DeliveryOrderDetails.Add(do1);
            d1.DeliveryOrderDetails.Add(do2);
            
            deliveryOrderRepository.Save(d1);

            deliveryOrderDetailRepository.Save(do1);
            deliveryOrderDetailRepository.Save(do2);


            DeliveryOrder d2 = new DeliveryOrder();
            d2.DeliveryOrderNo = "DO2";
            d2.PurchaseOrder = p1;
            d2.PurchaseOrder.PurchaseOrderNo = "PO3";
            d2.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail do3 = new DeliveryOrderDetail();
            do3.DeliveryOrderNo = "DO2";
            do3.Item = itemRepository.FindById("IT3");
            do3.ItemCode = i.ItemCode;
            do3.ActualQuantity = 2;
            do3.PlanQuantity = 15;

            DeliveryOrderDetail do4 = new DeliveryOrderDetail();
            do4.DeliveryOrderNo = "DO2";
            do4.Item = itemRepository.FindById("IT4");
            do4.ItemCode = i2.ItemCode;
            do4.ActualQuantity = 15;
            do4.PlanQuantity = 50;

            d2.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            d2.DeliveryOrderDetails.Add(do3);
            d2.DeliveryOrderDetails.Add(do4);

            deliveryOrderRepository.Save(d2);

            deliveryOrderDetailRepository.Save(do3);
            deliveryOrderDetailRepository.Save(do4);


            //Act
            var result=purchaseOrderService.FindReceivedQuantityByPurchaseOrderDetail(pd1);
            var result2 = purchaseOrderService.FindReceivedQuantityByPurchaseOrderDetail(pd2);

            //Assert
            Assert.AreEqual(result, 12);
            Assert.AreEqual(result2, 25);


        }

   

        [TestMethod]
        public void FindRemainingQuantityTest()
        {

            Item i = new Item();
            i.ItemCode = "IT5";
            i.ReorderLevel = 100;
            i.ReorderQuantity = 500;
            i.CreatedDateTime = DateTime.Now;

            itemRepository.Save(i);

            Item i2 = new Item();
            i2.ItemCode = "IT6";
            i2.ReorderLevel = 100;
            i2.ReorderQuantity = 500;
            i2.CreatedDateTime = DateTime.Now;

            itemRepository.Save(i2);


            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "PO4";
            p1.Status = statusRepository.FindById(12);
            p1.Status.StatusId = 12;
            p1.Supplier = supplierRepository.FindById("CHEP");
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrder = p1;
            pd1.PurchaseOrderNo = "PO4";
            pd1.Item = itemRepository.FindById("IT5");
            pd1.ItemCode = i.ItemCode;
            pd1.Quantity = 15;
            pd1.Status = statusRepository.FindById(12);

            PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
            pd2.PurchaseOrder = p1;
            pd2.PurchaseOrderNo = "PO4";
            pd2.Item = itemRepository.FindById("IT6");
            pd2.ItemCode = i2.ItemCode;
            pd2.Quantity = 50;
            pd2.Status = statusRepository.FindById(12);

            p1.PurchaseOrderDetails = new List<PurchaseOrderDetail>();

            p1.PurchaseOrderDetails.Add(pd1);
            p1.PurchaseOrderDetails.Add(pd2);


            purchaseOrderRepository.Save(p1);
            purchaseOrderDetailRepository.Save(pd1);
            purchaseOrderDetailRepository.Save(pd2);

            DeliveryOrder d1 = new DeliveryOrder();
            d1.DeliveryOrderNo = "DO3";
            d1.PurchaseOrder = p1;
            d1.PurchaseOrder.PurchaseOrderNo = "PO4";
            d1.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail do1 = new DeliveryOrderDetail();
            do1.DeliveryOrderNo = "DO3";
            do1.Item = itemRepository.FindById("IT5");
            do1.ItemCode = i.ItemCode;
            do1.ActualQuantity = 10;
            do1.PlanQuantity = 15;

            DeliveryOrderDetail do2 = new DeliveryOrderDetail();
            do2.DeliveryOrderNo = "DO3";
            do2.Item = itemRepository.FindById("IT6");
            do2.ItemCode = i2.ItemCode;
            do2.ActualQuantity = 10;
            do2.PlanQuantity = 50;

            d1.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            d1.DeliveryOrderDetails.Add(do1);
            d1.DeliveryOrderDetails.Add(do2);

            deliveryOrderRepository.Save(d1);

            deliveryOrderDetailRepository.Save(do1);
            deliveryOrderDetailRepository.Save(do2);


            DeliveryOrder d2 = new DeliveryOrder();
            d2.DeliveryOrderNo = "DO4";
            d2.PurchaseOrder = p1;
            d2.PurchaseOrder.PurchaseOrderNo = "PO4";
            d2.CreatedDateTime = DateTime.Now;

            DeliveryOrderDetail do3 = new DeliveryOrderDetail();
            do3.DeliveryOrderNo = "DO4";
            do3.Item = itemRepository.FindById("IT5");
            do3.ItemCode = i.ItemCode;
            do3.ActualQuantity = 2;
            do3.PlanQuantity = 15;

            DeliveryOrderDetail do4 = new DeliveryOrderDetail();
            do4.DeliveryOrderNo = "DO4";
            do4.Item = itemRepository.FindById("IT6");
            do4.ItemCode = i2.ItemCode;
            do4.ActualQuantity = 15;
            do4.PlanQuantity = 50;

            d2.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            d2.DeliveryOrderDetails.Add(do3);
            d2.DeliveryOrderDetails.Add(do4);

            deliveryOrderRepository.Save(d2);

            deliveryOrderDetailRepository.Save(do3);
            deliveryOrderDetailRepository.Save(do4);

            //act
            var remainingitem1 = purchaseOrderService.FindRemainingQuantity(pd1);
            var remainingitem2 = purchaseOrderService.FindRemainingQuantity(pd2);

            //assert
            Assert.AreEqual(3, remainingitem1);
            Assert.AreEqual(25, remainingitem2);


        }

        [TestMethod]
        public void CancelItemFromPurchaseOrderTest()
        {

            //Arrange
            Item i = new Item();
            i.ItemCode = "IT9";
            i.ReorderLevel = 100;
            i.ReorderQuantity = 500;
            i.CreatedDateTime = DateTime.Now;

            Item i2 = new Item();
            i2.ItemCode = "IT8";
            i2.ReorderLevel = 100;
            i2.ReorderQuantity = 500;
            i2.CreatedDateTime = DateTime.Now;

            itemRepository.Save(i);
            itemRepository.Save(i2);

            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "PUR-4";
            p1.Status = statusRepository.FindById(12);
            p1.Status.StatusId = 12;
            p1.Supplier = supplierRepository.FindById("CHEP");
            p1.CreatedDateTime = DateTime.Now;

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrder = p1;
            pd1.PurchaseOrderNo = "PUR-4";
            pd1.Item = itemRepository.FindById("IT9");
            pd1.ItemCode = i.ItemCode;
            pd1.Quantity = 15;
            pd1.Status = statusRepository.FindById(12);

            PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
            pd2.PurchaseOrder = p1;
            pd2.PurchaseOrderNo = "PUR-4";
            pd2.Item = itemRepository.FindById("IT8");
            pd2.ItemCode = i2.ItemCode;
            pd2.Quantity = 50;
            pd2.Status = statusRepository.FindById(11);

            purchaseOrderRepository.Save(p1);
            purchaseOrderDetailRepository.Save(pd1);
            purchaseOrderDetailRepository.Save(pd2);

            //Act
            purchaseOrderService.CancelItemFromPurchaseOrder("PUR-4","IT9");
            purchaseOrderService.CancelItemFromPurchaseOrder("PUR-4", "IT8");

            //Assert
            Assert.AreEqual(purchaseOrderDetailRepository.FindById("PUR-4", "IT9").Status.StatusId,12);
            Assert.AreEqual(purchaseOrderDetailRepository.FindById("PUR-4", "IT8").Status.StatusId, 2);

        }


        [TestCleanup]
        public void Cleanup()
        {
            //1
            bool check= purchaseOrderRepository.ExistsById("PUR-1");
            if (check)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PUR-1"));

            check = purchaseOrderRepository.ExistsById("PUR-3");
            if (check)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PUR-3"));

            check = supplierRepository.ExistsById("SUP1");
            if (check)
                supplierRepository.Delete(supplierRepository.FindById("SUP1"));

            check = itemRepository.ExistsById("IT8");
            if (check)
                itemRepository.Delete(itemRepository.FindById("IT8"));

            check = itemRepository.ExistsById("IT9");
            if (check)
                itemRepository.Delete(itemRepository.FindById("IT9"));

            check = purchaseOrderRepository.ExistsById("PUR-4");
            if (check)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PUR-4"));




            //3
            bool exist = purchaseOrderRepository.ExistsById("TEST10");
            if(exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("TEST10"));

            //5
            exist = purchaseOrderRepository.ExistsById("DUMMY");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMY"));

            //6

             exist = purchaseOrderRepository.ExistsById("DUMMYS1");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMYS1"));

            exist = purchaseOrderRepository.ExistsById("DUMMYS2");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMYS2"));

            exist = purchaseOrderRepository.ExistsById("DUMMYS3");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMYS3"));

            //7
            exist = purchaseOrderRepository.ExistsById("DUMMYSA1");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMYSA1"));

            exist = purchaseOrderRepository.ExistsById("DUMMYSA2");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("DUMMYSA2"));

            exist = purchaseOrderRepository.ExistsById("STATUSTEST");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("STATUSTEST"));

            //11
            exist = purchaseOrderRepository.ExistsById("PO1");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PO1"));

            exist = purchaseOrderDetailRepository.ExistsById("PO1","ITEM");
            if (exist)
                purchaseOrderDetailRepository.Delete(purchaseOrderDetailRepository.FindById("PO1", "ITEM"));

            exist = itemRepository.ExistsById("ITEM");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("ITEM"));

            exist = itemPriceRepository.ExistsById("ITEM","CHEP");
            if (exist)
                itemPriceRepository.Delete(itemPriceRepository.FindById("ITEM","CHEP"));


            //12
            exist = purchaseOrderRepository.ExistsById("PO2");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PO2"));


            exist = itemRepository.ExistsById("ITM2");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("ITM2"));

            exist = itemPriceRepository.ExistsById("ITM2", "CHEP");
            if (exist)
                itemPriceRepository.Delete(itemPriceRepository.FindById("ITM2", "CHEP"));



            //13
            exist = deliveryOrderRepository.ExistsById("DO1");
            if (exist)
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DO1"));

            exist = deliveryOrderRepository.ExistsById("DO2");
            if (exist)
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DO2"));

            exist = purchaseOrderRepository.ExistsById("PO3");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PO3"));

            exist = itemRepository.ExistsById("IT3");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("IT3"));

            exist = itemRepository.ExistsById("IT4");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("IT4"));

            //14

            exist = deliveryOrderRepository.ExistsById("DO3");
            if (exist)
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DO3"));

            exist = deliveryOrderRepository.ExistsById("DO4");
            if (exist)
                deliveryOrderRepository.Delete(deliveryOrderRepository.FindById("DO4"));

            exist = purchaseOrderRepository.ExistsById("PO4");
            if (exist)
                purchaseOrderRepository.Delete(purchaseOrderRepository.FindById("PO4"));

            exist = itemRepository.ExistsById("IT5");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("IT5"));

            exist = itemRepository.ExistsById("IT6");
            if (exist)
                itemRepository.Delete(itemRepository.FindById("IT6"));


            List<PurchaseOrder> poLIST=context.PurchaseOrder.Where(x => x.ApprovedDateTime == new DateTime(1993,7,9)).ToList();
            foreach(PurchaseOrder p in poLIST)
            {
                purchaseOrderRepository.Delete(p);
            }
        }

        [TestMethod]
        public void FindPurchaseOrderDetailByIdStatusTest()
        {
            //Arrange
            PurchaseOrder p1 = new PurchaseOrder();
            p1.PurchaseOrderNo = "STATUSTEST";
            p1.CreatedDateTime = DateTime.Now;
            purchaseOrderRepository.Save(p1);

            PurchaseOrderDetail pd1 = new PurchaseOrderDetail();
            pd1.PurchaseOrderNo = p1.PurchaseOrderNo;
            pd1.Item = itemRepository.FindById("E005");
            pd1.Status = statusRepository.FindById(11);
            purchaseOrderDetailRepository.Save(pd1);

            PurchaseOrderDetail pd2 = new PurchaseOrderDetail();
            pd2.PurchaseOrderNo = p1.PurchaseOrderNo;
            pd2.Item = itemRepository.FindById("E007");
            pd2.Status = statusRepository.FindById(12);
            purchaseOrderDetailRepository.Save(pd2);

            PurchaseOrderDetail pd3 = new PurchaseOrderDetail();
            pd3.PurchaseOrderNo = p1.PurchaseOrderNo;
            pd3.Item = itemRepository.FindById("C001");
            pd3.Status = statusRepository.FindById(2);
            purchaseOrderDetailRepository.Save(pd3);

            int[] statusId = new int[] { 11, 12 };

            //Act
            var result = purchaseOrderService.FindPurchaseOrderDetailByIdStatus(p1.PurchaseOrderNo,statusId);

            //Assert
            result.ForEach(x => Assert.IsTrue((x.Status.StatusId == 11 || x.Status.StatusId == 12) &&(x.PurchaseOrderNo=="STATUSTEST")));
            Assert.AreEqual(result.Count(), 2);

        }
    }
}
