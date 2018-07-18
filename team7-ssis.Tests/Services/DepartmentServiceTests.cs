using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DepartmentServiceTests
    {
        ApplicationDbContext context;
        DepartmentService departmentService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            departmentService = new DepartmentService(context);
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
            result.ForEach(x => Assert.AreEqual("COMM", x.Department));
        }
        [TestMethod]
        public void FindUsersByDepartmentNotNullTest()
        {
            //Arrange
            Department department = new Department();
            department.DepartmentCode = "ENGL";
            //Assert
            CollectionAssert.AllItemsAreNotNull(departmentService.FindUsersByDepartment(department));
        }
        [TestMethod]
        public void FindUsersByDepartmentUniqueTest()
        {
            //Arrange
            Department department = new Department();
            department.DepartmentCode = "ENGL";
            //Assert
            CollectionAssert.AllItemsAreUnique(departmentService.FindUsersByDepartment(department));
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
    }
}
