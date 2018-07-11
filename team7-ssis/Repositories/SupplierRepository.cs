using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class SupplierRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public Supplier Save(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Supplier FindById(string supplierCode)
        {
            throw new NotImplementedException();
        }

        public List<Supplier> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Supplier.Count();
        }

        public void Delete(Supplier Supplier)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string supplierCode)
        {
            throw new NotImplementedException();
        }
    }
}
