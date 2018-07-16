using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class RegisterService
    {
        ApplicationDbContext context;
        public RegisterService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<Department> FindAllDepartments()
        {
            throw new NotImplementedException();
        }
        public ApplicationUser RegisterNewUser()
        {
            throw new NotImplementedException();
        }
        public ApplicationUser FindUsersByDepartment(Department department)
        {
            throw new NotImplementedException();
        }
    }
}