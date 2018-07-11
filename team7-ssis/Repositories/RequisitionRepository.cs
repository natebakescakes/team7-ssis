using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class RequisitionRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public Requisition Save(Requisition requisition)
        {
            throw new NotImplementedException();
        }

        public Requisition FindById(int requisitionId)
        {
            throw new NotImplementedException();
        }

        public List<Requisition> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Requisition.Count();
        }

        public void Delete(Requisition requisition)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int requisitionId)
        {
            throw new NotImplementedException();
        }
    }
}
