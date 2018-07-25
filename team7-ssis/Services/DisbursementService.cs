using System;
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
        StatusRepository statusRepository;
        ItemService itemService;
        StockMovementService stockMovementService;


        public DisbursementService(ApplicationDbContext context)
        {
            this.context = context;
            disbursementRepository = new DisbursementRepository(context);
            disbursementDetailRepository = new DisbursementDetailRepository(context);
            statusRepository = new StatusRepository(context);
            itemService = new ItemService(context);
            stockMovementService = new StockMovementService(context);
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
                      
            //get the disbursement object
            Disbursement disbursement = this.FindDisbursementById(DisbursementId);

            //update status of the disbursement to Items collected
            disbursement.Status = statusRepository.FindById(10);
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

    }
}