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
        public ApplicationDbContext context = new ApplicationDbContext();
        DepartmentService departmentService;
        DelegationService delegationService;
        DelegationRepository delegationRepository;
        UserService userService;
        ApplicationUser user;

        public DepartmentAPIController()
        {
            departmentService = new DepartmentService(context);
            delegationService = new DelegationService(context);
            userService = new UserService(context);
            user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
        }
        
        [Route("api/department/all")] 
        [HttpGet]
        public List<DepartmentViewModel> Departments()
        {

            return departmentService.FindAllDepartments().Select(department => new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                DepartmentHead = department.Head.FirstName+ " " + department.Head.LastName,
                DepartmentRep = department.Representative != null ? department.Representative.FirstName + " " + department.Representative.LastName : "",
                CollectionPoint = department.CollectionPoint != null ? department.CollectionPoint.Name : "",
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber,
                EmailHead = department.Head.Email
            }).ToList();
        }
        public DepartmentViewModel GetDepartment(string id)
        {
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
                EmailHead = department.Head.Email
            };
        }
        [Route("api/delegation/all")]
        [HttpGet]
        public List<DepartmentViewModel> Delegations()
        {
            return delegationService.FindDelegationsByDepartment(user).Select(delegation => new DepartmentViewModel() //only displays delegation from your department
            //return delegationService.FindAllDelegations().Select(delegation => new DepartmentViewModel()
            {
                DelegationId = delegation.DelegationId,
                Recipient = delegation.Receipient != null? delegation.Receipient.FirstName + " " + delegation.Receipient.LastName : "",
                StartDate = delegation.StartDate.ToShortDateString(),
                EndDate = delegation.EndDate.ToShortDateString(),
                DelegationStatus = delegation.Status.StatusId
            }).ToList();
        }
    }
}