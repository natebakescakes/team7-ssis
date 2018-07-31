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
    public class NotificationServiceTests
    {
        ApplicationDbContext context;
        NotificationService notificationService;

        NotificationRepository notificationRepository;
        DisbursementRepository disbursementRepository;
        StockAdjustmentRepository saRepository;
        RequisitionRepository requisitionRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            notificationRepository = new NotificationRepository(context);
            notificationService = new NotificationService(context);

            disbursementRepository = new DisbursementRepository(context);
            saRepository = new StockAdjustmentRepository(context);
            requisitionRepository = new RequisitionRepository(context);

            //save new disbursement object into db
            Disbursement disbursement = new Disbursement();
            if (disbursementRepository.FindById("TEST") == null)
            {
                disbursement.DisbursementId = "TEST";
                disbursement.CreatedDateTime = DateTime.Now;

            }
            else disbursement = disbursementRepository.FindById("TEST");
            disbursementRepository.Save(disbursement);

            //save new requisition object into db
            Requisition requisition = new Requisition();
            if (requisitionRepository.FindById("TEST") == null)
            {
                requisition.RequisitionId = "TEST";
                requisition.CreatedDateTime = DateTime.Now;
            }
            else requisition = requisitionRepository.FindById("TEST");
            requisitionRepository.Save(requisition);

            //create new SA object and save into db
            StockAdjustment SA = new StockAdjustment();
            if (saRepository.FindById("TEST") == null)
            {
                SA.StockAdjustmentId = "TEST";
                SA.CreatedDateTime = DateTime.Now;

            }
            else SA = saRepository.FindById("TEST");
            saRepository.Save(SA);

        }

        [TestMethod]
        public void CreateDisbursementNotificationTest()
        {
            //Arrange
            Disbursement disbursement = context.Disbursement.Where(x=>x.DisbursementId=="TEST").First();
            ApplicationUser user = context.Users.Where(x=>x.FirstName=="admin").First();

            int expected = 1;

            //Act
            var result = notificationService.CreateNotification(disbursement, user);


            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void CreateRequisitionNotificationTest()
        {
            //Arrange
            Requisition requisition = context.Requisition.Where(x=>x.RequisitionId=="TEST").First();
            ApplicationUser user = context.Users.Where(x => x.FirstName == "admin").First();
            int expected = 2;
            //Act
            var result = notificationService.CreateNotification(requisition, user);


            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void CreateStockAdjustmentNotificationTest()
        {
            //Arrange
            StockAdjustment stockAdjustment = context.StockAdjustment.Where(x=>x.StockAdjustmentId=="TEST").First();
            ApplicationUser user = context.Users.Where(x => x.FirstName == "admin").First();
            int expected = 3;

            //Act
            var result = notificationService.CreateNotification(stockAdjustment, user);

            //Assert
            Assert.AreEqual(expected, result.NotificationType.NotificationTypeId);
        }

        [TestMethod]
        public void FindNotificationsByUserTest()
        {
            //Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            var notification = notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now,
                CreatedFor = new UserService(context).FindUserByEmail("root@admin.com"),
            });
            ApplicationUser user = notification.CreatedFor;

            //Act
            var result = notificationService.FindNotificationsByUser(user);

            //Assert
            result.ForEach(n => Assert.AreEqual(user, n.CreatedFor));
        }

        [TestMethod]
        public void FindNotificationsByTypeTest()
        {
            //Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            var notification = notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                NotificationType = new NotificationTypeRepository(context).FindById(2),
                CreatedDateTime = DateTime.Now
            });

            NotificationType type = notification.NotificationType;

            //Act
            var result = notificationService.FindNotificationByType(type);

            //Assert
            result.ForEach(n => Assert.AreEqual(type, n.NotificationType));
        }

        [TestMethod]
        public void FindUserByDepartment_NullInput()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationsByUser(null);

            // Assert
            Assert.IsTrue(result.Count == 0);
        }

        [TestMethod()]
        public void FindNotificationById_Exists()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationById(notificationId);

            // Assert
            Assert.AreEqual(notificationId, result.NotificationId);
        }

        [TestMethod]
        public void FindNotificationById_DoesNotExist_ReturnNull()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.FindNotificationById(0);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void ReadNotification_Valid()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            var expected = 15; // Read status
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                Status = new StatusService(context).FindStatusByStatusId(14),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.ReadNotification(notificationId);

            // Assert
            Assert.AreEqual(expected, result.Status.StatusId);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void ReadNotification_AlreadyRead()
        {
            // Arrange
            var notificationId = IdService.GetNewNotificationId(context);
            notificationService.Save(new Notification()
            {
                NotificationId = notificationId,
                Status = new StatusService(context).FindStatusByStatusId(15),
                CreatedDateTime = DateTime.Now
            });

            // Act
            var result = notificationService.ReadNotification(notificationId);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var usedNotificationId = IdService.GetNewNotificationId(context) - 1;
            if (notificationRepository.ExistsById(usedNotificationId))
                notificationRepository.Delete(notificationRepository.FindById(usedNotificationId));


            //delete test object from Disbursements
            List<Disbursement> ddlist = context.Disbursement.Where(x => x.DisbursementId == "TEST").ToList();
            foreach (Disbursement dd in ddlist)
            {
                disbursementRepository.Delete(dd);
            }

            //delete test object from Requisitions
            List<Requisition> rlist = context.Requisition.Where(x => x.RequisitionId == "TEST").ToList();
            foreach(Requisition r in rlist)
            {
                requisitionRepository.Delete(r);
            }

            //delete test object from StockAdjustments
            List<StockAdjustment> salist = context.StockAdjustment.Where(x => x.StockAdjustmentId == "TEST").ToList();
            foreach (StockAdjustment SA in salist)
            {
                saRepository.Delete(SA);
            }

        }
    }
}
