using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DelegationRepositoryTests
    {
        ApplicationDbContext context;
        DelegationRepository delegationRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            delegationRepository = new DelegationRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = delegationRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = delegationRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            delegationRepository.Save(new Delegation()
            {
                DelegationId = 999999,
                CreatedDateTime = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            });

            // Act
            var result = delegationRepository.FindById(999999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Delegation));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            delegationRepository.Save(new Delegation()
            {
                DelegationId = 999999,
                CreatedDateTime = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            });

            // Act
            var result = delegationRepository.ExistsById(999999);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            delegationRepository.Save(new Delegation()
            {
                DelegationId = 999999,
                CreatedDateTime = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = delegationRepository.FindById(999999);
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = delegationRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var delegation = new Delegation
            {
                DelegationId = 999999,
                CreatedDateTime = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            };

            // Act
            var saveResult = delegationRepository.Save(delegation);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Delegation));

            // Delete saved object from DB
            // Act
            delegationRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(delegationRepository.FindById(999999));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            delegationRepository.Save(new Delegation()
            {
                DelegationId = 999999,
                CreatedDateTime = DateTime.Now,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            });

            // Act
            var result = delegationRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (delegationRepository.ExistsById(999999))
                delegationRepository.Delete(delegationRepository.FindById(999999));

        }
    }
}
