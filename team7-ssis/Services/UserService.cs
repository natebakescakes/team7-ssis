using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class UserService
    {
        ApplicationDbContext context;
        public UserService(ApplicationDbContext context)
        {
            this.context = context;
        }
    
        public ApplicationUser RegisterNewUser()
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> listSupervisorsByUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> listManagersByUser(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

    }
}