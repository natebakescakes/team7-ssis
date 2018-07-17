using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{

    [TestClass()]
    class StockAdjustmentServiceTest
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
            stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
        }

        [TestMethod()]
        //create new StockAdjustment with status: draft
        public void CreateDraftStockAdjustmentTest()
        {
            // Arrange
            StockAdjustmentService service = new StockAdjustmentService(context);
            StockAdjustment expect = new StockAdjustment();
            expect.Status.StatusId = 3;

            // Act
            StockAdjustment stockadjustment = new StockAdjustment();
            service.CreateDraftStockAdjustment(stockadjustment);
            StockAdjustment result = stockAdjustmentRepository.FindAll().Last();

            // Assert
            Assert.Equals(expect, result);


        }

        [TestMethod()]
        //Delete one item if StockAdjustment in Draft Status
        public void DeleteItemFromDraftStockAdjustmentTest()
        {
           

        }

        [TestMethod()]
        //Delete whole StockAdjustment in Draft Status
        public void DeleteDraftStockAdjustmentTest()
        {
            

        }
        [TestMethod()]

        //create new StockAdjustment with status: pending
        public void CreatePendingStockAdjustmentTest()
        {
           

        }

        [TestMethod()]
        //cancel pening stockadjustment before being approved/rejected
        public void CancelPendingStockAdjustmentTest()
        {
          

        }



        [TestMethod()]
        //find all stockadjustemnt
        public void FindAllStockAdjustmentTest()
        {
            
        }

        [TestMethod()]
        //find stockadjustment by stockjustmentid
        public void FindStockAdjustmentByIdTest()
        {
          

        }

        [TestMethod()]
        //approve pending stockadjustment
        public void ApproveStockAdjustmentTest()
        {
           

        }

        [TestMethod()]
        //reject pending stockadjustment
        public void RejectStockAdjustmentTest()
        {
            
        }

        [TestMethod()]
        // show sepcific StockAdjustmentDetail in the StockAdjustment
        public void ShowStockAdjustmentDetailTest()
        {
           
        }

    }
}
