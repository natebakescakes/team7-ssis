﻿using System;
using System.Collections.Generic;
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
        StockAdjustmentService service ;
       // ItemService itemService;
        ItemRepository itemRepository;
        InventoryRepository inventoryRepository;
        ItemService itemService;

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
        }

        //create new StockAdjustment with status: draft
        [TestMethod()]
        
        public void CreateDraftStockAdjustmentTest()
        {
            //Arrange 
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = i.ToString();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            // Act     
            try
            {
                var result = service.CreateDraftStockAdjustment(expect);
                Assert.AreEqual(3, result.Status.StatusId);
                stockAdjustmentRepository.Delete(expect);
            }
            catch(Exception e)
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
            string id = i.ToString();
            StockAdjustment expect = new StockAdjustment();
            expect.StockAdjustmentId =id;
            expect.CreatedDateTime = DateTime.Now;
            StockAdjustmentDetail s1 = new StockAdjustmentDetail();
            s1.StockAdjustmentId = id;
            s1.ItemCode = "C003";
            s1.OriginalQuantity = 10;
            s1.AfterQuantity = 20;
            StockAdjustmentDetail s2 = new StockAdjustmentDetail();
            s2.StockAdjustmentId = id;
            s2.ItemCode="C002";
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
            string id = i.ToString();
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
            string id = i.ToString();
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
            catch(Exception e)
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
            string id = i.ToString();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            service.CreateDraftStockAdjustment(expect);
            //Act
            var result=service.FindStockAdjustmentById(id);
            //Assert
            Assert.AreEqual(expect, result);
            stockAdjustmentRepository.Delete(expect);
        }

        //approve pending stockadjustment
        [TestMethod()]
        public void ApproveStockAdjustmentTest()
        {
            //Arrange
            Item item = new Item();
            item.ItemCode = "BBB";
            item.CreatedDateTime = DateTime.Now;
            itemRepository.Save(item);
            itemService.SaveInventory(item, 40);


            StockAdjustmentDetail sd = new StockAdjustmentDetail();
            sd.Item = item;
            sd.OriginalQuantity = 10;
            sd.AfterQuantity = 20;

            List<StockAdjustmentDetail> li = new List<StockAdjustmentDetail>();
            li.Add(sd);

            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = i.ToString();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            expect.StockAdjustmentDetails = li;
            service.CreatePendingStockAdjustment(expect);

                 
            try
            {
                //Act
                var result = service.ApproveStockAdjustment(id);
                //Assert
                Assert.IsTrue(expect.Status.StatusId == 6);
                Assert.IsTrue(item.Inventory.Quantity==20);
                stockAdjustmentRepository.Delete(expect);
               itemRepository.Delete(item);
          
            }
            catch(Exception e)
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
            string id = i.ToString();
            expect.StockAdjustmentId = id;
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
            string id = i.ToString();
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
            catch(Exception e)
            {
                Assert.IsTrue(e.Message.Contains("can't find stockAdjustmentDetail"));
            }
        }
    }
}
