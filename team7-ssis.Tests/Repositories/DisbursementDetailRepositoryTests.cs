using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DisbursementDetailRepositoryTests
    {
        ApplicationDbContext context;
        DisbursementRepository disbursementRepository;
        DisbursementDetailRepository disbursementDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            disbursementRepository = new DisbursementRepository(context);
            disbursementDetailRepository = new DisbursementDetailRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = disbursementDetailRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = disbursementDetailRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            var expected = new DisbursementDetail()
            {
                DisbursementId = "DDREPOTEST",
                ItemCode = "E030",
            };
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            disbursementDetailRepository.Save(expected);

            // Act
            var result = disbursementDetailRepository.FindById("DDREPOTEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DisbursementDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            var expected = new DisbursementDetail()
            {
                DisbursementId = "DDREPOTEST",
                ItemCode = "E030",
            };
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            disbursementDetailRepository.Save(expected);

            // Act
            var result = disbursementDetailRepository.ExistsById("DDREPOTEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            disbursementDetailRepository.Save(new DisbursementDetail()
            {
                DisbursementId = "DDREPOTEST",
                Status = status,
                ItemCode = "E030",
            });

            // Arrange - Get Existing
            var expected = disbursementDetailRepository.FindById("DDREPOTEST", "E030");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = disbursementDetailRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void Save_InstanceOfType()
        {
            // Arrange
            var disbursementDetail = new DisbursementDetail
            {
                DisbursementId = "DDREPOTEST",
                ItemCode = "P030"
            };
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var saveResult = disbursementDetailRepository.Save(disbursementDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(DisbursementDetail));
        }

        [TestMethod]
        public void Delete_CannotFind()
        {
            // Arrange
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DDREPOTEST",
                CreatedDateTime = DateTime.Now,
            });
            var saveResult = disbursementDetailRepository.Save(new DisbursementDetail()
            {
                DisbursementId = "DDREPOTEST",
                ItemCode = "E030",
            });

            // Act
            disbursementDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(disbursementDetailRepository.FindById("DDDREPOTEST", "E030"));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (disbursementDetailRepository.ExistsById("DDREPOTEST", "E030"))
                disbursementDetailRepository.Delete(disbursementDetailRepository.FindById("DDREPOTEST", "E030"));

            if (disbursementDetailRepository.ExistsById("DDREPOTEST", "P030"))
                disbursementDetailRepository.Delete(disbursementDetailRepository.FindById("DDREPOTEST", "P030"));

            if (disbursementRepository.ExistsById("DDREPOTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("DDREPOTEST"));

        }
    }
}
