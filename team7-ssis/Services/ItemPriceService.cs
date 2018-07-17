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

        public List<ItemPrice> FindAllItemPrice()
        {
            return itemPriceRepository.FindAll().ToList();
        }

        public ItemPrice FindItemPriceByItemCode(string itemCode)
        {
            return (ItemPrice)itemPriceRepository.FindByItemCode(itemCode);
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

        public void DeleteItemPrice(ItemPrice itemPrice)
        {
            itemPrice.Status = statusRepository.FindById(0);
            itemPriceRepository.Save(itemPrice);
        }
    }
}