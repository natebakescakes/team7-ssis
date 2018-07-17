using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class ItemCategoryService
    {
        ApplicationDbContext context;

        public ItemCategoryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ItemCategory> FindAllItemCategory()
        {
            throw new NotImplementedException();
        }

        public ItemCategory FindItemCategoryByItemCategoryId(int itemCategoryId)
        {
            throw new NotImplementedException();
        }

        public ItemCategory Save(ItemCategory itemCategory)
        {
            throw new NotImplementedException();
        }

        public ItemCategory Delete(ItemCategory itemCategory)
        {
            throw new NotImplementedException();
        }

    }
}