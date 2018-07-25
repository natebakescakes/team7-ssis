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
        
        [Route("api/department/all")] 
        [HttpGet]
        public List<DepartmentViewModel> Departments()
        {
            departmentService = new DepartmentService(context);
            return departmentService.FindAllDepartments().Select(department => new DepartmentViewModel()
            {
                DepartmentCode = department.DepartmentCode,
                DepartmentName = department.Name,
                ContactName = department.ContactName,
                PhoneNumber = department.PhoneNumber,
                FaxNumber = department.FaxNumber
            }).ToList();
        }
    }
}