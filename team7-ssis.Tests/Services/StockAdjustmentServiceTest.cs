using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class StockAdjustmentServiceTest
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        StockAdjustmentService service ;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
            stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
            service = new StockAdjustmentService(context);
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
            var result = service.CreateDraftStockAdjustment(expect);
            //Assert
            Assert.AreEqual(3, result.Status.StatusId);
            stockAdjustmentRepository.Delete(expect);

        }

        //Delete one item if StockAdjustment in Draft Status
        [TestMethod()]
        public void DeleteItemFromDraftStockAdjustmentTest()
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
            s1.ItemCode = "C001";
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

            // Act        
           string delete_Item = "C001";
           var result= service.DeleteItemFromDraftStockAdjustment(id, delete_Item);
            //Assert
            Assert.AreEqual(delete_Item, result);
            Assert.AreEqual(stockAdjustmentDetailRepository.FindById(id,result), null);
            Assert.AreEqual(expect.StockAdjustmentDetails.Count, 1);
            stockAdjustmentRepository.Delete(expect);      
        }

        //Delete whole StockAdjustment in Draft Status
        [TestMethod()]
        public void DeleteDraftStockAdjustmentTest()
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
            var result=service.DeleteDraftStockAdjustment(id);
            //Assert
            Assert.AreEqual(id, result);
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
            //Act
            var result=service.CreatePendingStockAdjustment(expect);
            //Assert
            Assert.IsTrue(result.Status.StatusId == 4);
            stockAdjustmentRepository.Delete(expect);

        }

        //cancel pening stockadjustment before being approved/rejected
        [TestMethod()]
        public void CancelPendingStockAdjustmentTest()
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
            service.CancelPendingStockAdjustment(id);
            //Assert
            Assert.IsTrue(expect.Status.StatusId == 2);

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
            StockAdjustment expect = new StockAdjustment();
            Random rd = new Random();
            int i = rd.Next();
            string id = i.ToString();
            expect.StockAdjustmentId = id;
            expect.CreatedDateTime = DateTime.Now;
            service.CreatePendingStockAdjustment(expect);
            //Act
            var result=service.ApproveStockAdjustment(id);
            //Assert
            Assert.IsTrue(expect.Status.StatusId == 6);
            stockAdjustmentRepository.Delete(expect);
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
            //Act
            var result = service.RejectStockAdjustment(id);
            //Assert
            Assert.IsTrue(expect.Status.StatusId == 5);
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
            s1.ItemCode = "C001";
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
            //Act
            var result = service.ShowStockAdjustmentDetail(id, s1.ItemCode);
            //Assert
            Assert.IsTrue(result.ItemCode == s1.ItemCode);
            stockAdjustmentRepository.Delete(expect);
        }
    }
}
