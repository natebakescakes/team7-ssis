using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class RequisitionDetailRepositoryTests
    {
        ApplicationDbContext context;
        RequisitionRepository disbursementRepository;
        RequisitionDetailRepository requisitionDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            disbursementRepository = new RequisitionRepository(context);
            requisitionDetailRepository = new RequisitionDetailRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = requisitionDetailRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = requisitionDetailRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            var expected = new RequisitionDetail()
            {
                RequisitionId = "RQDREPOTEST",
                ItemCode = "E030",
            };
            disbursementRepository.Save(new Requisition()
            {
                RequisitionId = "RQDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            requisitionDetailRepository.Save(expected);

            // Act
            var result = requisitionDetailRepository.FindById("RQDREPOTEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RequisitionDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            var expected = new RequisitionDetail()
            {
                RequisitionId = "RQDREPOTEST",
                ItemCode = "E030",
            };
            disbursementRepository.Save(new Requisition()
            {
                RequisitionId = "RQDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            requisitionDetailRepository.Save(expected);

            // Act
            var result = requisitionDetailRepository.ExistsById("RQDREPOTEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            disbursementRepository.Save(new Requisition()
            {
                RequisitionId = "RQDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            requisitionDetailRepository.Save(new RequisitionDetail()
            {
                RequisitionId = "RQDREPOTEST",
                Status = status,
                ItemCode = "E030",
            });

            // Arrange - Get Existing
            var expected = requisitionDetailRepository.FindById("RQDREPOTEST", "E030");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = requisitionDetailRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void Save_InstanceOfType()
        {
            // Arrange
            var requisitionDetail = new RequisitionDetail
            {
                RequisitionId = "RQDREPOTEST",
                ItemCode = "P030"
            };
            disbursementRepository.Save(new Requisition()
            {
                RequisitionId = "RQDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var saveResult = requisitionDetailRepository.Save(requisitionDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(RequisitionDetail));
        }

        [TestMethod]
        public void Delete_CannotFind()
        {
            // Arrange
            disbursementRepository.Save(new Requisition()
            {
                RequisitionId = "RQDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            var saveResult = requisitionDetailRepository.Save(new RequisitionDetail()
            {
                RequisitionId = "RQDREPOTEST",
                ItemCode = "E030",
            });

            // Act
            requisitionDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(requisitionDetailRepository.FindById("RQDREPOTEST", "E030"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (requisitionDetailRepository.ExistsById("RQDREPOTEST", "E030"))
                requisitionDetailRepository.Delete(requisitionDetailRepository.FindById("RQDREPOTEST", "E030"));

            if (requisitionDetailRepository.ExistsById("RQDREPOTEST", "P030"))
                requisitionDetailRepository.Delete(requisitionDetailRepository.FindById("RQDREPOTEST", "P030"));

            if (disbursementRepository.ExistsById("RQDREPOTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("RQDREPOTEST"));

        }
    }
}
