using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
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
            int result = requisitionRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RQREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = requisitionRepository.FindById("RQREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Requisition));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RQREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = requisitionRepository.ExistsById("RQREPOTEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RQREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = requisitionRepository.FindById("RQREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = requisitionRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var requisition = new Requisition
            {
                RequisitionId = "RQREPOTEST",
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
            Assert.IsNull(requisitionRepository.FindById("RQREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RQREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = requisitionRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestMethod]
        public void FindByDepartmentTest()
        {
            // Arrange
            requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RQREPOTEST",
                Department = new DepartmentRepository(context).FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = requisitionRepository.FindByDepartment(new DepartmentRepository(context).FindById("ENGL"));

            // Assert
            result.ToList().ForEach(r => Assert.AreEqual("ENGL", r.Department.DepartmentCode));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (requisitionRepository.ExistsById("RQREPOTEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("RQREPOTEST"));

        }
    }
}
