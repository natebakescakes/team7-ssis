using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.ViewModels;

namespace team7_ssis.Services
{
    public class RetrievalService
    {
        ApplicationDbContext context;
        RetrievalRepository retrievalRepository;
        ItemService itemService;
        StockMovementService stockmovementService;
        StatusRepository statusRepository;

        public RetrievalService(ApplicationDbContext context)
        {
            this.context = context;
            retrievalRepository = new RetrievalRepository(context);
            itemService = new ItemService(context);
            stockmovementService = new StockMovementService(context);

            statusRepository = new StatusRepository(context);

        }

        public List<Retrieval> FindAllRetrievals()
        {
            return retrievalRepository.FindAll().ToList();
        }


        public Retrieval FindRetrievalById(string id)
        {
            return retrievalRepository.FindById(id);
        }
        public Retrieval Save(Retrieval retrieval)
        {
            //mapped to confirm retrieval, add and edit retrievals (if any) 
            return retrievalRepository.Save(retrieval);

        }

        public Retrieval RetrieveItems(Retrieval other)
        {
            // Get RetrieveByRetrievalId
            Retrieval retrieval = this.FindRetrievalById(other.RetrievalId);

            // Update Actual Quantity
            retrieval.Disbursements = other.Disbursements;

            // Save Retrieval
            this.Save(retrieval);

            // Update Item Quantity based on amount retrieved into Inventory
            foreach (Disbursement d in retrieval.Disbursements)
            {
                foreach (DisbursementDetail detail in d.DisbursementDetails)
                {

                    //Create Stock Movement Transaction
                    stockmovementService.CreateStockMovement(detail);
                }
            }



            return retrieval;

        }

        public void RetrieveItem(string retrievalId, string email, string itemCode)
        {
            if (!retrievalRepository.ExistsById(retrievalId))
                throw new ArgumentException("Retrieval does not exist");

            var retrieval = retrievalRepository.FindById(retrievalId);

            if (retrievalRepository.FindById(retrievalId).Disbursements.SelectMany(d => d.DisbursementDetails.Where(dd => dd.ItemCode == itemCode)).Any(dd => dd.Status.StatusId == 18))
                throw new ArgumentException("Item already retrieved");

            retrieval.Disbursements.SelectMany(d => d.DisbursementDetails.Where(dd => dd.ItemCode == itemCode)).ToList().ForEach(disbursementDetail =>
            {
                disbursementDetail.Status = new StatusService(context).FindStatusByStatusId(18);
            });
        }
        /// <summary>
        /// Sets Retrieval.Status to "Retrieved" if all DisbursementDetails are "Picked"
        /// </summary>
        /// <param name="retId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ConfirmRetrieval(string retId, string email)
        {
            Retrieval r = retrievalRepository.FindById(retId);
            // get list of Disbursement details
            List<DisbursementDetail> ddList = r.Disbursements.SelectMany(x => x.DisbursementDetails).ToList();
            // check if all of them are picked
            bool allPicked = ddList.TrueForAll(x => x.Status.StatusId == 18);
            if (allPicked)
            {
                r.Status = statusRepository.FindById(20);
            } else {
                r.Status = statusRepository.FindById(19);
            }
            retrievalRepository.Save(r);

            return allPicked;
        }

        public void UpdateActualQuantity(string retrievalId, string email, string itemCode, List<BreakdownByDepartment> retrievalDetails)
        {
            if (!retrievalRepository.ExistsById(retrievalId))
                throw new ArgumentException("Retrieval does not exist");

            foreach (BreakdownByDepartment breakdown in retrievalDetails)
            {
                var disbursement = FindRetrievalById(retrievalId).Disbursements.Where(d => d.Department.DepartmentCode == breakdown.DeptId).FirstOrDefault();

                new DisbursementService(context).UpdateActualQuantityForDisbursementDetail(disbursement.DisbursementId, itemCode, breakdown.Actual, email);
            }
        }

        
    }
}