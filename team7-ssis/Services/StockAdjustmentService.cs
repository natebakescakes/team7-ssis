﻿using System;
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
        StatusRepository statusRepository;
        public StockAdjustmentService(ApplicationDbContext context)
        {
            this.context = context;
            this.statusRepository = new StatusRepository(context);
            this.stockAdjustmentRepository = new StockAdjustmentRepository(context);
           this.stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
        }

        //create new StockAdjustment with status: draft
        public StockAdjustment CreateDraftStockAdjustment(StockAdjustment stockadjustment)
        {
            //controller pass stockadjustment to the method
            stockadjustment.Status = statusRepository.FindById(3);
            stockAdjustmentRepository.Save(stockadjustment);
            return stockadjustment;
        }

        //Delete one item if StockAdjustment in Draft Status
        public string DeleteItemFromDraftStockAdjustment(string stockadjustment_id, string itemcode)
        {
            //controller pass stockadjustmentid and itemcode to the method
            StockAdjustment s1 = stockAdjustmentRepository.FindById(stockadjustment_id);
            StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode);
            if (s1.Status.StatusId==3)
            {
               //remove one StockAdjustmentDetail in List<StockAdjustmentDetail>
                s1.StockAdjustmentDetails.Remove(s);
                //delete one stockadjustmentdetail
                stockAdjustmentDetailRepository.Delete(s);
            }
            return itemcode;

        }

        //Delete whole StockAdjustment in Draft Status
        public string DeleteDraftStockAdjustment(string id)
        {
            //controller pass stockadjustmentid the method
            StockAdjustment stockAdjustment = stockAdjustmentRepository.FindById(id);
       
            if (stockAdjustment.Status.StatusId == 3)
            {
                stockAdjustmentRepository.Delete(stockAdjustment);
            }
            return stockAdjustment.StockAdjustmentId;


        }

        //create new StockAdjustment with status: pending
        public StockAdjustment CreatePendingStockAdjustment(StockAdjustment stockadjustment)
            //controller pass stockadjustment to the method
        {
                stockadjustment.Status = statusRepository.FindById(4);
                stockAdjustmentRepository.Save(stockadjustment);
                return stockadjustment;

        }

        //cancel pening stockadjustment before being approved/rejected
        public StockAdjustment CancelPendingStockAdjustment(string id)
        {
            //controller pass stockadjustmentid to the method
            StockAdjustment stockadjustment = stockAdjustmentRepository.FindById(id);
            stockadjustment.Status = statusRepository.FindById(2);
            stockAdjustmentRepository.Save(stockadjustment);
            return stockadjustment;

        }


        //find all stockadjustemnt
        public List<StockAdjustment> FindAllStockAdjustment()
        {
            return stockAdjustmentRepository.FindAll().ToList();
        }

        //find stockadjustment by stockjustmentid
        public StockAdjustment FindStockAdjustmentById(string id)
        {
            return stockAdjustmentRepository.FindById(id);
            
        }

        //approve pending stockadjustment
        public StockAdjustment ApproveStockAdjustment(string id)
        {
            //controller pass stockadjustmentid to the method
            StockAdjustment stockadjustment = stockAdjustmentRepository.FindById(id);
            if (stockadjustment.Status.StatusId==4)
            {
                stockadjustment.Status = statusRepository.FindById(6);
                stockAdjustmentRepository.Save(stockadjustment);
            }
            return stockadjustment;
        }

        //reject pending stockadjustment
        public StockAdjustment RejectStockAdjustment(string id)
        {
            //controller pass stockadjustmentid to the method
            StockAdjustment stockadjustment = stockAdjustmentRepository.FindById(id);
            if (stockadjustment.Status.StatusId == 4)
            {
                stockadjustment.Status=statusRepository.FindById(5);
                stockAdjustmentRepository.Save(stockadjustment);
            }
            return stockadjustment;
        }

        // show sepcific StockAdjustmentDetail in the StockAdjustment
        public StockAdjustmentDetail ShowStockAdjustmentDetail(string stockadjustment_id, string itemcode)
        {
            
            StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode);
            return s;
        }

    }
}
