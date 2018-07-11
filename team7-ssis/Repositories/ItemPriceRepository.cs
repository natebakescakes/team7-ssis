using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class ItemPriceRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ItemPrice Save(ItemPrice itemPrice)
        {
            throw new NotImplementedException();
        }

        public ItemPrice FindByIds(string itemCode, string supplierCode)
        {
            throw new NotImplementedException();
        }

        public List<ItemPrice> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.ItemPrice.Count();
        }

        public void Delete(ItemPrice itemPrice)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string itemCode, string supplierCode)
        {
            throw new NotImplementedException();
        }
    }
}
