using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DepartmentServiceTests
    {
        ApplicationDbContext context;
        DepartmentService departmentService;
        DepartmentRepository departmentRepository;
        UserService userService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            departmentService = new DepartmentService(context);
            departmentRepository = new DepartmentRepository(context);
            userService = new UserService(context);
        }

        [TestMethod]
        public void FindUsersByDepartmentTest()
        {
            //Arrange
            int expected = context.Users.Where(x => x.Department.DepartmentCode == "COMM").Count();
            Department departmentvar = context.Department.Where(x => x.DepartmentCode == "COMM").FirstOrDefault();

            //Act
            var result = departmentService.FindUsersByDepartment(departmentvar).Count();
            //Assert
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void FindUsersByDepartmentObjectTest()
        {
            //Arrange
            Department department = context.Department.Where(x => x.DepartmentCode == "COMM").FirstOrDefault();
            //Act
            var result = departmentService.FindUsersByDepartment(department);
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(ApplicationUser));
            result.ForEach(x => Assert.AreEqual("COMM", x.Department.DepartmentCode));
        }
        [TestMethod]
        public void FindUsersByDepartmentNotNullTest()
        {
            //Arrange
            Department department = new Department
            {
                DepartmentCode = "ENGL"
            };
            //Assert
            CollectionAssert.AllItemsAreNotNull(departmentService.FindUsersByDepartment(department));
        }
        [TestMethod]
        public void FindUsersByDepartmentUniqueTest()
        {
            //Arrange
            Department department = new Department
            {
                DepartmentCode = "ENGL"
            };
            //Assert
            CollectionAssert.AllItemsAreUnique(departmentService.FindUsersByDepartment(department));
        }

        [TestMethod]
        public void FindAllTest()
        {
            // Arrange
            var expected = departmentRepository.Count();

            // Act
            var result = departmentService.FindAllDepartments().Count;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindByIdTest()
        {
            // Arrange
            var expected = "ENGL";

            // Act
            var result = departmentService.FindDepartmentByDepartmentCode("ENGL");

            // Assert
            Assert.AreEqual(expected, result.DepartmentCode);
        }

        //[TestMethod]
        //public void FindDepartmentByUserTest()
        //{
        //    //Arrange
        //    ApplicationUser user = new ApplicationUser();
        //    user.Department.DepartmentCode = "COMM";
        //    string expected = "COMM";
        //    //Act
        //    var result = departmentService.FindDepartmentByUser(user).ToString();
        //    //Assert
        //    Assert.AreEqual(expected, result);
        //}

        [TestMethod]
        public void ChangeRepresentative_Valid()
        {
            // Arrange
            var expected = "CommerceEmp@email.com";
            if (!userService.FindRolesByEmail("CommerceHead@email.com").Contains("DepartmentHead"))
                userService.AddDepartmentHeadRole("CommerceHead@email.com");

            // Act
            departmentService.ChangeRepresentative(expected, "CommerceHead@email.com");

            // Assert
            Assert.AreEqual(expected, new UserRepository(context).FindByEmail("CommerceHead@email.com").Department.Representative.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeRepresentative_NotSameDepartment_ThrowsException()
        {
            // Arrange

            // Act
            departmentService.ChangeRepresentative("CommerceEmp@email.com", "RegistraHead@email.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ChangeRepresentative_NotManager_ThrowException()
        {
            // Arrange

            // Act
            departmentService.ChangeRepresentative("CommerceEmp@email.com", "CommerceEmp@email.com");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Change back representative
            var department = departmentService.FindDepartmentByUser(new UserRepository(context).FindByEmail("CommerceHead@email.com"));
            department.Representative = new UserRepository(context).FindByEmail("CommerceRep@email.com");
            departmentService.Save(department);
        }
    }
}
