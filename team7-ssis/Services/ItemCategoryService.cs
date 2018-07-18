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
        ItemCategoryRepository itemCategoryRepository;
        StatusRepository statusRepository;

        public ItemCategoryService(ApplicationDbContext context)
        {
            this.context = context;
            itemCategoryRepository = new ItemCategoryRepository(context);
            statusRepository = new StatusRepository(context);
        }

        public List<ItemCategory> FindAllItemCategory()
        {
            return itemCategoryRepository.FindAll().ToList();
        }

        public ItemCategory FindItemCategoryByItemCategoryId(int itemCategoryId)
        {
            return itemCategoryRepository.FindById(itemCategoryId);
        }

        public ItemCategory Save(ItemCategory itemCategory)
        {
            return itemCategoryRepository.Save(itemCategory);
        }

        public ItemCategory Delete(ItemCategory itemCategory)
        {
            itemCategory.Status = statusRepository.FindById(0);
            return itemCategoryRepository.Save(itemCategory);
        }

    }
}