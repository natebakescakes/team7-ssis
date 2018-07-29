using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    public class StockAdjustmentService
    {
        ApplicationDbContext context;
        StockAdjustmentRepository stockAdjustmentRepository;
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        StatusRepository statusRepository;
        ItemService itemService;
        InventoryRepository inventoryRepository;
        StockMovementRepository stockMovementRepository;
        StockMovementService stockMovementService;

        public StockAdjustmentService(ApplicationDbContext context)
        {
            this.context = context;
            this.statusRepository = new StatusRepository(context);
            this.stockAdjustmentRepository = new StockAdjustmentRepository(context);
            this.stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
            this.itemService = new ItemService(context);
            this.inventoryRepository = new InventoryRepository(context);
            this.stockMovementRepository = new StockMovementRepository(context);
            this.stockMovementService = new StockMovementService(context);
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
        public string DeleteItemFromDraftOrPendingStockAdjustment(string stockadjustment_id, string itemcode)
        {
            //controller pass stockadjustmentid and itemcode to the method
            StockAdjustment s1 = stockAdjustmentRepository.FindById(stockadjustment_id);
            StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode);
            if (stockAdjustmentRepository.FindById(stockadjustment_id) == null)
            {
                throw new Exception("can't find StockAdjustment");
            }
            else if (stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode) == null)
            {
                throw new Exception("can't find stockAdjustmentDetail");
            }
           
            if (s1.Status.StatusId==3 || s1.Status.StatusId==4)
            {
               //remove one StockAdjustmentDetail in List<StockAdjustmentDetail>
                s1.StockAdjustmentDetails.Remove(s);
                //delete one stockadjustmentdetail
                stockAdjustmentDetailRepository.Delete(s);
                stockAdjustmentRepository.Save(s1);
            }
            return itemcode;
            

        }

        //Cancel StockAdjustment in Draft or Pending Status
        public StockAdjustment CancelDraftOrPendingStockAdjustment(string id)
        {
            //controller pass stockadjustmentid the method
            StockAdjustment stockAdjustment = stockAdjustmentRepository.FindById(id);
            if(stockAdjustmentRepository.FindById(id)==null)
            {
                throw new Exception("can't find the StockAdjustment");
            }
       
            if (stockAdjustment.Status.StatusId == 3 || stockAdjustment.Status.StatusId==4)
            {
                stockAdjustment.Status = statusRepository.FindById(2);
                //if(statusRepository.FindById(2)==null)
                //{
                //    throw new Exception("can't find such status");
                //}
                stockAdjustmentRepository.Save(stockAdjustment);
                
            }
            return stockAdjustment;
        }



        //update stockadjustment to pending
        public StockAdjustment updateToPendingStockAdjustment(StockAdjustment s)
        {

            s.Status = statusRepository.FindById(4);
            return stockAdjustmentRepository.Save(s);

        }

        //update stockadjustment to draft
        public StockAdjustment updateToDraftStockAdjustment(StockAdjustment s)
        {

            s.Status = statusRepository.FindById(3);
            return stockAdjustmentRepository.Save(s);

        }

       


        //create new StockAdjustment with status: pending
        public StockAdjustment CreatePendingStockAdjustment(StockAdjustment stockadjustment)
            //controller pass stockadjustment to the method
        {
         
            stockadjustment.Status = statusRepository.FindById(4);
            stockAdjustmentRepository.Save(stockadjustment);
                return stockadjustment;

        }


        //find all stockadjustemnt
        public List<StockAdjustment> FindAllStockAdjustment()
        {
            return stockAdjustmentRepository.FindAll().ToList();
            
        }


        //find stockadjustments except draft

        public List<StockAdjustment> FindAllStockAdjustmentExceptDraft()
        {
            return stockAdjustmentRepository.FindAll().Where(x => x.Status.StatusId != 3 ).Where(x => x.Status.StatusId != 2).ToList();
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
            if(stockAdjustmentRepository.FindById(id)==null)
            {
                throw new Exception("can't find StockAdjustment");
            }
            StockAdjustment stockadjustment = stockAdjustmentRepository.FindById(id);
            if (stockadjustment.Status.StatusId==4)
            {
                stockadjustment.Status = statusRepository.FindById(6);
                stockAdjustmentRepository.Save(stockadjustment);
                //update item inventory
                foreach (StockAdjustmentDetail sd in stockadjustment.StockAdjustmentDetails)
                {
                    stockMovementService.CreateStockMovement(sd);
                }

            }
            return stockadjustment;
        }

        //reject pending stockadjustment
        public StockAdjustment RejectStockAdjustment(string id)
        {
            //controller pass stockadjustmentid to the method
            if (stockAdjustmentRepository.FindById(id) == null)
            {
                throw new Exception("can't find StockAdjustment");
            }
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
            if(stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode)==null)
            {
                throw new Exception("can't find stockAdjustmentDetail");
            }
            StockAdjustmentDetail s = stockAdjustmentDetailRepository.FindById(stockadjustment_id, itemcode);
            return s;
        }

        public StockAdjustment updateStockAdjustment(StockAdjustment sa)
        {
            stockAdjustmentRepository.Save(sa);
            return sa;

        }

        public StockAdjustmentDetail updateStockAdjustmentDetail(StockAdjustmentDetail sd)
        {
            stockAdjustmentDetailRepository.Save(sd);
            return sd;
        }

        public StockAdjustmentDetail findStockAdjustmentDetailById(string id,string itemcode)
        {
            return stockAdjustmentDetailRepository.FindById(id, itemcode);
        }



    }
}
