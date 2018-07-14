using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class UserRepositoryTests
    {
        ApplicationDbContext context;
        UserRepository userRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            userRepository = new UserRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = userRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = userRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByEmailTestNotNull()
        {
            // Act
            var result = userRepository.FindByEmail("root@admin.com");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ApplicationUser));
        }

        [TestMethod]
        public void ExistsByEmailTestIsTrue()
        {
            // Act
            var result = userRepository.ExistsByEmail("root@admin.com");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeLastName()
        {
            // Save new object into DB
            // Arrange
            var user = userRepository.FindByEmail("root@admin.com");
            user.LastName = "Admin";

            // Act
            var saveResult = userRepository.Save(user);

            // Assert
            Assert.AreEqual("Admin", saveResult.LastName);

            // Tear Down
            user.LastName = "";
            userRepository.Save(user);
        }
    }
}