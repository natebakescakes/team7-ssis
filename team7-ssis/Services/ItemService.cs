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
        ApplicationDbContext context;
        ItemRepository itemRepository;
        StatusRepository statusRepository;

        public ItemService(ApplicationDbContext context)
        {
            this.context = context;
            itemRepository = new ItemRepository(context);
            statusRepository = new StatusRepository(context);
        }

        public Item FindItemByItemCode(string itemCode)
        {
            return itemRepository.FindById(itemCode);
        }

        public  List<Item> FindAllItems()
        {
            return itemRepository.FindAll().ToList();
        }


        public List<Item> FindItemsByCategory(ItemCategory itemCategory)
        {
            return itemRepository.FindByCategory(itemCategory).ToList();
        }

        public Item Save(Item item)
        {
            return itemRepository.Save(item);
        }

        public Item DeleteItem(Item item)
        {
            item.Status= statusRepository.FindById(0);
            return itemRepository.Save(item);
        }
        
    }
}