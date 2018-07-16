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

        public Department FindDepartmentByUser()
        {
            throw new NotImplementedException();
        }
        public CollectionPoint FindAllCollectionPoint()
        {
            throw new NotImplementedException();
        }
        public ApplicationUser FindUsersByRole(ApplicationUser user)
        {
            throw new NotImplementedException();
        }
        public Department Save()
        {
            throw new NotImplementedException();
        }
    }
}