using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class ItemRepository : CrudRepository<Item, String>
    {
        public ItemRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Item;
        }

        public IQueryable<Item> FindByCategory(ItemCategory itemCategory)
        {
            return context.Item
                .Where(x => x.ItemCategory.ItemCategoryId == itemCategory.ItemCategoryId);
        }
    }
}
