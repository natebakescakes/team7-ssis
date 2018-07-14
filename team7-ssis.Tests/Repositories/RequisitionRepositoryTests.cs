using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class RequisitionRepositoryTests
    {
        ApplicationDbContext context;
        RequisitionRepository requisitionRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            requisitionRepository = new RequisitionRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = requisitionRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = requisitionRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = requisitionRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Requisition));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = requisitionRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(3);
            var requisition = requisitionRepository.FindById("TEST");
            var original = requisition.Status;
            requisition.Status = status;

            // Act
            var result = requisitionRepository.Save(requisition);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            requisition.Status = original;
            requisitionRepository.Save(requisition);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var requisition = new Requisition
            {
                RequisitionId = "UNIT TEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = requisitionRepository.Save(requisition);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Requisition));

            // Delete saved object from DB
            // Act
            requisitionRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(requisitionRepository.FindById("UNIT TEST"));
        }
    }
}