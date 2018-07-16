using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Services
{
    class StockAdjustmentService
    {
        ApplicationDbContext context;
        public StockAdjustmentService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void DeleteItem(StockAdjustment stockAdjustment, Item item)
        {
            throw new NotImplementedException();
        }


        //create new StockAdjustment with status pending
        public void CreateNewStockAdjustment(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();

        }

        public List<StockAdjustment>  FindAllStockAdjustment()
        {
            throw new NotImplementedException();
        }

        public List<StockAdjustment> FindAllStockAdjustmentById()
        {
            throw new NotImplementedException();
        }

        public void ApproveStockAdjustment(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();

        }

        public void RejectStockAdjustment(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();

        }
        public void ShowStockAdjustmentDetails(StockAdjustment stockadjustment)
        {
            throw new NotImplementedException();
        }

    }
}
