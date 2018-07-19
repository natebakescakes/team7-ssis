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
        StockMovementRepository stockmovementRepository;

        public StockMovementService(ApplicationDbContext context)
        {
            this.context = context;
            stockmovementRepository = new StockMovementRepository(context);
        }

        public List<StockMovement> FindAllStockMovement()
        {
            return stockmovementRepository.FindAll().ToList();
        }

        public List<StockMovement> FindStockMovementByItemCode(string itemCode)
        {
            return stockmovementRepository.FindByItemCode(itemCode).ToList();
        }

        public List<StockMovement> FindStockMovementByDisbursementId(string disbursementId)
        {
            return stockmovementRepository.FindByDisbursementId(disbursementId).ToList();
        }

        public List<StockMovement> FindStockMovementByStockAdjustmentId(string stockAdjustmentId)
        {
            return stockmovementRepository.FindByStockAdjustmentId(stockAdjustmentId).ToList();
        }

        public StockMovement Save(StockMovement stockMovement)
        {
            return stockmovementRepository.Save(stockMovement);
        }
    }
}