using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class StockMovementServiceTests
    {
        ApplicationDbContext context;
        StockMovementService stockmovementService;
        ItemService itemService;

        StockMovementRepository stockmovementRepository;
        DisbursementDetailRepository ddRepository;
        StockAdjustmentDetailRepository sadRepository;
        DeliveryOrderDetailRepository dodRepository;
        DisbursementRepository disbursementRepository;
        DeliveryOrderRepository doRepository;
        StockAdjustmentRepository saRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            
            stockmovementService = new StockMovementService(context);
            itemService = new ItemService(context);

            stockmovementRepository = new StockMovementRepository(context);

            ddRepository = new DisbursementDetailRepository(context);
            sadRepository = new StockAdjustmentDetailRepository(context);
            dodRepository = new DeliveryOrderDetailRepository(context);
            disbursementRepository = new DisbursementRepository(context);
            doRepository = new DeliveryOrderRepository(context);
            saRepository = new StockAdjustmentRepository(context);

            //create new test object and save into db
            StockMovement sm = new StockMovement()
            {
                StockMovementId = IdService.GetNewStockMovementId(context),
                DisbursementId = "TEST",
                StockAdjustmentId = "TEST",
                DeliveryOrderNo = "TEST",
                Item = context.Item.Where(x => x.ItemCode == "C003").First(),
                CreatedDateTime = DateTime.Now,
                OriginalQuantity = 1,
                AfterQuantity = 2              

            };
            stockmovementRepository.Save(sm);
            
            //create new disbursement object and save into db
            Disbursement disbursement = new Disbursement();
            if (disbursementRepository.FindById("TEST") == null)
            {
                disbursement.DisbursementId = "TEST";
                disbursement.CreatedDateTime = DateTime.Now;
                

            }
            else disbursement = disbursementRepository.FindById("TEST");
            disbursementRepository.Save(disbursement);

            //create new DisbursementDetail object and save into db
            DisbursementDetail detail = new DisbursementDetail()
            {
                DisbursementId = "TEST",
                Item = context.Item.First(),
                PlanQuantity = 3,
                ActualQuantity = 3 

            };
            ddRepository.Save(detail);

            //create new DO object and save into db
            DeliveryOrder d = new DeliveryOrder();
            if (doRepository.FindById("TEST") == null)
            {
                d.DeliveryOrderNo = "TEST";
                d.CreatedDateTime = DateTime.Now;


            }
            else d = doRepository.FindById("TEST");
            doRepository.Save(d);

            //create new DO detail object and save into db
            DeliveryOrderDetail dod = new DeliveryOrderDetail() {
                DeliveryOrder = d,
                Item = context.Item.First(),
                PlanQuantity = 4,
                ActualQuantity = 4

            };
            dodRepository.Save(dod);

            //create new SA object and save into db
            StockAdjustment SA = new StockAdjustment();
            if (saRepository.FindById("TEST") == null)
            {
               SA.StockAdjustmentId = "TEST";
                SA.CreatedDateTime = DateTime.Now;


            }
            else SA = saRepository.FindById("TEST");
            saRepository.Save(SA);

            //create new SA detail object and save into db
            StockAdjustmentDetail SADetail = new StockAdjustmentDetail()
            {
                StockAdjustment = SA,
                Item = context.Item.First(),
                OriginalQuantity = 2,
                AfterQuantity = 4
           
            };
            sadRepository.Save(SADetail);
        }

        [TestMethod]
        public void FindAllStockMovementServiceTest()
        {
            //Arrange
            int expected = context.StockMovement.ToList().Count;

            //Act
            var result = stockmovementService.FindAllStockMovement().Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindStockMovementByItemCodeTest()
        {
            //Arrange
            StockMovement a = context.StockMovement.Where(x => x.DisbursementId == "TEST").First();
            int expected = context.StockMovement.Where(x => x.Item.ItemCode == a.Item.ItemCode).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByItemCode("a.Item.ItemCode");

            //Assert
            foreach(StockMovement i in result){
                Assert.AreEqual(a.Item.ItemCode, i.Item.ItemCode);
            }
        }

        [TestMethod]
        public void FindStockMovementByDisbursementIdTest()
        {
            //Arrange
            //Persist a dummy data object
            StockMovement a = new StockMovement();
            a.CreatedDateTime = DateTime.Now;
            a.DisbursementId = IdService.GetNewDisbursementId(context);
            a.Item = context.Item.First();
            stockmovementService.Save(a);

            int expected = context.StockMovement.Where(x => x.DisbursementId == a.DisbursementId).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByDisbursementId(a.DisbursementId).Count;
            stockmovementRepository.Delete(a);

            //Assert
            Assert.AreEqual(expected, result);

        }

        [TestMethod]
        public void FindStockMovementByStockAdjustmentIdTest()
        {
            //Arrange
            StockMovement a = new StockMovement();
            a.CreatedDateTime = DateTime.Now;
            a.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            a.Item = context.Item.First();
            stockmovementService.Save(a);

            int expected = context.StockMovement.Where(x => x.StockAdjustmentId == a.StockAdjustmentId).ToList().Count;

            //Act
            var result = stockmovementService.FindStockMovementByStockAdjustmentId(a.StockAdjustmentId).Count;
            stockmovementRepository.Delete(a); 

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SaveEditTest()
        {
            //Arrange
            StockMovement a = context.StockMovement.Where(x=>x.DisbursementId=="TEST").First();
            string expected = "DEMO";
            a.DisbursementId = expected;

            //Act
            var result = stockmovementService.Save(a);

            //Assert
            Assert.AreEqual(expected, result.DisbursementId);

        }

        [TestMethod]
        public void CreateDisbursementStockMovementTest()
        {
            //Arrange
            DisbursementDetail detail = context.DisbursementDetail.Where(x => x.DisbursementId == "TEST").First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
           var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.DisbursementId, result.DisbursementId);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }

        [TestMethod]
        public void CreateStockAdjustmentStockMovementTest()
        {
            //Arrange
            StockAdjustmentDetail detail = context.StockAdjustmentDetail.Where(x=>x.StockAdjustmentId=="TEST").First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
            var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.StockAdjustmentId, result.StockAdjustmentId);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }
        [TestMethod]
        public void CreateDeliveryOrderStockMovementTest()
        {
            //Arrange
            DeliveryOrderDetail detail = context.DeliveryOrderDetail.First();
            int expected = detail.Item.Inventory.Quantity;

            //Act
            var result = stockmovementService.CreateStockMovement(detail);

            //Assert
            Assert.AreEqual(detail.DeliveryOrderNo, result.DeliveryOrderNo);

            //Clean-up
            itemService.UpdateQuantity(detail.Item, expected); //reset item quantity after stock movement test
            stockmovementRepository.Delete(result);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            StockMovement a = context.StockMovement.Where(x => x.StockMovementId == 1).First();
            //Revert edit object
            a.DisbursementId = null;
            stockmovementService.Save(a);

            List<StockMovement> smlist = context.StockMovement.Where(x => x.StockAdjustmentId == "TEST").ToList();
            foreach (StockMovement sm in smlist)
            {
                stockmovementRepository.Delete(sm);
            }

            List<Disbursement> ddlist = context.Disbursement.Where(x => x.DisbursementId == "TEST").ToList();
            foreach (Disbursement dd in ddlist)
            {
                disbursementRepository.Delete(dd);
            }

            List<DeliveryOrder> dolist = context.DeliveryOrder.Where(x => x.DeliveryOrderNo == "TEST").ToList();
            foreach(DeliveryOrder d in dolist)
            {
                doRepository.Delete(d);
            }

            List<StockAdjustment> salist = context.StockAdjustment.Where(x => x.StockAdjustmentId == "TEST").ToList();
            foreach (StockAdjustment SA in salist)
            {
                saRepository.Delete(SA);
            }
        }

    }
}
