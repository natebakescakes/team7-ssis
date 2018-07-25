using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{

    [TestClass]
    public class RetrievalServiceTest
    {
        ApplicationDbContext context;
        RetrievalService retrievalService;
        RetrievalRepository retrievalRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            retrievalRepository = new RetrievalRepository(context);
            retrievalService = new RetrievalService(context);
        }

        [TestMethod]
        public void FindAllRetrievalsTest()
        {
            //Arrange
            int expected = context.Retrieval.ToList().Count;

            //Act
            var result = retrievalService.FindAllRetrievals();

            //Assert
            Assert.AreEqual(expected, result.Count);

        }

        [TestMethod]
        [Ignore]
        public void FindRetrievalByIdTest()
        {
            //Arrange
            string expected = "TEST";

            //Act
            var result = retrievalService.FindRetrievalById(expected);

            //Assert
            Assert.AreEqual(expected, result.RetrievalId);

        }

        [TestMethod]
        [Ignore]
        public void SaveTest()
        {
            //Arrange
            Retrieval newRetrieval = new Retrieval();
            newRetrieval.RetrievalId = IdService.GetNewRetrievalId(context);
            newRetrieval.CreatedDateTime = DateTime.Now;
            string expected = newRetrieval.RetrievalId;

            //Act
            var result = retrievalService.Save(newRetrieval);
            retrievalRepository.Delete(newRetrieval);

            //Assert
            Assert.AreEqual(expected, result.RetrievalId);

            
           
        }

        [TestMethod]
        public void SaveEditTest()
        {
            //Arrange
            Retrieval retrieval = context.Retrieval.First();
            Status status = context.Status.Where(x => x.StatusId == 1).First();
            retrieval.Status = status;

            //Act
            var result = retrievalService.Save(retrieval);

            //Assert
            Assert.AreEqual(status.Name, result.Status.Name);
        }

    }
}
