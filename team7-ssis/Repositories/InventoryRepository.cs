using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class InventoryRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public Inventory Save(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        public Inventory FindById(string itemCode)
        {
            throw new NotImplementedException();
        }

        public List<Inventory> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Inventory.Count();
        }

        public void Delete(Inventory inventory)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string itemCode)
        {
            throw new NotImplementedException();
        }
    }
}
