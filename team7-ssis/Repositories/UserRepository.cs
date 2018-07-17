using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class UserRepository : CrudRepository<ApplicationUser, String>
    {
        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = (DbSet<ApplicationUser>)context.Users;
        }

        public ApplicationUser FindByEmail(String email)
        {
            return entity.FirstOrDefault(x => x.Email == email);
        }

        public bool ExistsByEmail(String email)
        {
            return entity.FirstOrDefault(x => x.Email == email) != null;
        }
        public IQueryable<ApplicationUser> FindByDepartment(Department department)
        {
            return context.Users.Where(x => x.Department.DepartmentCode == department.DepartmentCode);
        }
    }
}
