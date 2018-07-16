using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            // Act
            var result = retrievalRepository.FindById("TEST");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Retrieval));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = retrievalRepository.ExistsById("TEST");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(3);
            var retrieval = retrievalRepository.FindById("TEST");
            var original = retrieval.Status;
            retrieval.Status = status;

            // Act
            var result = retrievalRepository.Save(retrieval);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            retrieval.Status = original;
            retrievalRepository.Save(retrieval);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var retrieval = new Retrieval
            {
                RetrievalId = "UNIT TEST",
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
            Assert.IsNull(retrievalRepository.FindById("UNIT TEST"));
        }

        [TestMethod]
        public void FindByCreatedDateTimeTestNotNull()
        {
            // Arrange

            // Act
            var result = retrievalRepository.FindByCreatedDateTime(DateTime.Now.Date.AddYears(-1), DateTime.Now.Date.AddDays(1));

            // Assert
            Assert.IsTrue(result.Count() >= 1);
        }
    }
}
