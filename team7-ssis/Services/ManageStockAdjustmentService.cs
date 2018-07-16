using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class ManageStockAdjustmentService
    {
        ApplicationDbContext context;
    
        public ManageStockAdjustmentService(ApplicationDbContext context)
        {
            this.context = context;         
        }

        public List<Item> FindAllItems() 
        {
            ItemRepository i = new ItemRepository(context);
            return i.FindAll().ToList();
        }

        public List<Item> SelectItems(List<Item> items)
        {
            throw new NotImplementedException();

        }
        public void DeleteRecord()
        {
            throw new NotImplementedException(); 
        }

        public void CreateNewStockAdjustment(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();

        }

        public StockAdjustment SearchStockAdjustment(string input)
        {
            throw new NotImplementedException();
        }

        public StockAdjustment SearchStockAdjustment(Status status)
        {
            throw new NotImplementedException();

        }
        public List<Item> SearchItems(String input)
        {
            throw new NotImplementedException();
        }

        public List<Item> SearchItems(Supplier supplier)
        {
            throw new NotImplementedException();

        }










    }
}