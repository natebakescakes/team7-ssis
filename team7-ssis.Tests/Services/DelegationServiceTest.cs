using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DelegationServiceTest
    {
        private ApplicationDbContext context;
        private DelegationService delegationService;
        private DepartmentService departmentService;
        private UserService userService;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            delegationService = new DelegationService(context);
            departmentService = new DepartmentService(context);
            userService = new UserService(context);

            // Ensure CommerceHead@email.com is head of department
            var department = departmentService.FindDepartmentByDepartmentCode("COMM");
            department.Head = userService.FindUserByEmail("CommerceHead@email.com");
            departmentService.Save(department);
        }

        [TestMethod]
        public void DelegateManagerRole_Valid()
        {
            // Arrange

            // Act
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");

            // Assert
            Assert.IsTrue(userService.FindRolesByEmail("CommerceEmp@email.com").Contains("DepartmentHead"));
            Assert.AreEqual("CommerceEmp@email.com", delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault().Receipient.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelegateManagerRole_InvalidDate_ThrowsException()
        {
            // Arrange

            // Act
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sun Jul 29 00:00:00 GMT+08:00 2018", "Sat Jul 28 00:00:00 GMT + 08:00 2018");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelegateManagerRole_NotHead_ThrowsException()
        {
            // Arrange

            // Act
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceRep@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DelegateManagerRole_NotSameDepartment_ThrowsException()
        {
            // Arrange

            // Act
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "RegistraHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");
        }

        [TestMethod]
        public void CancelDelegation_Valid()
        {
            // Arrange
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");

            // Act
            var latestDelegation = delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault();
            delegationService.CancelDelegation(latestDelegation.DelegationId, "CommerceHead@email.com");

            // Assert
            Assert.AreEqual(3, latestDelegation.Status.StatusId);
            Assert.AreEqual("CommerceHead@email.com", latestDelegation.UpdatedBy.Email);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CancelDelegation_Invalid()
        {
            // Act
            delegationService.CancelDelegation(999999, "CommerceHead@email.com");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CancelDelegation_NotHead()
        {
            // Arrange
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");

            // Act
            var latestDelegation = delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault();
            delegationService.CancelDelegation(latestDelegation.DelegationId, "CommerceEmp@email.com");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Remove delegation
            if (userService.FindRolesByEmail("CommerceEmp@email.com").Contains("DepartmentHead"))
                userService.RemoveDepartmentHeadRole("CommerceEmp@email.com");

            // Add User Role
            if (!userService.FindRolesByEmail("CommerceHead@email.com").Contains("DepartmentHead"))
                userService.AddDepartmentHeadRole("CommerceHead@email.com");

            // Remove Delegation
            var latestDelegation = delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault();

            if (latestDelegation != null && 
                latestDelegation.Receipient.Email == "CommerceEmp@email.com" &&
                latestDelegation.CreatedBy.Email == "CommerceHead@email.com")
                new DelegationRepository(context).Delete(latestDelegation);
        }
    }
}
