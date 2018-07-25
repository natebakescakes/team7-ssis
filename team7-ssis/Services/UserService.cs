using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class UserService
    {
        ApplicationDbContext context;
        UserRepository userRepository;
        public UserService(ApplicationDbContext context)
        {
            this.context = context;
            this.userRepository = new UserRepository(context);
        }

        public List<ApplicationUser> FindUsersByDepartment(Department department)
        {
            if (department == null) return new List<ApplicationUser>();

            return userRepository.FindByDepartment(department).ToList();
        }

        public List<ApplicationUser> FindAllUsers()
        {
            return userRepository.FindAll().ToList();
        }

        public ApplicationUser FindUserByEmail(string email)
        {
            return userRepository.FindByEmail(email);
        }

        public List<ApplicationUser> FindSupervisorsByDepartment(Department department)
        {
            if (department == null) return new List<ApplicationUser>();

            return userRepository.FindSupervisorByDepartment(department).ToList();
        }

        public ApplicationUser Save(ApplicationUser user)
        {
            return userRepository.Save(user);
        }

        public List<string> FindRolesByEmail(string email)
        {
            if (!userRepository.ExistsByEmail(email))
                return new List<string>();

            var roleRepository = new RoleRepository(context);

            return FindUserByEmail(email).Roles
                .Select(role => roleRepository.FindById(role.RoleId).Name)
                .ToList();
        }
    }
}