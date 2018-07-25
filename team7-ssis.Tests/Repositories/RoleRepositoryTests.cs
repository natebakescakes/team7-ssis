using System;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class RoleRepositoryTests
    {
        ApplicationDbContext context;
        RoleRepository roleRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            roleRepository = new RoleRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = roleRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = roleRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = roleRepository.FindById("1");

            // Assert
            Assert.IsInstanceOfType(result, typeof(IdentityRole));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = roleRepository.ExistsById("1");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeName()
        {
            // Arrange
            var role = roleRepository.FindById("1");
            var original = role.Name;
            role.Name = "Employee";

            // Act
            var result = roleRepository.Save(role);

            // Assert
            Assert.AreEqual("Employee", result.Name);

            // Tear Down
            role.Name = original;
            roleRepository.Save(role);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var role = new IdentityRole
            {
                Id = "99",
                Name = "TEST"
            };

            // Act
            var saveResult = roleRepository.Save(role);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(IdentityRole));

            // Delete saved object from DB
            // Act
            roleRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(roleRepository.FindById("99"));
        }
    }
}
