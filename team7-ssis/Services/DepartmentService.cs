using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class DepartmentService
    {
        ApplicationDbContext context;
        public DepartmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Department FindDepartmentByUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
        
        public ApplicationUser FindUsersByRole(string role)
        {
            throw new NotImplementedException();
        }
        public Department Save(Department department)
        {
            throw new NotImplementedException();
        }
    }
}