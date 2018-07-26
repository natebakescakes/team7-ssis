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
            return i.Price.ToString();
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

        public List<ItemPrice> DeleteItemPrice(ItemPrice itemPrice)
        {
           List<ItemPrice> p = itemPriceRepository.FindByItemCode(itemPrice.ItemCode).ToList();
            List<ItemPrice> q = new List<ItemPrice>();
            foreach (ItemPrice element in p)
            {
                element.Status = statusRepository.FindById(0);
                q.Add(itemPriceRepository.Save(element));
            }
            return q;
        }

        public ItemPrice FindSingleItemPriceByPriority(Item item, int priority)
        {
            ItemPrice i = itemPriceRepository.FindByItemCode(item.ItemCode).Where(x => x.PrioritySequence == priority).First();
            return i;
        }
    }
}