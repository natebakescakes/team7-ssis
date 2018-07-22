using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        ApplicationDbContext context;
        UserRepository userRepository;
        UserService userService;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            userService = new UserService(context);
            userRepository = new UserRepository(context);
        }

        [TestMethod]
        public void FindAllTest()
        {
            // Arrange
            var expected = userRepository.Count();

            // Act
            var result = userService.FindAllUsers().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindByIdTest()
        {
            // Arrange
            var expected = "root@admin.com";

            // Act
            var result = userService.FindUserByEmail("root@admin.com");

            // Assert
            Assert.AreEqual(expected, result.Email);
        }
    }
}
