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
            // Act
            var result = delegationRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Delegation));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = delegationRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeReceipient()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var delegation = delegationRepository.FindById(1);
            var original = delegation.Receipient;
            delegation.Receipient = user;

            // Act
            var result = delegationRepository.Save(delegation);

            // Assert
            Assert.AreEqual(user, result.Receipient);

            // Tear Down
            delegation.Receipient = original;
            delegationRepository.Save(delegation);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var delegation = new Delegation
            {
                DelegationId = 999999,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                CreatedDateTime = DateTime.Now
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
    }
}
