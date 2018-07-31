using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class StockAdjustmentControllerTest
    {

        ApplicationDbContext context;
        StatusRepository statusRepository;
        StockAdjustmentRepository saRepository;
        UserRepository userRepository;



        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            statusRepository = new StatusRepository(context);
            saRepository = new StockAdjustmentRepository(context);
            userRepository = new UserRepository(context);
            //Create a draft stockadjustment
            StockAdjustment sa1 = new StockAdjustment();
            sa1.StockAdjustmentId = "he01";
            sa1.CreatedDateTime = DateTime.Now;
            sa1.Status = statusRepository.FindById(3);
            sa1.CreatedBy = userRepository.FindByEmail("StoreClerk1@emaiil.com");
            sa1.ApprovedBySupervisor = userRepository.FindByEmail("StoreSupervisor@email.com");

            StockAdjustmentDetail s1 = new StockAdjustmentDetail();
            s1.StockAdjustmentId = "he01";
            s1.ItemCode = "C001";
            s1.OriginalQuantity = 10;
            s1.AfterQuantity = 20;
            StockAdjustmentDetail s2 = new StockAdjustmentDetail();
            s2.StockAdjustmentId = "he01";
            s2.ItemCode = "C002";
            s2.OriginalQuantity = 20;
            s2.AfterQuantity = 30;
            List<StockAdjustmentDetail> list = new List<StockAdjustmentDetail>();
            list.Add(s1);
            list.Add(s2);
            sa1.StockAdjustmentDetails = list;
            saRepository.Save(sa1);


        }


        [TestMethod]
        public void Index_Test()
        {
            // Arrange
           StockAdjustmentController controller = new StockAdjustmentController();

            // Act
            ViewResult result = controller.Index("he01") as ViewResult;

            // Assert
            Assert.IsNotNull(result);

        }



        [TestMethod]
        public void New_Test()
        {
            // Arrange
            StockAdjustmentController controller = new StockAdjustmentController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                 context = this.context
            };
            // Act
            ViewResult result = controller.New() as ViewResult;
            StockAdjustmentViewModel viewmodel = (StockAdjustmentViewModel) result.Model;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(viewmodel.supervisors.Contains(userRepository.FindByEmail("StoreSupervisor@email.com")));
            Assert.IsTrue(viewmodel.managers.Contains(userRepository.FindByEmail("StoreManager@email.com")));

        }

        [TestMethod]
        public void Process_Test()
        {
            // Arrange
            StockAdjustmentController controller = new StockAdjustmentController()
            {
                CurrentUserName = "StoreSupervisor@email.com",
                context = this.context
            };
            // Act
            ViewResult result = controller.Process("he01") as ViewResult;
            StockAdjustmentViewModel viewmodel = (StockAdjustmentViewModel)result.Model;

            //Assert

            Assert.AreEqual(viewmodel.ApprovedBySupervisor, "Store Supervisor");

        }


        [TestMethod]
        public void DetailsNoEdit_Test()
        {
            // Arrange
            StockAdjustmentController controller = new StockAdjustmentController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                context = this.context
            };
            // Act
            ViewResult result = controller.DetailsNoEdit("he01") as ViewResult;
            StockAdjustmentViewModel viewmodel = (StockAdjustmentViewModel)result.Model;

            //Assert

            Assert.AreEqual(viewmodel.ApprovedBySupervisor, "Store Supervisor");

        }


        [TestMethod]
        public void DetailsEdit_Test()
        {
            // Arrange
            StockAdjustmentController controller = new StockAdjustmentController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                context = this.context
            };
            // Act
            ViewResult result = controller.DetailsEdit("he01") as ViewResult;
            StockAdjustmentViewModel viewmodel = (StockAdjustmentViewModel)result.Model;

            //Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(viewmodel.supervisors.Contains(userRepository.FindByEmail("StoreSupervisor@email.com")));
            Assert.IsTrue(viewmodel.managers.Contains(userRepository.FindByEmail("StoreManager@email.com")));

        }




        [TestCleanup]
        public void TestClean()
        {
            saRepository.Delete(saRepository.FindById("he01"));
        }


    }
}
