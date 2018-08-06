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
    public class StockAdjustmentServiceTest
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        StockAdjustmentService service;
        // ItemService itemService;
        ItemRepository itemRepository;
        InventoryRepository inventoryRepository;
        ItemService itemService;
        StockMovementRepository stockMovementRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
            stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
            service = new StockAdjustmentService(context);
            itemRepository = new ItemRepository(context);
            inventoryRepository = new InventoryRepository(context);
            itemService = new ItemService(context);
            this.stockMovementRepository = new StockMovementRepository(context);

            //save new item object into db
            Item item = new Item();
            item.ItemCode = "he06";
            item.CreatedDateTime = DateTime.Now;
            itemRepository.Save(item);
            itemService.SaveInventory(item, 40);


        }

        //create new StockAdjustment with status: draft
        [TestMethod()]
        public void CreateDraftStockAdjustmentTest()
        {
            //Arrange 
            StockAdjustment expect = new StockAdjustment();
            //Random rd = new Random();
            //int i = rd.Next();
            string id = "he01";
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            // Act     
            try
            {
                var result = service.CreateDraftStockAdjustment(expect);
                Assert.AreEqual(3, result.Status.StatusId);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find such status"));
            }

        }

        //Delete one item if StockAdjustment in Draft Status
        [TestMethod()]
        public void DeleteItemFromDraftOrPendingStockAdjustmentTest()
        {
            //Arrange 
            Random rd = new Random();
            int i = rd.Next();
            string id = "he02";
            StockAdjustment expect = new StockAdjustment();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            StockAdjustmentDetail s1 = new StockAdjustmentDetail();
            s1.StockAdjustmentId = id;
            s1.ItemCode = "C003";
            s1.OriginalQuantity = 10;
            s1.AfterQuantity = 20;
            StockAdjustmentDetail s2 = new StockAdjustmentDetail();
            s2.StockAdjustmentId = id;
            s2.ItemCode = "C002";
            s2.OriginalQuantity = 20;
            s2.AfterQuantity = 30;
            List<StockAdjustmentDetail> list = new List<StockAdjustmentDetail>();
            list.Add(s1);
            list.Add(s2);
            expect.StockAdjustmentDetails = list;
            service.CreateDraftStockAdjustment(expect);
            string delete_Item = "C003";

            //test can't find StockAdjustment
            try
            {

                var result = service.DeleteItemFromDraftOrPendingStockAdjustment(id, "123");
                //Assert
                Assert.AreEqual(delete_Item, result);
                Assert.AreEqual(stockAdjustmentDetailRepository.FindById(id, result), null);
                Assert.AreEqual(expect.StockAdjustmentDetails.Count, 1);
                //stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            { Assert.IsTrue(e.Message.Contains("can't find stockAdjustmentDetail")); }


            // test can't find stockAdjustment
            try
            {

                var result = service.DeleteItemFromDraftOrPendingStockAdjustment("3", delete_Item);//don't exist
                //Assert
                Assert.AreEqual(delete_Item, result);
                Assert.AreEqual(stockAdjustmentDetailRepository.FindById(id, result), null);
                Assert.AreEqual(expect.StockAdjustmentDetails.Count, 1);
                // stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            { Assert.IsTrue(e.Message.Contains("can't find StockAdjustment")); }

            // No Exception part    
            try
            {
                string delete_Item1 = "C003";
                var result = service.DeleteItemFromDraftOrPendingStockAdjustment(id, delete_Item1);
                //Assert
                Assert.AreEqual(delete_Item1, result);
                Assert.AreEqual(stockAdjustmentDetailRepository.FindById(id, result), null);
                Assert.AreEqual(expect.StockAdjustmentDetails.Count, 1);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            { Assert.IsTrue(e.Message.Contains("can't find StockAdjustmentDetail")); }


        }

        //Cancel StockAdjustment in Draft or Pending Status
        [TestMethod()]
        public void CancelDraftOrPendingStockAdjustmentTest()
        {
            //Arrange
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = "he03";
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            service.CreateDraftStockAdjustment(expect);


            //Test Exception
            try
            {
                //Act
                var result = service.CancelDraftOrPendingStockAdjustment("3");
                Assert.AreEqual(2, result.Status.StatusId);
            }
            catch (Exception e)
            {
                //Assert
                Assert.IsTrue(e.Message.Contains("can't find the StockAdjustment"));
            }

            //No exception part
            try
            {
                //Act
                var result = service.CancelDraftOrPendingStockAdjustment(id);
                Assert.AreEqual(2, result.Status.StatusId);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                //Assert
                Assert.IsTrue(e.Message.Contains("can't find the StockAdjustment"));
            }
        }


        //create new StockAdjustment with status: pending
        [TestMethod()]
        public void CreatePendingStockAdjustmentTest()
        {
            //Arrange
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = "he04";
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            service.CreateDraftStockAdjustment(expect);
            try
            {
                //Act
                var result = service.CreatePendingStockAdjustment(expect);
                //Assert
                Assert.IsTrue(result.Status.StatusId == 4);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find such status"));
            }

        }



        //find all stockadjustemnt
        [TestMethod()]
        public void FindAllStockAdjustmentTest()
        {
            //Arrange
            int expected = stockAdjustmentRepository.Count();
            //Act
            var result = service.FindAllStockAdjustment();
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(StockAdjustment));
        }

        //find stockadjustment by stockjustmentid
        [TestMethod()]
        public void FindStockAdjustmentByIdTest()
        {
            //Arrange
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = "he05";
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            service.CreateDraftStockAdjustment(expect);
            //Act
            var result = service.FindStockAdjustmentById(id);
            //Assert
            Assert.AreEqual(expect, result);
            stockAdjustmentRepository.Delete(expect);
        }

        //approve pending stockadjustment
        [TestMethod()]
        public void ApproveStockAdjustmentTest()
        {
            //Arrange
            Item item = context.Item.Where(x => x.ItemCode == "he06").First();


            StockAdjustmentDetail sd = new StockAdjustmentDetail();
            sd.Item = item;
            sd.OriginalQuantity = 10;
            sd.AfterQuantity = 20;

            List<StockAdjustmentDetail> list = new List<StockAdjustmentDetail>();
            list.Add(sd);

            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = "he07";
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            expect.StockAdjustmentDetails = list;
            service.CreatePendingStockAdjustment(expect);


            StockMovement sm = new StockMovement();

            try
            {
                //Act
                var result = service.ApproveStockAdjustment(id);
                sm = context.StockMovement.Where(x => x.Item.ItemCode == "he06").First();

                //Assert
                int latest_id = stockMovementRepository.Count();
                sm = stockMovementRepository.FindById(latest_id);


                Assert.IsTrue(expect.Status.StatusId == 6);
                Assert.IsTrue(item.Inventory.Quantity == 20);
                Assert.IsTrue(sm.AfterQuantity == 20);
                stockMovementRepository.Delete(sm);
                stockAdjustmentRepository.Delete(expect);
                itemRepository.Delete(item);


            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find StockAdjustment"));

            }
        }

        //reject pending stockadjustment
        [TestMethod()]
        public void RejectStockAdjustmentTest()
        {
            //Arrange
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = "he08";
            expect.StockAdjustmentId = id;
            expect.CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com");
            expect.CreatedDateTime = DateTime.Now;
            service.CreatePendingStockAdjustment(expect);
            //test exception
            try
            {
                //Act
                var result = service.RejectStockAdjustment(".");
                //Assert
                Assert.IsTrue(result.Status.StatusId == 6);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find StockAdjustment"));
            }


            //No exception part

            //Act
            var result1 = service.RejectStockAdjustment(id);
            //Assert
            Assert.IsTrue(result1.Status.StatusId == 5);
            stockAdjustmentRepository.Delete(expect);


        }

        // show sepcific StockAdjustmentDetail in the StockAdjustment
        [TestMethod()]
        public void ShowStockAdjustmentDetailTest()
        {

            //Arrange 
            Random rd = new Random();
            int i = rd.Next();
            string id = "he09";
            StockAdjustment expect = new StockAdjustment();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            StockAdjustmentDetail s1 = new StockAdjustmentDetail();
            s1.StockAdjustmentId = id;
            s1.ItemCode = "C003";
            s1.OriginalQuantity = 10;
            s1.AfterQuantity = 20;
            StockAdjustmentDetail s2 = new StockAdjustmentDetail();
            s2.StockAdjustmentId = id;
            s2.ItemCode = "C002";
            s2.OriginalQuantity = 20;
            s2.AfterQuantity = 30;
            List<StockAdjustmentDetail> list = new List<StockAdjustmentDetail>();
            list.Add(s1);
            list.Add(s2);
            expect.StockAdjustmentDetails = list;
            service.CreateDraftStockAdjustment(expect);
            //test exception
            try
            {
                //Act
                var result = service.ShowStockAdjustmentDetail("123", s1.ItemCode);
                //Assert
                Assert.IsTrue(result.ItemCode == s1.ItemCode);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find stockAdjustmentDetail"));
            }
            //No exception part
            try
            {
                //Act
                var result = service.ShowStockAdjustmentDetail(id, s1.ItemCode);
                //Assert
                Assert.IsTrue(result.ItemCode == s1.ItemCode);
                stockAdjustmentRepository.Delete(expect);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find stockAdjustmentDetail"));
            }
        }

        [TestMethod]
        public void ApproveStockAdjustmentMobile_Valid()
        {
            // Arrange
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);
            var stockAdjustmentService = new StockAdjustmentService(context);

            var stockAdjustment = stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJAPPROVETEST",
                CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com"),
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(4),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJAPPROVETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            stockAdjustmentService.ApproveStockAdjustment("ADJAPPROVETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(6, stockAdjustment.Status.StatusId);
            Assert.AreEqual(10, new StockMovementRepository(context).FindByStockAdjustmentId("ADJAPPROVETEST").FirstOrDefault().AfterQuantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ApproveStockAdjustmentMobile_DoesNotExist_ThrowsException()
        {
            // Act
            new StockAdjustmentService(context).ApproveStockAdjustment("ASDFASDFASDF", "StoreClerk1@email.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ApproveStockAdjustmentMobile_AlreadyApproved_ThrowsException()
        {
            // Arrange
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);
            var stockAdjustmentService = new StockAdjustmentService(context);

            var stockAdjustment = stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJAPPROVETEST",
                CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com"),
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(6),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJAPPROVETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            stockAdjustmentService.ApproveStockAdjustment("ADJAPPROVETEST", "StoreClerk1@email.com");
        }

        [TestMethod]
        public void RejectStockAdjustmentMobile_Valid()
        {
            // Arrange
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);
            var stockAdjustmentService = new StockAdjustmentService(context);

            var stockAdjustment = stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJAPPROVETEST",
                CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com"),
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(4),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJAPPROVETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            stockAdjustmentService.RejectStockAdjustment("ADJAPPROVETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(5, stockAdjustment.Status.StatusId);
            Assert.IsTrue(new StockMovementRepository(context).FindByStockAdjustmentId("ADJAPPROVETEST").FirstOrDefault() == null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RejectStockAdjustmentMobile_DoesNotExist_ThrowsException()
        {
            // Act
            new StockAdjustmentService(context).RejectStockAdjustment("ASDFASDFASDF", "StoreClerk1@email.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RejectStockAdjustmentMobile_AlreadyApproved_ThrowsException()
        {
            // Arrange
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);
            var stockAdjustmentService = new StockAdjustmentService(context);

            var stockAdjustment = stockAdjustmentRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJAPPROVETEST",
                CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com"),
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(6),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJAPPROVETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            stockAdjustmentService.RejectStockAdjustment("ADJAPPROVETEST", "StoreClerk1@email.com");
        }

        [TestCleanup]
        public void CleanAllObjectCreated()
        {
            string[] ids = new string[]
            { "he01","he02","he03","he04","he05","he07","he08","he09" };

            foreach (string id in ids)
            {
                StockAdjustment sa = stockAdjustmentRepository.FindById(id);
                if (sa != null)
                    stockAdjustmentRepository.Delete(sa);
            }

            if (itemRepository.FindById("he06") != null)
            {
                itemRepository.Delete(itemRepository.FindById("he06"));
            }

            if (inventoryRepository.FindById("he06") != null)
            {
                inventoryRepository.Delete(inventoryRepository.FindById("he06"));
            }

            if (stockAdjustmentRepository.ExistsById("ADJAPPROVETEST"))
                stockAdjustmentRepository.Delete(stockAdjustmentRepository.FindById("ADJAPPROVETEST"));
        }
    }
}
