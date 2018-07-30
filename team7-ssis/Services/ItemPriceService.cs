using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class ItemPriceService
    {
        ApplicationDbContext context;
        ItemPriceRepository itemPriceRepository;
        StatusRepository statusRepository;

        public ItemPriceService(ApplicationDbContext context)
        {
            this.context = context;
            itemPriceRepository = new ItemPriceRepository(context);
            statusRepository = new StatusRepository(context);
        }
        public string GetDefaultPrice(Item item, int priority)
        {
            ItemPrice i = itemPriceRepository.FindByItemCode(item.ItemCode).Where(x => x.PrioritySequence == priority).First();
            if (i != null)
                return i.Price.ToString();
            else
                return "";
        }
        

        public List<ItemPrice> FindAllItemPriceByOrder(string itemCode)
        {
            return itemPriceRepository.FindOrderBySequence(itemCode).ToList();
        }
        public List<ItemPrice> FindAllItemPrice()
        {
            return itemPriceRepository.FindAll().ToList();
        }

        public List<ItemPrice> FindItemPriceByItemCode(string itemCode)
        {
            return itemPriceRepository.FindByItemCode(itemCode).ToList();
        }

        public List<ItemPrice> FindItemPriceBySupplierCode(string supplierCode)
        {
            return itemPriceRepository.FindBySupplierCode(supplierCode).ToList();
        }

        public List<ItemPrice> FindItemPriceByPrioritySequence(int prioritySequence)
        {
            return itemPriceRepository.FindByPrioritySequence(prioritySequence).ToList();
        }

        public ItemPrice Save(ItemPrice itemPrice)
        {
            return itemPriceRepository.Save(itemPrice);
        }
        
        public void  DeleteItemPrice(ItemPrice itemPrice)
        {
            itemPriceRepository.Delete(itemPrice);
        }

        public ItemPrice FindOneByItemAndSequence(Item i,int priority)
        {
            return itemPriceRepository.findByItemAndSequence(i, priority).First();
        }
    }
}