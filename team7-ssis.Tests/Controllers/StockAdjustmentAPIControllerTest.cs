using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class StockAdjustmentAPIControllerTest
    {
        private ApplicationDbContext context;
        StockAdjustmentService saService;
        StockAdjustmentRepository saRepository;
        StockAdjustmentRepository sadRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            saService = new StockAdjustmentService(context);
            saRepository = new StockAdjustmentRepository(context);
            sadRepository = new StockAdjustmentRepository(context);

        }

        [TestMethod]
        public void SaveFromMobileTest()
        {
            //Arrange
            //Instantiate controller
         
            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "root@admin.com",
                context = this.context
            };
            controller.ModelState.Clear();

            Item item = context.Item.First();

            List<MobileSADViewModel> list = new List<MobileSADViewModel>();

            MobileSADViewModel VM = new MobileSADViewModel() {
                ItemCode = item.ItemCode,
                UserName = "StoreClerk1@email.com",
                QuantityAdjusted = 5,
                Reason = "API Controller TEST"

            };
            list.Add(VM);
            //Act
            IHttpActionResult actionResult = controller.Save(list);
            var contentResult = actionResult as OkNegotiatedContentResult<String>;

            //Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(contentResult.Content);

            //Delete
            saRepository.Delete(saRepository.FindById(contentResult.Content));
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            
        }
    }
}
