using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public  class ItemService
    {
        static ApplicationDbContext context;
        static ItemRepository itemRepository;
        static StatusRepository statusRepository;

        public  ItemService(ApplicationDbContext context)
        {
            context = context;
            itemRepository = new ItemRepository(context);
            statusRepository = new StatusRepository(context);
        }

        public static Item FindItemByItemCode(string itemCode)
        {
            return itemRepository.FindById(itemCode);
        }

        public static List<Item> FindAllItems()
        {
            return itemRepository.FindAll().ToList();
        }


        public static List<Item> FindItemsByCategory(ItemCategory itemCategory)
        {
            return itemRepository.FindItemsByCategory(itemCategory).ToList();
        }

        public static Item Save(Item item)
        {
            return itemRepository.Save(item);
        }

        public static Item DeleteItem(Item item)
        {
            item.Status= statusRepository.FindById(0);
            return itemRepository.Save(item);
        }
        
    }
}