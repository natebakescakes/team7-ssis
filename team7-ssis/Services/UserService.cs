using Microsoft.AspNet.Identity.EntityFramework;
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

            if (department.DepartmentCode == "STOR")
                return userRepository.FindByDepartment(department).Where(user => user.Roles.Select(r => r.RoleId).Contains("4")).ToList();

            return userRepository.FindSupervisorByDepartment(department).ToList();
        }

        public ApplicationUser FindRepresentativeByDepartment(Department department)
        {
            return userRepository.FindRepByDepartment(department).FirstOrDefault();
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

        public void AddDepartmentHeadRole(string email)
        {
            if (FindRolesByEmail(email).Contains("DepartmentHead"))
                throw new ArgumentException("User already has Department Head role");

            var user = FindUserByEmail(email);
            var role = new IdentityUserRole()
            {
                UserId = user.Id,
                RoleId = "2",
            };
            user.Roles.Add(role);
            userRepository.Save(user);
        }

        public void RemoveDepartmentHeadRole(string email)
        {
            if (!FindRolesByEmail(email).Contains("DepartmentHead"))
                throw new ArgumentException("User does not have Department Head role");

            var user = FindUserByEmail(email);
            user.Roles.Remove(user.Roles.Where(userRole => userRole.RoleId == "2").FirstOrDefault());
            context.SaveChanges();
        }
    }
}