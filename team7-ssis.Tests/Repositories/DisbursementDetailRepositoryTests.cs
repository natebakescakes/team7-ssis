using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DisbursementDetailRepositoryTests
    {
        ApplicationDbContext context;
        DisbursementDetailRepository disbursementDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
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
            int result = disbursementDetailRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = disbursementDetailRepository.FindById("TEST", "E030");

            // Assert
            Assert.IsInstanceOfType(result, typeof(DisbursementDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = disbursementDetailRepository.ExistsById("TEST", "E030");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeUpdatedBy()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var disbursementDetail = disbursementDetailRepository.FindById("TEST", "E030");
            var original = disbursementDetail.UpdatedBy;
            disbursementDetail.UpdatedBy = user;

            // Act
            var result = disbursementDetailRepository.Save(disbursementDetail);

            // Assert
            Assert.AreEqual(user, result.UpdatedBy);

            // Tear Down
            disbursementDetail.UpdatedBy = original;
            disbursementDetailRepository.Save(disbursementDetail);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var disbursementDetail = new DisbursementDetail
            {
                DisbursementId = "TEST",
                ItemCode = "P030"
            };

            // Act
            var saveResult = disbursementDetailRepository.Save(disbursementDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(DisbursementDetail));

            // Delete saved object from DB
            // Act
            disbursementDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(disbursementDetailRepository.FindById("TEST", "P030"));
        }
    }
}
