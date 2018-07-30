﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class DisbursementService
    {
        ApplicationDbContext context;
        DisbursementRepository disbursementRepository;
        DisbursementDetailRepository disbursementDetailRepository;
        ItemService itemService;
        StockMovementService stockMovementService;
        StatusService statusService;
        RequisitionRepository requisitionRepository;
      

        public DisbursementService(ApplicationDbContext context)
        {
            this.context = context;
            disbursementRepository = new DisbursementRepository(context);
            disbursementDetailRepository = new DisbursementDetailRepository(context);
            itemService = new ItemService(context);
            stockMovementService = new StockMovementService(context);
            statusService = new StatusService(context);
            requisitionRepository = new RequisitionRepository(context);
            

        }

        public List<Disbursement> FindAllDisbursements()
        {
            return disbursementRepository.FindAll().ToList();
        }

        public Disbursement FindDisbursementById(string id)
        {
            return disbursementRepository.FindById(id);
        }

        public Disbursement Save(Disbursement disbursement)
        {

            //Edit Disbursement and confirmDeliveryStatus and also delete
            return disbursementRepository.Save(disbursement);

        }

        public void Save(List<Disbursement> disbursements)
        {

            //Edit Disbursement and delete
            foreach(var a in disbursements)
            {
                this.Save(a);
            }
            
        }

        public Disbursement ConfirmCollection(string DisbursementId)
        {
            // Throw error if Disbursement already collected
            if (disbursementRepository.FindById(DisbursementId).Status == new StatusService(context).FindStatusByStatusId(10))
                throw new ArgumentException("Items already collected!");

            //get the disbursement object
            Disbursement disbursement = this.FindDisbursementById(DisbursementId);

            //update status of the disbursement to Items collected
            disbursement.Status = statusService.FindStatusByStatusId(10);
            disbursement.CollectedDateTime = DateTime.Now;
            disbursement.CollectedBy = disbursement.Retrieval.Requisitions.First().CreatedBy;

          
            return this.Save(disbursement);

         }

        public List<Disbursement> FindDisbursementsByRetrievalId(string Rid)
        {

            return disbursementRepository.FindByRetrievalId(Rid).ToList();

        }

        public Disbursement UpdateActualQuantityForDisbursementDetail(string DisbursementId, string ItemCode, int quantity)
        {
            Disbursement disbursement = this.FindDisbursementById(DisbursementId);
            disbursement.DisbursementDetails.Find(x => x.ItemCode == ItemCode).ActualQuantity = quantity;

            return this.Save(disbursement);
        }

        public List<Requisition> UpdateRequisitionStatus(Disbursement disbursement)
        {
            int count = 0;
            //get list of requisitions with the same retrieval id as the disbursement
            List<Requisition> requisitions = disbursement.Retrieval.Requisitions;
            List<Requisition> updated = new List<Requisition>();

            foreach (DisbursementDetail d in disbursement.DisbursementDetails)
            {
                foreach (Requisition requisition in requisitions)
                {
                    count = 0;
                    RequisitionDetail r = requisition.RequisitionDetails.Find(x => x.Item == d.Item);
                  
                        if (r.Item == d.Item)
                        {
                            if (r.Quantity > d.ActualQuantity)
                            {
                                //set requisition detail status to be partially fulfilled IF NOT ZERO
                                r.Status = statusService.FindStatusByStatusId(9);
                                
                            }

                            else if (r.Quantity < d.ActualQuantity)
                            {
                                //set requisition detail status to be fully delivered
                                r.Status = statusService.FindStatusByStatusId(10);
                                d.ActualQuantity -= r.Quantity;
                                count++;
                            }


                        }

                        if(count > 0 )
                        {
                            requisition.Status = statusService.FindStatusByStatusId(9);
                        }
                        if (count == requisition.RequisitionDetails.Count)
                        {
                            requisition.Status = statusService.FindStatusByStatusId(10);
                        }
                       

                    updated.Add(requisitionRepository.Save(requisition));
                }
            }

            return updated;
        }

    }
}