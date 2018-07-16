using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class ItemService
    {
        ApplicationDbContext context;
        public ItemService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Item FindItemByItemCode(string itemcode)
        {
            throw new NotImplementedException();
        }

        public List<Item> FindAllItems()
        {
            throw new NotImplementedException();
        }

        public List<Supplier> FindSuppliersByItemCode(string itemcode)
        {
            throw new NotImplementedException();
        }

        public List<Item> FindItemsByCategory(ItemCategory itemCategory)
        {
            throw new NotImplementedException();
        }

        public Item SaveItem(Item item)
        {
            throw new NotImplementedException();
        }

        public List<Item> DeleteItem(Item item)
        {
            throw new NotImplementedException();
        }
        
        public List<Item> EditItem(Item item)
        {
            throw new NotImplementedException();
        }

        public List<ItemPrice> PrintItemPriceList()
        {
            throw new NotImplementedException();
        }




    }
}