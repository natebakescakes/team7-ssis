using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class TitleRepositoryTests
    {
        ApplicationDbContext context;
        TitleRepository titleRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            titleRepository = new TitleRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = titleRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = titleRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = titleRepository.FindById(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Title));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = titleRepository.ExistsById(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeName()
        {
            // Arrange
            var title = titleRepository.FindById(1);
            var original = title.Name;
            title.Name = "Mr..";

            // Act
            var result = titleRepository.Save(title);

            // Assert
            Assert.AreEqual("Mr..", result.Name);

            // Tear Down
            title.Name = original;
            titleRepository.Save(title);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var title = new Title
            {
                TitleId = 999999,
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = titleRepository.Save(title);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Title));

            // Delete saved object from DB
            // Act
            titleRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(titleRepository.FindById(999999));
        }
    }
}
