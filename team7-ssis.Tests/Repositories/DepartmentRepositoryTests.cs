using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass]
    public class DepartmentRepositoryTests
    {
        ApplicationDbContext context;
        DepartmentRepository departmentRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            departmentRepository = new DepartmentRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = departmentRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = departmentRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = departmentRepository.FindById("ENGL");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Department));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = departmentRepository.ExistsById("ENGL");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeEmployees()
        {
            // Arrange
            var user = new UserRepository(context).FindByEmail("root@admin.com");
            var department = departmentRepository.FindById("ZOOL");
            var original = department.Employees;
            department.Employees = new List<ApplicationUser>() { user };

            // Act
            var result = departmentRepository.Save(department);

            // Assert
            CollectionAssert.Contains(result.Employees, user);

            // Tear Down
            department.Employees = original;
            departmentRepository.Save(department);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var department = new Department
            {
                DepartmentCode = "XXXX",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = departmentRepository.Save(department);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Department));

            // Delete saved object from DB
            // Act
            departmentRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(departmentRepository.FindById("XXXX"));
        }
    }
}
