using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Services
{
   public class StockAdjustmentService
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        public StockAdjustmentService(ApplicationDbContext context)
        {
            this.context = context;
            this.stockAdjustmentRepository = new StockAdjustmentRepository(context);
           this.stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
        }

        //create new StockAdjustment with status: draft
        public void CreateDraftStockAdjustment(StockAdjustment stockadjustment)
        {
            StockAdjustment st = stockadjustment;
            st.Status.StatusId = 3;
            stockAdjustmentRepository.Save(st);
        }

        //Delete one item if StockAdjustment in Draft Status
        public void DeleteItemFromDraftStockAdjustment(StockAdjustment stockAdjustment, Item item)
        {
            string stockadjustment_id = stockAdjustment.StockAdjustmentId;
            string itemcode = item.ItemCode;
            if (stockAdjustment.Status.StatusId==3)
            {
                StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id,itemcode);
                //remove one StockAdjustmentDetail in List<StockAdjustmentDetail>
                stockAdjustment.StockAdjustmentDetails.Remove(s);
                //delete one stockadjustmentdetail
                stockAdjustmentDetailRepository.Delete(s);
            }

        }

        //Delete whole StockAdjustment in Draft Status
        public void DeleteDraftStockAdjustment(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment.Status.StatusId == 3)
            {
                stockAdjustmentRepository.Delete(stockAdjustment);
            }
      
        }

        //create new StockAdjustment with status: pending
        public void CreatePendingStockAdjustment(StockAdjustment stockadjustment)
        {
            stockadjustment.Status.StatusId = 4;
            stockAdjustmentRepository.Save(stockadjustment);

        }

        //cancel pening stockadjustment before being approved/rejected
        public void CancelPendingStockAdjustment(StockAdjustment stockadjustment)
        {
            stockadjustment.Status.StatusId = 2;
            stockAdjustmentRepository.Save(stockadjustment);

        }



        //find all stockadjustemnt
        public List<StockAdjustment>  FindAllStockAdjustment()
        {
            return stockAdjustmentRepository.FindAll().ToList();
        }

        //find stockadjustment by stockjustmentid
        public StockAdjustment FindStockAdjustmentById(string id)
        {
            return stockAdjustmentRepository.FindById(id);
            
        }

        //approve pending stockadjustment
        public void ApproveStockAdjustment(StockAdjustment stockadjustment)
        {
            if(stockadjustment.Status.StatusId==4)
            {
                stockadjustment.Status.StatusId = 6;
                stockAdjustmentRepository.Save(stockadjustment);
            }

        }

        //reject pending stockadjustment
        public void RejectStockAdjustment(StockAdjustment stockadjustment)
        {
            if (stockadjustment.Status.StatusId == 4)
            {
                stockadjustment.Status.StatusId = 5;
                stockAdjustmentRepository.Save(stockadjustment);
            }
        }

        // show sepcific StockAdjustmentDetail in the StockAdjustment
        public StockAdjustmentDetail ShowStockAdjustmentDetail(StockAdjustment stockadjustment,Item item)
        {
            string stockadjustment_id = stockadjustment.StockAdjustmentId;
            string itemcode = item.ItemCode;
            StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode);
            return s;
        }

    }
}
