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
            throw new NotImplementedException();
        }
        
        public List<ApplicationUser> FindUsersByDepartment(Department department)
        {
            return userRepository.FindAll().Where(x => x.Department.DepartmentCode == department.DepartmentCode).ToList();
        }
        public Department Save(Department department)
        {
            return departmentRepository.Save(department);
        }
    }
}