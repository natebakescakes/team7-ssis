﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class DepartmentRepository : CrudRepository<Department, String>
    {
        public DepartmentRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Department;
        }
        public Department FindByUser(ApplicationUser user)
        {
            return context.Department.Where(x => x.DepartmentCode == user.Department.DepartmentCode).FirstOrDefault();
        }
    }
}
