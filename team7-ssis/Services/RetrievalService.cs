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
                stockmovementService.CreateStockMovement(disbursementDetail);
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

            #region Update Retrieval status
            retrieval.Status = new StatusService(context).FindStatusByStatusId(20);
            retrieval.UpdatedBy = new UserService(context).FindUserByEmail(email);
            retrieval.UpdatedDateTime = DateTime.Now;
            #endregion

            #region Update Disbursement status
            foreach (var disbursement in retrieval.Disbursements)
            {
                // If item has not yet been retrieved, retrieve it
                // Since web does not retrieve items before confirm retrieval
                foreach (var detail in disbursement.DisbursementDetails)
                {
                    if (detail.Status.StatusId != 18)
                    {
                        detail.Status = new StatusService(context).FindStatusByStatusId(18);
                        stockmovementService.CreateStockMovement(detail);
                    }
                }

                disbursement.Status = statusRepository.FindById(8);
            }
            #endregion

            #region Create Outstanding Requisitions
            // For every group of requisitions in the retrieval with the same department
            foreach (var requisition in retrieval.Requisitions.GroupBy(r => r.Department))
            {
                // If retrieval has no disbursement for department,
                // Change status to Unable to Fulfill (21)
                if (retrieval.Disbursements.Where(d => d.Department == requisition.Key).Count() == 0)
                    requisition.ToList().ForEach(r => r.Status = statusRepository.FindById(21));
                // If disbursement related to requisitions have no actual quantity disbursed,
                // Change status to Unable to Fulfill (21)
                else if (retrieval.Disbursements.Where(d => d.Department == requisition.Key).FirstOrDefault().DisbursementDetails.All(dd => dd.ActualQuantity == 0))
                    requisition.ToList().ForEach(r => r.Status = statusRepository.FindById(21));

                var newRequisitionDetails = new List<RequisitionDetail>();
                // For every group of requisition details in the combined requisition with the same Item Code
                foreach (var detail in requisition.SelectMany(r => r.RequisitionDetails).GroupBy(detail => detail.Item))
                {
                    // Sum of actual quantity disbursement in disbursement details with the same item code 
                    // as requisition details with in disbursement with the same departmentcode
                    // If not found, quantity disbursed is 0
                    var totalDisbursedQuantity = retrieval.Disbursements
                        .Where(disbursement => disbursement.Department == requisition.Key)
                        .FirstOrDefault() == null ? 0 : retrieval.Disbursements
                            .Where(disbursement => disbursement.Department == requisition.Key)
                            .FirstOrDefault()
                            .DisbursementDetails
                            .Where(dd => dd.Item == detail.Key)
                            .Sum(dd => dd.ActualQuantity);

                    // If total quantity disbursed is 0, change requisition detail status to unfulfilled (21)
                    if (totalDisbursedQuantity == 0)
                        detail.ToList().ForEach(d =>
                        {
                            d.Status = statusRepository.FindById(21);
                        });

                    // If remaining quantity is 0, skip creating outstanding requisition detail
                    if (detail.Sum(d => d.Quantity) - totalDisbursedQuantity == 0)
                        continue;

                    newRequisitionDetails.Add(new RequisitionDetail()
                    {
                        RequisitionId = IdService.GetNewAutoGenerateRequisitionId(context),
                        Item = detail.Key,
                        ItemCode = detail.Key.ItemCode,
                        Quantity = detail.Sum(d => d.Quantity) - totalDisbursedQuantity,
                        Status = new StatusService(context).FindStatusByStatusId(3),
                    });
                }

                // Create new outstanding requisition
                // Collection point is based on one of the requisition in this retrieval for this department at random
                var newRequisition = new Requisition()
                {
                    RequisitionId = IdService.GetNewAutoGenerateRequisitionId(context),
                    Department = requisition.Key,
                    CollectionPoint = requisition.OrderBy(r => r.CreatedDateTime).FirstOrDefault().CollectionPoint,
                    CreatedBy = new UserService(context).FindUserByEmail("root@admin.com"),
                    CreatedDateTime = DateTime.Now,
                    Status = new StatusService(context).FindStatusByStatusId(6),
                    EmployeeRemarks = $"Automatically generated by system based on outstanding quantity - {retrievalId}",
                    RequisitionDetails = newRequisitionDetails,
                };

                // If there are any requisition details saved to new outstanding requisition
                if (newRequisitionDetails.Count() > 0)
                    new RequisitionRepository(context).Save(newRequisition);
            }
            #endregion

            #region Update all requisition statuses that are not Unable to Fulfilled
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
            #endregion

            retrievalRepository.Save(retrieval);

            #region Create Notification
            foreach (var requisition in retrieval.Requisitions.GroupBy(r => r.Department))
            {
                if (retrieval.Disbursements.Where(d => d.Department.DepartmentCode == requisition.Key.DepartmentCode).Count() == 0)
                    new NotificationService(context).CreateUnableToFulFillNotification(retrieval, requisition.Key.Representative);
            }
            foreach (var disbursement in retrieval.Disbursements)
            {
                foreach (var requisition in disbursement.Retrieval.Requisitions)
                {
                    new NotificationService(context).CreateNotification(disbursement, requisition.CreatedBy);
                }
            }
            #endregion
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