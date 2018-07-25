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
        ItemService itemService;

        public StockMovementService(ApplicationDbContext context)
        {
            this.context = context;
            stockmovementRepository = new StockMovementRepository(context);
            itemService = new ItemService(context);
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

        private StockMovement InstantiateStockMovement()
        {
            StockMovement sm = new StockMovement();
            sm.StockMovementId = IdService.GetNewStockMovementId(context);
            sm.CreatedDateTime = DateTime.Now;

            return sm;
        }
        public StockMovement CreateStockMovement(DisbursementDetail detail)
        {
            StockMovement sm = this.InstantiateStockMovement();
            sm.DisbursementDetail = detail;
            sm.Item = detail.Item;
            sm.OriginalQuantity = detail.Item.Inventory.Quantity;
            sm.AfterQuantity =sm.OriginalQuantity - detail.ActualQuantity;

            //Update inventory quantity
            itemService.UpdateQuantity(sm.Item, sm.AfterQuantity);

            return this.Save(sm);
        }

        public StockMovement CreateStockMovement(DeliveryOrderDetail detail)
        {
            StockMovement sm = this.InstantiateStockMovement();
            sm.DeliveryOrderDetail = detail;
            sm.Item = detail.Item;
            sm.OriginalQuantity = detail.Item.Inventory.Quantity;
            sm.AfterQuantity = sm.OriginalQuantity + detail.ActualQuantity;

            //Update inventory quantity
            itemService.UpdateQuantity(sm.Item, sm.AfterQuantity);

            return this.Save(sm);
        }

        public StockMovement CreateStockMovement(StockAdjustmentDetail detail)
        {
            StockMovement sm = this.InstantiateStockMovement();
            sm.StockAdjustmentDetail = detail;
            sm.Item = detail.Item;
            sm.OriginalQuantity = detail.Item.Inventory.Quantity; //should be same as detail.OriginalQuantity
            sm.AfterQuantity = detail.AfterQuantity;

            //Update inventory quantity
            itemService.UpdateQuantity(sm.Item, sm.AfterQuantity);
            
            return this.Save(sm);
        }


    }
}