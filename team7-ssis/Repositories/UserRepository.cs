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
        public IQueryable<ApplicationUser> FindRepByDepartment(Department department)
        {
            return context.Users.Where(x => x.Email == department.Representative.Email);
        }

        /// <summary>
        /// Finds users who are supervisors and match department
        /// </summary>
        /// <param name="department"></param>
        /// <returns>IQueryable of matching users</returns>
        public IQueryable<ApplicationUser> FindSupervisorByDepartment(Department department)
        {
            // list of supervisors
            var supervisorList = context.Users.Select(x => x.Supervisor.UserName).Distinct();

            return context.Users
                .Where(x => x.Department.DepartmentCode == department.DepartmentCode &&
                    supervisorList.Contains(x.UserName));
        }
        
    }
}
