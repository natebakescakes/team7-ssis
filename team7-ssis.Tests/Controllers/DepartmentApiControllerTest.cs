using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class DepartmentApiControllerTest
    {
        private ApplicationDbContext context;
        private UserService userService;
        private DelegationService delegationService;
        private DepartmentService departmentService;

        public DepartmentApiControllerTest()
        {
            context = new ApplicationDbContext();
            userService = new UserService(context);
            delegationService = new DelegationService(context);
            departmentService = new DepartmentService(context);
        }

        [TestMethod]
        public void GetDepartmentOptions_Valid()
        {
            // Arrange
            var department = new DepartmentRepository(context).FindById("COMM");
            var expectedRepresentative = department.Representative != null ? $"{department.Representative.FirstName} {department.Representative.LastName}" : "";
            var expectedEmployeeEmail = "CommerceHead@email.com";
            var expectedDepartment = department.Name;

            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            // Act
            IHttpActionResult actionResult = controller.GetDepartmentOptions(new EmailViewModel()
            {
                Email = "CommerceHead@email.com",
            });

            var contentResult = actionResult as OkNegotiatedContentResult<DepartmentOptionsViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(expectedDepartment, contentResult.Content.Department);
            Assert.AreEqual(expectedRepresentative, contentResult.Content.Representative);
            Assert.IsTrue(contentResult.Content.Employees.Select(e => e.Email).Contains(expectedEmployeeEmail));
        }

        [TestMethod]
        public void ChangeRepresentative_Valid()
        {
            // Arrange
            var expected = "CommerceEmp@email.com";
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            var department = departmentService.FindDepartmentByDepartmentCode("COMM");

            // Act
            IHttpActionResult actionResult = controller.ChangeRepresentative(new ChangeRepresentativeViewModel()
            {
                RepresentativeEmail = "CommerceEmp@email.com",
                HeadEmail = "CommerceHead@email.com",
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Successfully changed");
            Assert.AreEqual(expected, new UserService(context).FindUserByEmail("CommerceEmp@email.com").Department.Representative.Email);
        }

        [TestMethod]
        public void ChangeRepresentative_NoRights_BadRequest()
        {
            // Arrange
            var expected = "User does not have managerial rights";
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.ChangeRepresentative(new ChangeRepresentativeViewModel()
            {
                RepresentativeEmail = "CommerceEmp@email.com",
                HeadEmail = "CommerceEmp@email.com",
            });
            BadRequestErrorMessageResult badRequest = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(expected, badRequest.Message);
        }

        [TestMethod]
        public void ChangeRepresentative_NotSameDepartment_BadRequest()
        {
            // Arrange
            var expected = "Representative and Requestor not from same Department";
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.ChangeRepresentative(new ChangeRepresentativeViewModel()
            {
                RepresentativeEmail = "CommerceEmp@email.com",
                HeadEmail = "RegistraHead@email.com",
            });
            BadRequestErrorMessageResult badRequest = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(expected, badRequest.Message);
        }

        [TestMethod]
        public void DelegateManagerRole_Valid()
        {
            // Arrange
            var expected = "CommerceEmp@email.com";
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.DelegateManagerRole(new DelegationSubmitViewModel()
            {
                RecipientEmail = "CommerceEmp@email.com",
                HeadEmail = "CommerceHead@email.com",
                StartDate = "Sat Jul 28 00:00:00 GMT + 08:00 2018",
                EndDate = "Sun Jul 29 00:00:00 GMT+08:00 2018"
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Successfully delegated");
            Assert.AreEqual(expected, new DelegationService(context).FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault().Receipient.Email);
        }

        [TestMethod]
        public void DelegateManagerRole_InvalidDates_BadRequest()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.DelegateManagerRole(new DelegationSubmitViewModel()
            {
                RecipientEmail = "CommerceEmp@email.com",
                HeadEmail = "CommerceHead@email.com",
                StartDate = "Sun Jul 29 00:00:00 GMT+08:00 2018",
                EndDate = "Sat Jul 28 00:00:00 GMT + 08:00 2018",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Invalid dates", badRequestResult.Message);
        }

        [TestMethod]
        public void DelegateManagerRole_NotHead_BadRequest()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.DelegateManagerRole(new DelegationSubmitViewModel()
            {
                RecipientEmail = "CommerceEmp@email.com",
                HeadEmail = "CommerceEmp@email.com",
                StartDate = "Sat Jul 28 00:00:00 GMT + 08:00 2018",
                EndDate = "Sun Jul 29 00:00:00 GMT+08:00 2018",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Only Department Heads can delegate roles", badRequestResult.Message);
        }

        [TestMethod]
        public void DelegateManagerRole_NotSameDepartment_BadRequest()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.DelegateManagerRole(new DelegationSubmitViewModel()
            {
                RecipientEmail = "CommerceEmp@email.com",
                HeadEmail = "RegistraHead@email.com",
                StartDate = "Sat Jul 28 00:00:00 GMT + 08:00 2018",
                EndDate = "Sun Jul 29 00:00:00 GMT+08:00 2018",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Representative and Department Head not from same Department", badRequestResult.Message);
        }

        [TestMethod]
        public void CancelDelegation_Valid()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");

            // Act
            var latestDelegation = delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault();
            IHttpActionResult actionResult = controller.CancelDelegation(new CancelDelegationViewModel()
            {
                DelegationId = latestDelegation.DelegationId,
                HeadEmail = "CommerceHead@email.com",
            });
            var contentResult = actionResult as OkNegotiatedContentResult<MessageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(contentResult.Content.Message, "Successfully cancelled");
            Assert.AreEqual(3, delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault().Status.StatusId);
        }

        [TestMethod]
        public void CancelDelegation_InvalidId_BadRequest()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };

            // Act
            IHttpActionResult actionResult = controller.CancelDelegation(new CancelDelegationViewModel()
            {
                DelegationId = 999999,
                HeadEmail = "CommerceHead@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Delegation does not exist", badRequestResult.Message);
        }

        [TestMethod]
        public void CancelDelegation_NotHead_BadRequest()
        {
            // Arrange
            var controller = new DepartmentAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
                Context = context,
            };
            delegationService.DelegateManagerRole("CommerceEmp@email.com", "CommerceHead@email.com", "Sat Jul 28 00:00:00 GMT + 08:00 2018", "Sun Jul 29 00:00:00 GMT+08:00 2018");

            // Act
            var latestDelegation = delegationService.FindAllDelegations().OrderByDescending(d => d.DelegationId).FirstOrDefault();
            IHttpActionResult actionResult = controller.CancelDelegation(new CancelDelegationViewModel()
            {
                DelegationId = latestDelegation.DelegationId,
                HeadEmail = "CommerceEmp@email.com",
            });
            var badRequestResult = actionResult as BadRequestErrorMessageResult;

            // Assert
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Only Department Heads can cancel delegations", badRequestResult.Message);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Change back representative
            var department = departmentService.FindDepartmentByUser(new UserRepository(context).FindByEmail("CommerceHead@email.com"));
            department.Representative = new UserRepository(context).FindByEmail("CommerceRep@email.com");
            departmentService.Save(department);

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
