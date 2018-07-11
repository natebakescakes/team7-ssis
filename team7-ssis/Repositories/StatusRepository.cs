using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class StatusRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public Status Save(Status status)
        {
            throw new NotImplementedException();
        }

        public Status FindById(int statusId)
        {
            throw new NotImplementedException();
        }

        public List<Status> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Status.Count();
        }

        public void Delete(Status status)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int statusId)
        {
            throw new NotImplementedException();
        }
    }
}
