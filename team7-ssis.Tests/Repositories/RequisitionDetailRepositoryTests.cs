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
        RequisitionDetailRepository requisitionDetailRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
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
            // Act
            var result = requisitionDetailRepository.FindById("TEST", "E032");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RequisitionDetail));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = requisitionDetailRepository.ExistsById("TEST", "E032");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeQuantity()
        {
            // Arrange
            var requisitionDetail = requisitionDetailRepository.FindById("TEST", "E032");
            var original = requisitionDetail.Quantity;
            requisitionDetail.Quantity = 999999;

            // Act
            var result = requisitionDetailRepository.Save(requisitionDetail);

            // Assert
            Assert.AreEqual(999999, result.Quantity);

            // Tear Down
            requisitionDetail.Quantity = original;
            requisitionDetailRepository.Save(requisitionDetail);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var requisitionDetail = new RequisitionDetail
            {
                RequisitionId = "TEST",
                ItemCode = "P030"
            };

            // Act
            var saveResult = requisitionDetailRepository.Save(requisitionDetail);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(RequisitionDetail));

            // Delete saved object from DB
            // Act
            requisitionDetailRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(requisitionDetailRepository.FindById("TEST", "P030"));
        }
    }
}
