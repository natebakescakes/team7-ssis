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
                PurchaseOrderNo = "VALLI",
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
            string expected = "VALLI";
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

        //save delivery order test
        [TestMethod]
        public void SaveTest()
        {
            // Arrange
            PurchaseOrder po = purchaseOrderRepository.FindById("VALLI");
            DeliveryOrder d1 = new DeliveryOrder();
            d1.DeliveryOrderNo = "DODTEST";
            d1.PurchaseOrder = po;
            d1.CreatedDateTime = DateTime.Now;

            //Act

            var result = deliveryOrderService.Save(d1);

            //Assert

            Assert.AreEqual("DODTEST", result.DeliveryOrderNo);
            deliveryOrderRepository.Delete(d1);
        }

        [TestMethod]
        public void SaveDeliveryOrderDetailsTest()
        {
            //Arrange

            PurchaseOrder PO= purchaseOrderRepository.FindById("VALLI");
            DeliveryOrder DO = deliveryOrderRepository.FindById("DOTEST");

            DeliveryOrderDetail dod = new DeliveryOrderDetail()
            {
                DeliveryOrderNo = "DOTEST",
                ItemCode = itemRepository.FindById("C003").ItemCode,
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
            PurchaseOrder PO = context.PurchaseOrder.Where(x => x.PurchaseOrderNo == "VALLI").First();

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
            //PO.Status = statusRepository.FindById(15);
            //purchaseOrderRepository.Save(PO);
            Inventory inv = inventoryRepository.FindById("E030");
            inv.Quantity = inv.Quantity - 50;
            inventoryRepository.Save(inv);
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

            PurchaseOrder po = purchaseOrderRepository.FindById("VALLI");

            DeliveryOrder DO = context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "DOTEST").First();

            DeliveryOrderDetail DOD = new DeliveryOrderDetail()
            {
                DeliveryOrder = DO,
                DeliveryOrderNo = DO.DeliveryOrderNo,
                Item = i,
                ItemCode = i.ItemCode,
                PlanQuantity = 100,
                ActualQuantity = 50,
                Status = statusRepository.FindById(1),
                UpdatedDateTime = DateTime.Now
            };

            List<DeliveryOrderDetail> list = new List<DeliveryOrderDetail>
            {
                DOD
            };
            DO.DeliveryOrderDetails = list;
            new DeliveryOrderRepository(context).Save(DO);
            new DeliveryOrderDetailRepository(context).Save(DOD);


          //  Act
            var result = deliveryOrderService.SaveStockMovement(DOD);

          //  Arrange
            Assert.AreEqual("E030", result.Item.ItemCode);
        }



        [TestCleanup]
        public void TestClean()
        {
            StockMovement sm = context.StockMovement.Where(x => x.DeliveryOrderNo == "DOTEST").FirstOrDefault();
            if(sm!=null)
                stockMovementRepository.Delete(sm);

            List<DeliveryOrder> doList = context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "DOTEST").ToList();
            if (doList.Count > 0)
            {
                foreach (DeliveryOrder d in doList)
                {
                    deliveryOrderRepository.Delete(d);
                }
            }

            PurchaseOrder p = context.PurchaseOrder.Where(x => x.PurchaseOrderNo == "VALLI").First();
            purchaseOrderRepository.Delete(p);

            List<DeliveryOrderDetail> dod= context.DeliveryOrderDetail.Where(x => x.DeliveryOrderNo == "DOTEST").ToList();
            if (dod.Count > 0)
            {
                foreach (DeliveryOrderDetail d in dod)
                {
                    deliveryOrderDetailRepository.Delete(d);
                }
            }

           List<DeliveryOrderDetail> dod1 = context.DeliveryOrderDetail.Where(x => x.DeliveryOrderNo == "DODTEST").ToList();
            if (dod.Count > 0)
            {
                foreach (DeliveryOrderDetail d in dod)
                {
                    deliveryOrderDetailRepository.Delete(d);
                }
            }
        }
    }
}
