using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DisbursementRepositoryTests
    {
        ApplicationDbContext context;
        DisbursementRepository disbursementRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            disbursementRepository = new DisbursementRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = disbursementRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = disbursementRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = disbursementRepository.FindById("DOREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Disbursement));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = disbursementRepository.ExistsById("DOREPOTEST");

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
                DisbursementId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = disbursementRepository.FindById("DOREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = disbursementRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var disbursement = new Disbursement
            {
                DisbursementId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = disbursementRepository.Save(disbursement);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Disbursement));

            // Delete saved object from DB
            // Act
            disbursementRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(disbursementRepository.FindById("DOREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DOREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = disbursementRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (disbursementRepository.ExistsById("DOREPOTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("DOREPOTEST"));

        }
    }
}
