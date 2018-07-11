using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class UserRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ApplicationUser Save(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser FindById(int userId)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Users.Count();
        }

        public void Delete(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
