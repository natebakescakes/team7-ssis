using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class RoleRepository : CrudRepository<IdentityRole, String>
    {
        public RoleRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = (DbSet<IdentityRole>)context.Roles;
        }
    }
}