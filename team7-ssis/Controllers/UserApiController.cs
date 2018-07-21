using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    public class UserApiController : ApiController
    {
        private ApplicationDbContext context;

        public UserApiController()
        {
            context = new ApplicationDbContext();
        }

        [Route("api/users/supervisors/{departmentCode}")]
        public IHttpActionResult GetSupervisorsFromDepartment(string departmentCode)
        {
            var supervisors = new UserService(context).FindSupervisorsByDepartment(new DepartmentService(context).FindDepartmentByDepartmentCode(departmentCode));

            if (supervisors.Count == 0) return NotFound();

            return Ok(supervisors.Select(supervisor => new EmailNameViewModel
            {
                Email = supervisor.Email,
                Name = $"{supervisor.FirstName} {supervisor.LastName}"
            }));
        }

        [Route("api/users/{departmentCode}")]
        public IHttpActionResult GetUsersFromDepartment(string departmentCode)
        {
            var users = new UserService(context).FindUsersByDepartment(new DepartmentService(context).FindDepartmentByDepartmentCode(departmentCode));

            if (users.Count == 0) return NotFound();

            return Ok(users.Select(supervisor => new EmailNameViewModel
            {
                Email = supervisor.Email,
                Name = $"{supervisor.FirstName} {supervisor.LastName}"
            }));
        }
    }
}
