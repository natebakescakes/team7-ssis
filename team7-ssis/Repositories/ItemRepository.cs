using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class ItemRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public Item Save(Item item)
        {
            throw new NotImplementedException();
        }

        public Item FindById(string itemCode)
        {
            throw new NotImplementedException();
        }

        public List<Item> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.Item.Count();
        }

        public void Delete(Item item)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string itemCode)
        {
            throw new NotImplementedException();
        }
    }
}
