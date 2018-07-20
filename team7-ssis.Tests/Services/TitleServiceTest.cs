using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class TitleServiceTest
    {
        private ApplicationDbContext context;
        private TitleRepository titleRepository;
        private TitleService titleService;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            titleRepository = new TitleRepository(context);
            titleService = new TitleService(context);
        }

        [TestMethod]
        public void FindAllTest()
        {
            // Arrange
            var expected = titleRepository.Count();

            // Act
            var result = titleService.FindAllTitles().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindByIdTest()
        {
            // Arrange
            var expected = "Mr.";

            // Act
            var result = titleService.FindTitleByTitleId(1);

            // Assert
            Assert.AreEqual(expected, result.Name);
        }
    }
}
