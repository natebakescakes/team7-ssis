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
        StockAdjustmentDetailRepository sadRepository;
        StatusRepository statusRepository;
        ItemRepository itemRepository;
        StockMovementRepository smRepository;
        UserService userService;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            saService = new StockAdjustmentService(context);
            saRepository = new StockAdjustmentRepository(context);
            sadRepository = new StockAdjustmentDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemRepository = new ItemRepository(context);
            smRepository = new StockMovementRepository(context);
            userService = new UserService(context);
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
                OriginalQuantity = "5",
                AfterQuantity = "6",

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
        public void GetAllStockadjustment_ContainResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new StockAdjustmentAPIController();

            StockAdjustment sa1 = new StockAdjustment();
            sa1.StockAdjustmentId = "he01";
            sa1.Remarks = "THIS IS A TEST";
            sa1.CreatedDateTime = DateTime.Now;
            sa1.Status = statusRepository.FindById(3);

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

            //Act
            StockAdjustmentViewModel result = controller.GetAllStockAdjustments().OrderByDescending
                (x => x.CreatedDateTime).First();

            //Assert
            Assert.AreEqual(sa1.StockAdjustmentId, result.StockAdjustmentId);
            saRepository.Delete(saRepository.FindById("he01"));

        }


        [TestMethod]
        public void GetAllStockadjustmentExceptDraft_ContainResult()
        {
            //Arrange
            //Instantiate controller
            var controller = new StockAdjustmentAPIController();

            StockAdjustment sa1 = new StockAdjustment();
            sa1.StockAdjustmentId = "he02";
            sa1.Remarks = "THIS IS A TEST";
            sa1.CreatedDateTime = DateTime.Now;
            sa1.Status = statusRepository.FindById(3);

            StockAdjustmentDetail s1 = new StockAdjustmentDetail();
            s1.StockAdjustmentId = "he02";
            s1.ItemCode = "C001";
            s1.OriginalQuantity = 10;
            s1.AfterQuantity = 20;
            StockAdjustmentDetail s2 = new StockAdjustmentDetail();
            s2.StockAdjustmentId = "he02";
            s2.ItemCode = "C002";
            s2.OriginalQuantity = 20;
            s2.AfterQuantity = 30;
            List<StockAdjustmentDetail> list = new List<StockAdjustmentDetail>();
            list.Add(s1);
            list.Add(s2);
            sa1.StockAdjustmentDetails = list;
            saRepository.Save(sa1);

            //Act
            StockAdjustmentViewModel result = controller.GetAllStockAdjustments().OrderByDescending
                (x => x.CreatedDateTime).First();

            //Assert
            Assert.AreEqual(sa1.StockAdjustmentId, result.StockAdjustmentId);
            saRepository.Delete(saRepository.FindById("he02"));
        }

        [TestMethod]
        public void SaveStockAdjustmentAsDraft_Test()
        {
            //Arrange
            List<ViewModelFromNew> list = new List<ViewModelFromNew>();
            ViewModelFromNew v1 = new ViewModelFromNew();
            v1.Adjustment = 1;
            v1.Itemcode = "C001";
            v1.Reason = "Test1";
            list.Add(v1);
            var controller = new StockAdjustmentAPIController();

            //Act
            controller.SaveStockAdjustmentAsDraft(list);
            StockAdjustment sa = context.StockAdjustment.OrderByDescending(x => x.StockAdjustmentId).First();
            sa.Remarks = "THIS IS A TEST";
            saService.updateStockAdjustment(sa);

            StockAdjustmentDetail sad = context.StockAdjustmentDetail.OrderByDescending(x => x.StockAdjustmentId).First();

            //Assert
            Assert.IsTrue(sad.ItemCode == "C001");
            Assert.IsTrue(sa.Status.StatusId == 3);
            saRepository.Delete(sa);

        }

        [TestMethod]
        public void CreatePendingStockAdjustmentAsDraft_Test()
        {
            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };
            //Arrange
            List<ViewModelFromNew> list = new List<ViewModelFromNew>();
            ViewModelFromNew v1 = new ViewModelFromNew();
            v1.Adjustment = 1;
            v1.Itemcode = "C001";
            v1.Reason = "Test1";
            v1.Unitprice = "1.0";
            list.Add(v1);
          

            //Act
            controller.CreatePendingStockAdjustment(list);
            StockAdjustment sa = context.StockAdjustment.OrderByDescending(x => x.StockAdjustmentId).First();
            sa.Remarks = "THIS IS A TEST";
            saService.updateStockAdjustment(sa);

            StockAdjustmentDetail sad = context.StockAdjustmentDetail.OrderByDescending(x => x.StockAdjustmentId).First();

            //Assert
            Assert.IsTrue(sad.ItemCode == "C001");
            Assert.IsTrue(sa.Status.StatusId == 4);
            saRepository.Delete(sa);

        }

        [TestMethod]
        public void RejectStockAdjustment_Test()
        {
            StockAdjustment sa = new StockAdjustment();
            sa.StockAdjustmentId = "test1";
            sa.Remarks = "THIS IS A TEST";
            sa.CreatedDateTime = DateTime.Now;
            sa.Status = statusRepository.FindById(4);

            StockAdjustmentDetail sad = new StockAdjustmentDetail();
            sad.StockAdjustmentId = "test1";
            sad.Reason = "test1";
            sad.ItemCode = "C001";

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            detaillist.Add(sad);

            sa.StockAdjustmentDetails = detaillist;
            saRepository.Save(sa);
            //sadRepository.Save(sad);

            List<ViewModelFromEditDetail> list = new List<ViewModelFromEditDetail>();
            ViewModelFromEditDetail v1 = new ViewModelFromEditDetail();
            v1.StockAdjustmentID = "test1";
            v1.Reason = "test1";
            v1.Itemcode = "C001";
            list.Add(v1);


            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };

            controller.RejectStockAdjustment(list);


            Assert.AreEqual(saRepository.FindById("test1").Status.StatusId, 5);
            saRepository.Delete(saRepository.FindById("test1"));
        }

        [TestMethod]
        public void ApproveStockAdjustment_Test()
        {

            StockAdjustment sa = new StockAdjustment();
            sa.StockAdjustmentId = "test1";
            sa.Remarks = "THIS IS A TEST";
            sa.CreatedDateTime = DateTime.Now;
            sa.Status = statusRepository.FindById(4);

            StockAdjustmentDetail sad = new StockAdjustmentDetail();
            sad.StockAdjustmentId = "test1";
            sad.Reason = "test1";
            sad.ItemCode = "C001";
            sad.Item = itemRepository.FindById("C001");
            sad.OriginalQuantity = 0;
            sad.AfterQuantity = 10;

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            detaillist.Add(sad);

            sa.StockAdjustmentDetails = detaillist;
            saRepository.Save(sa);
            //sadRepository.Save(sad);

            List<ViewModelFromEditDetail> list = new List<ViewModelFromEditDetail>();
            ViewModelFromEditDetail v1 = new ViewModelFromEditDetail();
            v1.StockAdjustmentID = "test1";
            v1.Reason = "test1";
            v1.Itemcode = "C001";
            list.Add(v1);


            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };

            controller.ApproveStockAdjustment(list);


            Assert.AreEqual(saRepository.FindById("test1").Status.StatusId, 6);
            saRepository.Delete(saRepository.FindById("test1"));
            StockMovement sv = context.StockMovement.OrderByDescending(x => x.StockMovementId).First();
            smRepository.Delete(sv);

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
            Assert.IsTrue(contentResult.Content
                .SelectMany(s => s.StockAdjustmentRequestDetails.Where(x => x.ItemCode == "E030").Select(sd => sd.OriginalQuantity))
                .FirstOrDefault().Contains(20.ToString()));
            
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

        [TestMethod]
        public void UpdateStockAdjustmentAsDraft_Test()
        {
            StockAdjustment sa = new StockAdjustment();
            sa.StockAdjustmentId = "test1";
            sa.Remarks = "THIS IS A TEST";
            sa.CreatedDateTime = DateTime.Now;
            sa.Status = statusRepository.FindById(3);

            StockAdjustmentDetail sad = new StockAdjustmentDetail();
            sad.StockAdjustmentId = "test1";
            sad.Reason = "test1";
            sad.ItemCode = "C001";

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            detaillist.Add(sad);

            sa.StockAdjustmentDetails = detaillist;
            saRepository.Save(sa);

            List<ViewModelFromEditDetail> list = new List<ViewModelFromEditDetail>();
            ViewModelFromEditDetail v1 = new ViewModelFromEditDetail();
            v1.StockAdjustmentID = "test1";
            v1.Reason = "test2";
            v1.Itemcode = "C001";
            v1.Adjustment = 10;
            list.Add(v1);

            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };

            controller.UpdateStockAdjustmentAsDraft(list);

            Assert.AreEqual(sadRepository.FindById("test1","C001").Reason,"test2");
          
            saRepository.Delete(saRepository.FindById("test1"));



        }


        [TestMethod]
        public void UpdateStockAdjustmentAsPending_Test()
        {
            StockAdjustment sa = new StockAdjustment();
            sa.StockAdjustmentId = "test1";
            sa.Remarks = "THIS IS A TEST";
            sa.CreatedDateTime = DateTime.Now;
            sa.Status = statusRepository.FindById(4);

            StockAdjustmentDetail sad = new StockAdjustmentDetail();
            sad.StockAdjustmentId = "test1";
            sad.Reason = "test1";
            sad.ItemCode = "C001";

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            detaillist.Add(sad);

            sa.StockAdjustmentDetails = detaillist;
            saRepository.Save(sa);

            List<ViewModelFromEditDetail> list = new List<ViewModelFromEditDetail>();
            ViewModelFromEditDetail v1 = new ViewModelFromEditDetail();
            v1.StockAdjustmentID = "test1";
            v1.Reason = "test2";
            v1.Itemcode = "C001";
            v1.Adjustment = 10;
            v1.Unitprice = "1.0";
            list.Add(v1);

            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };

            controller.UpdateStockAdjustmentAsDraft(list);

            Assert.AreEqual(sadRepository.FindById("test1", "C001").Reason, "test2");

            saRepository.Delete(saRepository.FindById("test1"));

        }

        [TestMethod]
        public void GetStockAdjustmentDetail_Test()
        {
            StockAdjustment sa = new StockAdjustment();
            sa.StockAdjustmentId = "test1";
            sa.CreatedDateTime = DateTime.Now;
            sa.Remarks = "THIS IS A TEST";
            sa.Status = statusRepository.FindById(4);

            StockAdjustmentDetail sad = new StockAdjustmentDetail();
            sad.StockAdjustmentId = "test1";
            sad.Reason = "test1";
            sad.ItemCode = "C001";

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            detaillist.Add(sad);

            sa.StockAdjustmentDetails = detaillist;
            saRepository.Save(sa);


            StockAdjustmentAPIController controller = new StockAdjustmentAPIController()
            {
                CurrentUserName = "StoreClerk1@email.com",
                Context = this.context
            };

            StockAdjustmentDetailViewModel result = controller.GetStockAdjustmentDetail("test1").First();

            Assert.AreEqual(result.Reason, "test1");
            saRepository.Delete(saRepository.FindById("test1"));


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
