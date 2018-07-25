using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class DepartmentAPIController : ApiController
    {
        public ApplicationDbContext context = new ApplicationDbContext();
        DepartmentService departmentService;

        public DepartmentAPIController()
        {
            departmentService = new DepartmentService(context);
        }
        
        [Route("api/department/all")] 
        [HttpGet]
        public List<DepartmentViewModel> Departments()
        {
            
            return departmentService.FindAllDepartments().Select(department => new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber
            }).ToList();
        }
        public DepartmentViewModel GetDepartment(string id)
        {
            Department department = departmentService.FindDepartmentByDepartmentCode(id);
            return new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber,
                Status = department.Status.StatusId
            };
        }
    }
}