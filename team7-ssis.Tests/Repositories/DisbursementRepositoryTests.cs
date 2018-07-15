using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
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
            // Act
            var result = disbursementRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Disbursement));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = disbursementRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeDepartment()
        {
            // Arrange
            var department = new DepartmentRepository(context).FindById("ZOOL");
            var disbursement = disbursementRepository.FindById("TEST");
            var original = disbursement.Department;
            disbursement.Department = department;

            // Act
            var result = disbursementRepository.Save(disbursement);

            // Assert
            Assert.AreEqual(department, result.Department);

            // Tear Down
            disbursement.Department = original;
            disbursementRepository.Save(disbursement);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var disbursement = new Disbursement
            {
                DisbursementId = "UNIT TEST",
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
            Assert.IsNull(disbursementRepository.FindById("UNIT TEST"));
        }
    }
}