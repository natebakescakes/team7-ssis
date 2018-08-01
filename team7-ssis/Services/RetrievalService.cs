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
        StatusRepository statusRepository;
        DisbursementRepository disbursementRepository;

        ItemService itemService;
        StockMovementService stockmovementService;
        DisbursementService disbursementService;


        public RetrievalService(ApplicationDbContext context)
        {
            this.context = context;
            retrievalRepository = new RetrievalRepository(context);
            statusRepository = new StatusRepository(context);
            disbursementRepository = new DisbursementRepository(context);

            itemService = new ItemService(context);
            stockmovementService = new StockMovementService(context);
            disbursementService = new DisbursementService(context);
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

            retrievalRepository.Save(retrieval);
        }
        /// <summary>
        /// Sets Retrieval.Status to "Retrieved".
        /// Creates outstanding requisitions for undisbursed items.
        /// </summary>
        /// <param name="retrievalId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public void ConfirmRetrieval(string retrievalId, string email)
        {
            // Throw exceptions
            if (!retrievalRepository.ExistsById(retrievalId))
                throw new ArgumentException("Retrieval does not exist");

            var retrieval = retrievalRepository.FindById(retrievalId);

            if (retrieval.Status.StatusId == 20)
                throw new ArgumentException("Retrieval already confirmed");

            // Update Retrieval status
            retrieval.Status = new StatusService(context).FindStatusByStatusId(20);
            retrieval.UpdatedBy = new UserService(context).FindUserByEmail(email);
            retrieval.UpdatedDateTime = DateTime.Now;

            // Create outstanding requisitions
            foreach (var disbursement in retrieval.Disbursements)
            {
                disbursement.Status = statusRepository.FindById(8);

                bool outstandingRequisition = true;

                var newRequisitionDetails = new List<RequisitionDetail>();
                disbursement.DisbursementDetails.ForEach(disbursementDetail =>
                {
                    // Get total requisited quantity
                    var totalRequisitionQuantity = disbursement.Retrieval.Requisitions
                        .Sum(r => r.RequisitionDetails
                            .Where(rr => rr.ItemCode == disbursementDetail.ItemCode)
                            .Sum(rr => rr.Quantity));

                    // Get actual quantity disbursed
                    var totalDisbursedQuantity = disbursementDetail.ActualQuantity;

                    if (totalDisbursedQuantity == totalRequisitionQuantity)
                    {
                        outstandingRequisition = false;
                        return;
                    }

                    var newRequisitionDetail = new RequisitionDetail()
                    {
                        RequisitionId = IdService.GetNewAutoGenerateRequisitionId(context),
                        Item = disbursementDetail.Item,
                        ItemCode = disbursementDetail.ItemCode,
                        Quantity = totalRequisitionQuantity - totalDisbursedQuantity,
                        Status = new StatusService(context).FindStatusByStatusId(3),
                    };

                    newRequisitionDetails.Add(newRequisitionDetail);

                    // Change requisition detail status if no actual quantity disbursed
                    if (totalDisbursedQuantity == 0)
                    {
                        retrieval.Requisitions.SelectMany(requisition => requisition.RequisitionDetails.Where(detail => detail.ItemCode == disbursementDetail.ItemCode)).ToList().ForEach(detail =>
                        {
                            detail.Status = statusRepository.FindById(21);
                        });
                    }
                });

                // Create new outstanding requisition
                var newRequisition = new Requisition()
                {
                    RequisitionId = IdService.GetNewAutoGenerateRequisitionId(context),
                    Department = disbursement.Department,
                    CollectionPoint = disbursement.Department.CollectionPoint,
                    CreatedBy = new UserService(context).FindUserByEmail("root@admin.com"),
                    CreatedDateTime = DateTime.Now,
                    Status = new StatusService(context).FindStatusByStatusId(3),
                    EmployeeRemarks = $"Automatically generated by system based on outstanding quantity - {retrievalId}",
                    RequisitionDetails = newRequisitionDetails,
                };

                if (outstandingRequisition)
                    new RequisitionRepository(context).Save(newRequisition);
            }

            // Update all requisition statuses that are not Unable to Fulfilled
            foreach (var requisition in retrieval.Requisitions)
            {
                if (requisition.Status.StatusId != 21)
                    requisition.Status = statusRepository.FindById(8);

                foreach (var detail in requisition.RequisitionDetails)
                {
                    if (detail.Status.StatusId != 21)
                        detail.Status = statusRepository.FindById(8);
                }
            }

            retrievalRepository.Save(retrieval);

            // Create Notification
            retrieval.Requisitions.ForEach(r => new NotificationService(context).CreateNotification(r, r.CreatedBy));
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

        public void SaveRetrieval(StationeryRetrievalJSONViewModel json)
        {
            List<Disbursement> disbList = disbursementService.FindDisbursementsByRetrievalId(json.RetrievalID);
            List<DisbursementDetail> ddList = disbList.SelectMany(x => x.DisbursementDetails).ToList();
            foreach (var row in json.Data)
            {
                if (row.RetrievedStatus == "Picked")
                {
                    ddList
                    .Where(x => x.ItemCode == row.ProductID)
                    .ToList()
                    .ForEach(x => x.ActualQuantity = x.PlanQuantity);
                }
                else if (row.RetrievedStatus == "Awaiting Picking")
                {
                    ddList
                    .Where(x => x.ItemCode == row.ProductID)
                    .ToList()
                    .ForEach(x => x.ActualQuantity = 0);
                }
            }
            foreach (Disbursement d in disbList)
            {
                disbursementRepository.Save(d);
            }
        }
    }
}