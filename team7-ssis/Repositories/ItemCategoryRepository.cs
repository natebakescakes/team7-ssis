using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class ItemCategoryRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ItemCategory Save(ItemCategory deliveryOrder)
        {
            throw new NotImplementedException();
        }

        public ItemCategory FindById(int itemCategoryId)
        {
            throw new NotImplementedException();
        }

        public List<ItemCategory> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.ItemCategory.Count();
        }

        public void Delete(ItemCategory itemCategory)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int itemCategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
