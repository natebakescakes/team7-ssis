using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class RetrievalRepositoryTests
    {
        ApplicationDbContext context;
        RetrievalRepository retrievalRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            retrievalRepository = new RetrievalRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = retrievalRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = retrievalRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Arrange
            retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RTREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = retrievalRepository.FindById("RTREPOTEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Retrieval));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Arrange
            retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RTREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = retrievalRepository.ExistsById("RTREPOTEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Save_ChangeStatus_Valid()
        {
            // Arrange - Initialize
            var status = new StatusRepository(context).FindById(14);
            retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RTREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Arrange - Get Existing
            var expected = retrievalRepository.FindById("RTREPOTEST");
            expected.Status = new StatusRepository(context).FindById(15);

            // Act
            var result = retrievalRepository.Save(expected);

            // Assert
            Assert.AreEqual(expected.Status, result.Status);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var retrieval = new Retrieval
            {
                RetrievalId = "RTREPOTEST",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = retrievalRepository.Save(retrieval);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Retrieval));

            // Delete saved object from DB
            // Act
            retrievalRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(retrievalRepository.FindById("RTREPOTEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange
            retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RTREPOTEST",
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = retrievalRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (retrievalRepository.ExistsById("RTREPOTEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("RTREPOTEST"));

        }
    }
}
