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

        public ItemPriceService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<ItemPrice> FindAllItemPrice()
        {
            throw new NotImplementedException();
        }

        public ItemPrice FindItemPriceByItemCode(string itemCode)
        {
            throw new NotImplementedException();
        }

        public List<ItemPrice> FindItemPriceBySupplierCode(string supplierCode)
        {
            throw new NotImplementedException();
        }

        public List<ItemPrice> FindItemPriceByPrioritySequence(int prioritySequence)
        {
            throw new NotImplementedException();
        }

        public ItemPrice Save(ItemPrice itemPrice)
        {
            throw new NotImplementedException();
        }

        public void DeleteItemPrice(ItemPrice itemPrice)
        {
            throw new NotImplementedException();
        }
    }
}