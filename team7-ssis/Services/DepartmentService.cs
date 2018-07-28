using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class DepartmentService
    {
        ApplicationDbContext context;
        DepartmentRepository departmentRepository;
        UserRepository userRepository;

        public DepartmentService(ApplicationDbContext context)
        {
            this.context = context;
            this.departmentRepository = new DepartmentRepository(context);
            this.userRepository = new UserRepository(context);
        }

        //method not needed
        public Department FindDepartmentByUser(ApplicationUser user)
        {
            return departmentRepository.FindByUser(user);
        }

        public List<ApplicationUser> FindUsersByDepartment(Department department)
        {
            return userRepository.FindByDepartment(department).ToList();
        }
        public Department Save(Department department)
        {
            return departmentRepository.Save(department);
        }

        public List<Department> FindAllDepartments()
        {
            return departmentRepository.FindAll().ToList();
        }

        public Department FindDepartmentByDepartmentCode(string departmentCode)
        {
            return departmentRepository.FindById(departmentCode);
        }

        public void ChangeRepresentative(string representativeEmail, string headEmail)
        {
            if (userRepository.FindByEmail(representativeEmail).Department.Name != userRepository.FindByEmail(headEmail).Department.Name)
                throw new ArgumentException("Representative and Requestor not from same Department");

            if (!userRepository.FindByEmail(headEmail).Roles.Select(userRole => userRole.RoleId).Contains("2"))
                throw new ArgumentException("User does not have managerial rights");

            var department = this.FindDepartmentByUser(userRepository.FindByEmail(headEmail));
            department.Representative = userRepository.FindByEmail(representativeEmail);
            department.UpdatedBy = userRepository.FindByEmail(headEmail);
            department.UpdatedDateTime = DateTime.Now;

            this.Save(department);
        }

        
    }
}