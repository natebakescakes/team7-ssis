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
using team7_ssis.Services;
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
                Context = this.context
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

        [TestMethod]
        public void GetStockAdjustmentsForSupervisor_ContainsResult()
        {
            // Arrange
            var expectedId = IdService.GetNewStockAdjustmentId(context);
            saRepository.Save(new StockAdjustment()
            {
                StockAdjustmentId = expectedId,
                CreatedBy = new UserService(context).FindUserByEmail("StoreClerk1@email.com"),
                CreatedDateTime = DateTime.Now,
                Remarks = "THIS IS A TEST",
                Status = new StatusService(context).FindStatusByStatusId(4),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = expectedId,
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 20,
                        AfterQuantity = 30,
                    }
                }
            });
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.GetStockAdjustmentsForSupervisor(new EmailViewModel()
            {
                Email = "StoreSupervisor@email.com",
            });

            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<StockAdjustmentRequestViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(s => s.StockAdjustmentId).Contains(expectedId));
            Assert.IsTrue(contentResult.Content.Select(s => s.StockAdjustmentRequestDetails.Select(sd => sd.OriginalQuantity)).FirstOrDefault().Contains(20.ToString()));
        }

        [TestMethod]
        public void ApproveStockAdjustment_DoesNotExist_BadRequest()
        {
            // Arrange
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.ApproveStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Stock Adjustment does not exist", badRequestResult.Message);
        }

        [TestMethod]
        public void ApproveStockAdjustment_AlreadyApproved_BadRequest()
        {
            // Arrange
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var stockAdjustment = new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(6),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJCONTROLTEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            IHttpActionResult actionResult = controller.ApproveStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Stock Adjustment already approved/rejected", badRequestResult.Message);
        }

        [TestMethod]
        public void ApproveStockAdjustment_Valid()
        {
            // Arrange
            var expected = new StatusService(context).FindStatusByStatusId(6);
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var stockAdjustment = new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(4),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJCONTROLTEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            IHttpActionResult actionResult = controller.ApproveStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Successfully approved");
            var result = new StockAdjustmentRepository(context).FindById("ADJCONTROLTEST");
            Assert.AreEqual(expected.Name, result.Status.Name);
        }

        [TestMethod]
        public void RejectStockAdjustment_DoesNotExist_BadRequest()
        {
            // Arrange
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.RejectStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Stock Adjustment does not exist", badRequestResult.Message);
        }

        [TestMethod]
        public void RejectStockAdjustment_AlreadyApproved_BadRequest()
        {
            // Arrange
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var stockAdjustment = new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(6),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJCONTROLTEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            IHttpActionResult actionResult = controller.RejectStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Stock Adjustment already approved/rejected", badRequestResult.Message);
        }

        [TestMethod]
        public void RejectStockAdjustment_Valid()
        {
            // Arrange
            var expected = new StatusService(context).FindStatusByStatusId(5);
            var controller = new StockAdjustmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var stockAdjustment = new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(4),
                StockAdjustmentDetails = new List<StockAdjustmentDetail>()
                {
                    new StockAdjustmentDetail()
                    {
                        StockAdjustmentId = "ADJCONTROLTEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        OriginalQuantity = 10,
                        AfterQuantity = 10,
                    }
                }
            });

            // Act
            IHttpActionResult actionResult = controller.RejectStockAdjustment(new StockAdjustmentIdViewModel()
            {
                StockAdjustmentId = "ADJCONTROLTEST",
                Email = "StoreSupervisor@email.com",
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Successfully rejected");
            var result = new StockAdjustmentRepository(context).FindById("ADJCONTROLTEST");
            Assert.AreEqual(expected.Name, result.Status.Name);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);

            if (saService.FindAllStockAdjustment().Where(x => x.Remarks == "THIS IS A TEST").FirstOrDefault() != null)
                saRepository.Delete(saService.FindAllStockAdjustment().Where(x => x.Remarks == "THIS IS A TEST").FirstOrDefault());

            if (stockAdjustmentRepository.ExistsById("ADJCONTROLTEST"))
                stockAdjustmentRepository.Delete(stockAdjustmentRepository.FindById("ADJCONTROLTEST"));
        }
    }
}
