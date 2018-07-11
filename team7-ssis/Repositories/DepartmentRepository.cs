using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class DepartmentRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public Department Save(Department deliveryOrder)
        {
            throw new NotImplementedException();
        }

        public Department FindById(string departmentCode)
        {
            throw new NotImplementedException();
        }

        public List<Department> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Department.Count();
        }

        public void Delete(Department department)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string departmentCode)
        {
            throw new NotImplementedException();
        }
    }
}
