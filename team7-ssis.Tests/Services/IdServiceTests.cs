using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class IdServiceTests
    {
        ApplicationDbContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
        }

        [TestMethod]
        public void GetNewCollectionPointIdTest()
        {
            // Arrange
            int expected = context.CollectionPoint.OrderByDescending(x => x.CollectionPointId).FirstOrDefault().CollectionPointId + 1;
            
            // Act
            var result = IdService.GetNewCollectionPointId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNewDelegationIdTest()
        {
            // Act
            var result = IdService.GetNewDelegationId(context);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetNewDeliveryOrderNoTest()
        {
            // Arrange
            string expectedPrefix = $"DO-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewDeliveryOrderNo(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 9));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewDeliveryOrderNo_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"DO-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewDeliveryOrderNo(context);
            new DeliveryOrderRepository(context).Save(new DeliveryOrder()
            {
                DeliveryOrderNo = previous,
                InvoiceFileName = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewDeliveryOrderNo(context);
            new DeliveryOrderRepository(context).Save(new DeliveryOrder()
            {   
                DeliveryOrderNo = current,
                InvoiceFileName = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod()]
        public void GetNewDisbursementIdTest()
        {
            // Arrange
            string expectedPrefix = $"DSB-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewDisbursementId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewDisbursementId_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"DSB-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewDisbursementId(context);
            new DisbursementRepository(context).Save(new Disbursement()
            {
                DisbursementId = previous,
                Remarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewDisbursementId(context);
            new DisbursementRepository(context).Save(new Disbursement()
            {
                DisbursementId = current,
                Remarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewItemCategoryIdTest()
        {
            // Arrange
            int expected = context.ItemCategory.OrderByDescending(x => x.ItemCategoryId)
                .FirstOrDefault().ItemCategoryId + 1;

            // Act
            var result = IdService.GetNewItemCategoryId(context);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void GetNewNotificationIdTest()
        {
            // Act
            var result = IdService.GetNewNotificationId(context);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetNewPurchaseOrderNoTest()
        {
            // Arrange
            string expectedPrefix = $"PO-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewPurchaseOrderNo(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 9));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        [Ignore]
        public void GetNewPurchaseOrderNo_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"PO-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewPurchaseOrderNo(context);
            new PurchaseOrderRepository(context).Save(new PurchaseOrder()
            {
                PurchaseOrderNo = previous,
                Status = new StatusService(context).FindStatusByStatusId(16),
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewPurchaseOrderNo(context);
            new PurchaseOrderRepository(context).Save(new PurchaseOrder()
            {
                PurchaseOrderNo = current,
                Status = new StatusService(context).FindStatusByStatusId(16),
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewRequisitionIdTest()
        {
            // Arrange
            string expectedPrefix = $"REQ-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewRequisitionId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewRequisitionId_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"REQ-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewRequisitionId(context);
            new RequisitionRepository(context).Save(new Requisition()
            {
                RequisitionId = previous,
                EmployeeRemarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewRequisitionId(context);
            new RequisitionRepository(context).Save(new Requisition()
            {
                RequisitionId = current,
                EmployeeRemarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewAutoGenerateRequisitionIdTest()
        {
            // Arrange
            string expectedPrefix = $"SRQ-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewAutoGenerateRequisitionId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewAutoGenerateRequisitionId_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"SRQ-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewAutoGenerateRequisitionId(context);
            new RequisitionRepository(context).Save(new Requisition()
            {
                RequisitionId = previous,
                EmployeeRemarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewAutoGenerateRequisitionId(context);
            new RequisitionRepository(context).Save(new Requisition()
            {
                RequisitionId = current,
                EmployeeRemarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewRetrievalIdTest()
        {
            // Arrange
            string expectedPrefix = $"RET-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewRetrievalId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewRetrievalId_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"RET-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewRetrievalId(context);
            new RetrievalRepository(context).Save(new Retrieval()
            {
                RetrievalId = previous,
                Status = new StatusService(context).FindStatusByStatusId(16),
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewRetrievalId(context);
            new RetrievalRepository(context).Save(new Retrieval()
            {
                RetrievalId = current,
                Status = new StatusService(context).FindStatusByStatusId(16),
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewStockAdjustmentIdTest()
        {
            // Arrange
            string expectedPrefix = $"ADJ-{DateTime.Now.Year}{DateTime.Now.Month:00}";

            // Act
            var result = IdService.GetNewStockAdjustmentId(context);
            var serialNoParseResult = Int32.TryParse(result.Substring(result.Length - 3), out int serialNo);

            // Assert
            Assert.AreEqual(expectedPrefix, result.Substring(0, 10));
            Assert.IsTrue(serialNoParseResult);
        }

        [TestMethod]
        public void GetNewStockAdjustmentId_ExistingId_Valid()
        {
            // Arrange
            string expectedPrefix = $"ADJ-{DateTime.Now.Year}{DateTime.Now.Month:00}";
            var previous = IdService.GetNewStockAdjustmentId(context);
            new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = previous,
                Remarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now.AddDays(1 - DateTime.Today.Day),
            });

            // Act
            var current = IdService.GetNewStockAdjustmentId(context);
            new StockAdjustmentRepository(context).Save(new StockAdjustment()
            {
                StockAdjustmentId = current,
                Remarks = "IDSERVICETEST",
                CreatedDateTime = DateTime.Now,
            });
            var previousSerialNoParseResult = Int32.TryParse(previous.Substring(previous.Length - 3), out int previousSerialNo);
            var resultSerialNoParseResult = Int32.TryParse(current.Substring(current.Length - 3), out int resultSerialNo);

            // Assert
            Assert.AreEqual(1, resultSerialNo - previousSerialNo);
        }

        [TestMethod]
        public void GetNewStockMovementIdTest()
        {
            // Act
            var result = IdService.GetNewStockMovementId(context);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Delete DeliveryOrders
            var deliveryOrderRepository = new DeliveryOrderRepository(context);
            if (deliveryOrderRepository.FindAll().Where(x => x.InvoiceFileName == "IDSERVICETEST").Count() > 0)
                deliveryOrderRepository.FindAll().Where(x => x.InvoiceFileName == "IDSERVICETEST").ToList().ForEach(x => deliveryOrderRepository.Delete(x));

            // Delete Disbursements
            var disbursementRepository = new DisbursementRepository(context);
            if (disbursementRepository.FindAll().Where(x => x.Remarks == "IDSERVICETEST").Count() > 0)
                disbursementRepository.FindAll().Where(x => x.Remarks == "IDSERVICETEST").ToList().ForEach(x => disbursementRepository.Delete(x));

            // Delete Purchase Orders
            var purchaseOrderRepository = new PurchaseOrderRepository(context);
            if (purchaseOrderRepository.FindAll().Where(x => x.Status.StatusId == 16).Count() > 0)
                purchaseOrderRepository.FindAll().Where(x => x.Status.StatusId == 16).ToList().ForEach(x => purchaseOrderRepository.Delete(x));

            // Delete Requisitions
            var requisitionRepository = new RequisitionRepository(context);
            if (requisitionRepository.FindAll().Where(x => x.EmployeeRemarks == "IDSERVICETEST").Count() > 0)
                requisitionRepository.FindAll().Where(x => x.EmployeeRemarks == "IDSERVICETEST").ToList().ForEach(x => requisitionRepository.Delete(x));

            // Delete Retrievals
            var retrievalRepository = new RetrievalRepository(context);
            if (retrievalRepository.FindAll().Where(x => x.Status.StatusId == 16).Count() > 0)
                retrievalRepository.FindAll().Where(x => x.Status.StatusId == 16).ToList().ForEach(x => retrievalRepository.Delete(x));

            // Delete StockAdjustments
            var stockAdjustmentRepository = new StockAdjustmentRepository(context);
            if (stockAdjustmentRepository.FindAll().Where(x => x.Remarks == "IDSERVICETEST").Count() > 0)
                stockAdjustmentRepository.FindAll().Where(x => x.Remarks == "IDSERVICETEST").ToList().ForEach(x => stockAdjustmentRepository.Delete(x));
        }
    }
}
