using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;


namespace team7_ssis.Services
{
    public class StockMovementService
    {
        ApplicationDbContext context;

        public StockMovementService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public List<StockMovement> FindAllStockMovementService()
        {
            throw new NotImplementedException();
        }

        public List<StockMovement> FindStockMovementByItemCode(string itemCode)
        {
            throw new NotImplementedException();
        }

        public List<StockMovement> FindStockMovementByDisbursementId(int disbursementId)
        {
            throw new NotImplementedException();
        }

        public List<StockMovement> FindStockMovementByStockAdjustmentId(int stockAdjustmentId)
        {
            throw new NotImplementedException();
        }

        public StockMovement Save(StockMovement stockMovement)
        {
            throw new NotImplementedException();
        }
    }
}