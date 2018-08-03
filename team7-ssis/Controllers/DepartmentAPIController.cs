using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class DepartmentAPIController : ApiController
    {
        ApplicationDbContext context;

        public DepartmentAPIController()
        {
            context = new ApplicationDbContext();
        }

        public ApplicationDbContext Context { get { return context; } set { context = value; } }

        [Route("api/department/all")]
        [HttpGet]
        public List<DepartmentViewModel> Departments()
        {
            var departmentService = new DepartmentService(context);

            return departmentService.FindAllDepartments().Select(department => new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                DepartmentHead = department.Head.FirstName + " " + department.Head.LastName,
                DepartmentRep = department.Representative != null ? department.Representative.FirstName + " " + department.Representative.LastName : "",
                CollectionPoint = department.CollectionPoint != null ? department.CollectionPoint.Name : "",
                CollectionPointId = department.CollectionPoint != null ? department.CollectionPoint.CollectionPointId : 0,
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber,
                EmailHead = department.Head.Email
            }).ToList();
        }
        public DepartmentViewModel GetDepartment(string id)
        {
            var departmentService = new DepartmentService(context);

            Department department = departmentService.FindDepartmentByDepartmentCode(id);
            return new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                DepartmentHead = department.Head.FirstName + " " + department.Head.LastName,
                DepartmentRep = department.Representative != null ? department.Representative.FirstName + " " + department.Representative.LastName : "",
                CollectionPoint = department.CollectionPoint != null ? department.CollectionPoint.Name : "",
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber,
                Status = department.Status.StatusId,
                EmailHead = department.Head.Email,
                EmailRep = department.Representative != null ? department.Representative.Email : ""
            };
        }
        [Route("api/delegation/all")]
        [HttpGet]
        public List<DepartmentViewModel> Delegations()
        {
            var userService = new UserService(context);
            var delegationService = new DelegationService(context);

            var user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.Name);

            return delegationService.FindDelegationsByDepartment(user).Select(delegation => new DepartmentViewModel() //only displays delegation from your department
            //return delegationService.FindAllDelegations().Select(delegation => new DepartmentViewModel()
            {
                DelegationId = delegation.DelegationId,
                Recipient = delegation.Receipient != null ? delegation.Receipient.FirstName + " " + delegation.Receipient.LastName : "",
                StartDate = delegation.StartDate.ToShortDateString(),
                EndDate = delegation.EndDate.ToShortDateString(),
                DelegationStatus = delegation.Status.StatusId
            }).ToList();
        }

        [Route("api/departmentoptions/")]
        [HttpPost]
        public IHttpActionResult GetDepartmentOptions([FromBody] EmailViewModel model)
        {
            var departmentService = new DepartmentService(context);
            var userService = new UserService(context);

            var department = departmentService.FindDepartmentByUser(userService.FindUserByEmail(model.Email));

            var delegations = new DelegationService(context).FindDelegationsByDepartment(userService.FindUserByEmail(model.Email)).OrderByDescending(d => d.CreatedDateTime);

            return Ok(new DepartmentOptionsViewModel()
            {
                Department = department.Name,
                Representative = department.Representative != null ?
                    $"{department.Representative.FirstName} {department.Representative.LastName}" : "",
                Delegations = delegations.Select(delegation => new DelegationMobileViewModel()
                {
                    DelegationId = delegation.DelegationId,
                    Recipient = delegation.Receipient != null ? 
                        $"{delegation.Receipient.FirstName} {delegation.Receipient.LastName}" : "",
                    StartDate = delegation.StartDate.ToLongDateString(),
                    EndDate = delegation.EndDate.ToLongDateString(),
                    Status = delegation.Status.Name,
                }).ToList(),
                Employees = department.Employees.Select(employee => new EmployeeViewModel()
                {
                    Name = $"{employee.FirstName} {employee.LastName}",
                    Email = employee.Email,
                }).ToList(),
            });
        }

        [Route("api/departmentoptions/representative")]
        [HttpPost]
        public IHttpActionResult ChangeRepresentative([FromBody] ChangeRepresentativeViewModel model)
        {
            try
            {
                new DepartmentService(Context).ChangeRepresentative(model.RepresentativeEmail, model.HeadEmail);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully changed"
            });
        }

        [Route("api/departmentoptions/delegate")]
        [HttpPost]
        public IHttpActionResult DelegateManagerRole([FromBody] DelegationSubmitViewModel model)
        {
            try
            {
                new DelegationService(Context).DelegateManagerRole(model.RecipientEmail, model.HeadEmail, model.StartDate, model.EndDate);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully delegated"
            });
        }

        [Route("api/departmentoptions/canceldelegation")]
        [HttpPost]
        public IHttpActionResult CancelDelegation([FromBody] CancelDelegationViewModel model)
        {
            try
            {
                new DelegationService(Context).CancelDelegation(model.DelegationId, model.HeadEmail);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully cancelled"
            });
        }
    }
}