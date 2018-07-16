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

        public void  DeleteItem(StockAdjustment stockAdjustment, Item item)
        {
            throw new NotImplementedException(); 
        }
        
      
        public void CreateNewStockAdjustment(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();

        }

    
    }
}
