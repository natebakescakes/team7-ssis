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

            //create test PO object and save to db

            PurchaseOrder PO = new PurchaseOrder()
            {
                PurchaseOrderNo = "TEST",
                CreatedDateTime = DateTime.Now,
                Supplier = context.Supplier.Where(x => x.SupplierCode == "CHEP").First()

            };
            purchaseOrderRepository.Save(PO);

           // create test DO object and save to db
            DeliveryOrder DO = new DeliveryOrder()
            {
                DeliveryOrderNo = "DOTEST",
                PurchaseOrder = PO,
                CreatedDateTime = DateTime.Now,
                Supplier = context.Supplier.Where(x => x.SupplierCode == "CHEP").First()
            };
            deliveryOrderRepository.Save(DO);

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
        public void FindDeliveryOrderByIdTest()
        {
            //Arrange
            string expected = "DOTEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderById(expected);
            //Assert
            Assert.AreEqual(expected, result.DeliveryOrderNo);
        }

        [TestMethod]
        public void FindDeliveryOrderByPurchaseOrderNoTest()
        {
            //Arrange
            string expected = "TEST";
            //Act
            var result = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo(expected);

            //Assert
       //     Assert.AreEqual(expected, result.PurchaseOrderNo);
        }

        [TestMethod]
        public void FindDeliveryOrderBySupplierTest()
        {
           // Arrange
            string expected = "CHEP";
            // Act
            Supplier s = context.Supplier.Where(x => x.SupplierCode == "CHEP").First();

            //Assert
            Assert.AreEqual(expected, s.SupplierCode);
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

            //Act

            var result = deliveryOrderService.Save(d1);

            //Assert

            Assert.AreEqual("DDDD", result.DeliveryOrderNo);
            deliveryOrderRepository.Delete(d1);
        }

        [TestMethod]
        public void SaveDeliveryOrderDetailsTest()
        {
            //Arrange
            PurchaseOrder PO = new PurchaseOrder()
            {
                PurchaseOrderNo = "DODTEST",
                CreatedDateTime = DateTime.Now,
                Supplier = context.Supplier.Where(x => x.SupplierCode == "CHEP").First()

            };
            purchaseOrderRepository.Save(PO);

            // create test DO object and save to db
            DeliveryOrder DO = new DeliveryOrder()
            {
                DeliveryOrderNo = "DOTEST",
                PurchaseOrder = PO,
                CreatedDateTime = DateTime.Now,
                Supplier = context.Supplier.Where(x => x.SupplierCode == "CHEP").First()
            };
            deliveryOrderRepository.Save(DO);

            DeliveryOrderDetail dod = new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DOTEST",
                ItemCode = itemRepository.FindById("E030").ItemCode,
                PlanQuantity = 100,
                ActualQuantity = 50,
                Status = statusRepository.FindById(0)
            };

            //Act
            var result = deliveryOrderService.SaveDeliveryOrderDetails(dod);

            //Assert
            Assert.AreEqual("DOTEST", result.DeliveryOrderNo);

            deliveryOrderDetailRepository.Delete(dod);
        }

        [TestMethod]
        public void CheckSaveTest()
        {
            PurchaseOrder PO = context.PurchaseOrder.Where(x => x.PurchaseOrderNo == "TEST").First();
            purchaseOrderRepository.Save(PO);

            DeliveryOrder DO= context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "DOTEST").First();


            Item item = itemRepository.FindById("E030");

            DeliveryOrderDetail dod = new DeliveryOrderDetail()
            {
                DeliveryOrder = DO,
                DeliveryOrderNo = DO.DeliveryOrderNo,
                Item = item,
                ItemCode = item.ItemCode,
                PlanQuantity = 100,
                ActualQuantity = 50,
                Status = statusRepository.FindById(1),
                UpdatedDateTime = DateTime.Now
            };

            deliveryOrderService.SaveStockMovement(dod);
            deliveryOrderService.SaveInventory(dod.Item, 50);

            // Act
            var result = deliveryOrderService.SaveDeliveryOrderDetails(dod);


            //Assert
            Assert.AreEqual("DOTEST", result.DeliveryOrderNo);

            // clean
            PO.Status = statusRepository.FindById(15);
            purchaseOrderRepository.Save(PO);
            //StockMovement sm= context.StockMovement.Where(x => x.DisbursementDetailItemCode == "DUMMY").First();
            //stockMovementRepository.Delete(sm);
        }




        [TestMethod]
        public void SaveInventoryTest()
        {
         //   Arrange
            Item i = itemRepository.FindById("E030");

         //   Act
            var result = deliveryOrderService.SaveInventory(i, 50);
            Inventory inv = inventoryRepository.FindById("E030");
            inventoryRepository.Save(inv);

           // Arrange
            Assert.AreEqual("E030", result.ItemCode);

            //clean
            inv.Quantity = inv.Quantity - 50;
            inventoryRepository.Save(inv);
        }

        [TestMethod]
        public void SaveStockMovementTest()
        {
          //  Arrange
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


          //  Act
            var result = deliveryOrderService.SaveStockMovement(dod1);

          //  Arrange
            Assert.AreEqual("E030", result.Item.ItemCode);

         //   Clean
            stockMovementRepository.Delete(result);
            deliveryOrderDetailRepository.Delete(dod1);
            deliveryOrderRepository.Delete(d1);
        }



        [TestCleanup]
        public void TestClean()
        {
            List<DeliveryOrder> doList = context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "DOTEST").ToList();

            foreach (DeliveryOrder d in doList)
            {
                deliveryOrderRepository.Delete(d);
            }

            PurchaseOrder p = context.PurchaseOrder.Where(x => x.PurchaseOrderNo == "TEST").First();
            purchaseOrderRepository.Delete(p);

            Inventory inv = inventoryRepository.FindById("E030");
            inv.Quantity = inv.Quantity - 50;
            inventoryRepository.Save(inv);
        }
    }
}
