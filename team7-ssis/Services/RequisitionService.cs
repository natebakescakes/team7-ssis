using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;

using Microsoft.AspNet.Identity;

namespace team7_ssis.Services
{
    public class RequisitionService
    {
        ApplicationDbContext context;

        RetrievalService retrievalService;
        DisbursementService disbursementService;

        RequisitionRepository requisitionRepository;
        RequisitionDetailRepository requisitionDetailRepository;
        StatusRepository statusRepository;
        UserRepository userRepository;
        StatusService statusService;

        public RequisitionService(ApplicationDbContext context)
        {
            this.context = context;

            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);

            requisitionRepository = new RequisitionRepository(context);
            requisitionDetailRepository = new RequisitionDetailRepository(context);
            statusRepository = new StatusRepository(context);
            userRepository = new UserRepository(context);
            statusService = new StatusService(context);
        }

        public List<Requisition> FindRequisitionsByStatus(List<Status> statusList)
        {
            // TODO: To be obseleted
            var query = requisitionRepository.FindRequisitionsByStatus(statusList);
            if (query == null)
            {
                throw new ArgumentException("No Requisitions contain given statuses.");
            }
            else
            {
                return requisitionRepository.FindRequisitionsByStatus(statusList).ToList();
            }
        }

        public List<Requisition> FindAllRequisitions()
        {
            // TODO: Write Test
            return requisitionRepository.FindAll().ToList();
        }

        public List<RequisitionDetail> FindAllRequisitionDetail()
        {
            // TODO: Write Test
            return requisitionDetailRepository.FindAll().ToList();
        }

        public List<RequisitionDetail> GetRequisitionDetails(string requisitionId)
        {
            var query = requisitionRepository.FindRequisitionDetails(requisitionId).ToList();
            if (query == null)
            {
                throw new Exception("No Requisition Details Found");
            }
            else
            {
                return query;
            }
        }

        public string ProcessRequisitions(List<Requisition> requestList)
        {
            if (requestList.Count == 0)
            {
                throw new Exception("List of Requisitions cannot be null");
            }

            // create one Retrieval
            Retrieval r = new Retrieval();
            r.RetrievalId = IdService.GetNewRetrievalId(context);
            r.CreatedDateTime = DateTime.Now;
            r.Status = statusRepository.FindById(17);
            if (HttpContext.Current != null)
            {
                r.CreatedBy = userRepository.FindById(HttpContext.Current.User.Identity.GetUserId());
            }

            // save the Retrieval
            retrievalService.Save(r);

            // Map each requisition to each retrieval
            foreach (Requisition req in requestList)
            {
                req.Retrieval = r;
            }

            // create Disbursements, one for each department
            List<Disbursement> emptyDisbursements = CreateDisbursementForEachDepartment(requestList);

            // create DisbursementDetails, one for each item by department
            List<Disbursement> filledDisbursements = AddDisbursementDetailsForEachDisbursement(emptyDisbursements, requestList);

            foreach (Disbursement d in filledDisbursements)
            {
                var newDisbursementDetails = new List<DisbursementDetail>();

                // if disbursement details has plan quantity = 0, ignore
                for (int i = 0; i < d.DisbursementDetails.Count(); i++)
                {
                    if (d.DisbursementDetails[i].PlanQuantity == 0)
                    {
                        // Change Requisition Detail Status to Unable to fulfill when no quantity is disbursed
                        d.Retrieval.Requisitions.SelectMany(requisition => requisition.RequisitionDetails.Where(detail => detail.ItemCode == d.DisbursementDetails[i].ItemCode)).ToList().ForEach(detail =>
                        {
                            detail.Status = statusRepository.FindById(21);
                        });

                        continue;
                    }

                    // Add only DisbursementDetails that have plan quantity
                    newDisbursementDetails.Add(d.DisbursementDetails[i]);
                }

                // Replace DisbursementDetails with filtered list
                d.DisbursementDetails = newDisbursementDetails;

                // if disbursement has no disbursement details, skip to next disbursement
                if (d.DisbursementDetails.Count() == 0)
                {
                    // Change Requisition Status to Unable to fulfill when no items are disbursed
                    d.Retrieval.Requisitions.Where(requisition => requisition.Department.DepartmentCode == d.Department.DepartmentCode).ToList().ForEach(requisition =>
                    {
                        requisition.Status = statusRepository.FindById(21);
                        requisitionRepository.Save(requisition);
                    });
                    continue;
                }


                d.DisbursementId = IdService.GetNewDisbursementId(context);
                d.Retrieval = r;
                d.Status = statusRepository.FindById(19);
                if (HttpContext.Current != null)
                {
                    d.CreatedBy = userRepository.FindById(HttpContext.Current.User.Identity.GetUserId());
                }
                disbursementService.Save(d);
            }

            // update the status of the requisitions
            foreach (Requisition req in requestList)
            {
                req.Status = statusRepository.FindById(7);
                requisitionRepository.Save(req);
            }

            return r.RetrievalId;
        }


        public List<Disbursement> CreateDisbursementForEachDepartment(List<Requisition> requestList)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();

            // select all distinct Department from requestList
            var departments = requestList.Select(x => x.Department).Distinct();

            // create Disbursement for each Department
            foreach (Department dept in departments)
            {
                Disbursement d = new Disbursement();
                d.CreatedDateTime = DateTime.Now;
                // d.DisbursementId = IdService.GetNewDisbursementId(context);
                d.Department = dept;
                d.Retrieval = requestList.FirstOrDefault().Retrieval;
                disbursementList.Add(d);
            }

            //disbursementService.Save(disbursementList);
            return disbursementList;
        }

        public Requisition Save(Requisition requisition)
        {
            return requisitionRepository.Save(requisition);
        }

        public List<Requisition> Save(List<Requisition> reqList)
        {
            foreach (Requisition r in reqList)
            {
                requisitionRepository.Save(r);
            }
            return reqList;
        }

        private List<Disbursement> AddDisbursementDetailsForEachDisbursement(List<Disbursement> disbursementList, List<Requisition> requestList)
        {

            // create Disbursement Details, one for each item by department
            foreach (Disbursement d in disbursementList)
            {
                // prepare to populate DisbursementDetails
                d.DisbursementDetails = new List<DisbursementDetail>();

                // Initialize inventory map
                Dictionary<string, int> inventory = new Dictionary<string, int>();

                // populate them based on CreatedDate first
                foreach (Requisition rq in requestList.OrderBy(r => r.CreatedDateTime))
                {
                    if (rq.Department == d.Department)
                    {
                        foreach (RequisitionDetail rd in rq.RequisitionDetails)
                        {
                            var query = d.DisbursementDetails.Where(x => x.Item == rd.Item);

                            // Use quantity in inventory map if available, else get current inventory level in context
                            int currentQuantity;
                            if (inventory.ContainsKey(rd.ItemCode))
                                currentQuantity = inventory[rd.ItemCode];
                            else
                            {
                                inventory[rd.ItemCode] = new ItemService(context).FindInventoryByItemCode(rd.ItemCode).Quantity;
                                currentQuantity = inventory[rd.ItemCode];
                            }

                            // if a DisbursementDetail has the same ItemCode as the RequisitionDetail
                            if (query.Count() > 0)
                            {
                                DisbursementDetail existingDD = query.ToList().First();
                                existingDD.PlanQuantity += Math.Min(rd.Quantity, inventory[rd.ItemCode]);

                                // Deduct quantity
                                inventory[rd.ItemCode] -= existingDD.PlanQuantity;
                            }
                            else // Create a DD with the RD
                            {
                                DisbursementDetail newDD = new DisbursementDetail();
                                newDD.Item = rd.Item;
                                newDD.ItemCode = rd.ItemCode;
                                newDD.PlanQuantity = Math.Min(rd.Quantity, inventory[rd.ItemCode]);
                                newDD.Bin = rd.Item.Bin;

                                // Deduct quantity
                                inventory[rd.ItemCode] -= newDD.PlanQuantity;

                                // Add to the Disbursement
                                d.DisbursementDetails.Add(newDD);
                            }
                        }
                    }
                }
            }

            return disbursementList;
        }

        public int FindUnfulfilledQuantityRequested(Item item)
        {
            int totalQuantity = 0;

            List<Status> statusList = new List<Status>();
            Status approved = statusService.FindStatusByStatusId(6);
            Status reqProcessed = statusService.FindStatusByStatusId(7);

            statusList.Add(approved);
            statusList.Add(reqProcessed);
            List<Requisition> outstandingReq = FindRequisitionsByStatus(statusList);

            foreach (Requisition req in outstandingReq)
            {
                foreach (RequisitionDetail reqDetail in req.RequisitionDetails)
                {
                    if (reqDetail.ItemCode == item.ItemCode)
                    {
                        totalQuantity = totalQuantity + reqDetail.Quantity;
                    }
                }
            }

            return totalQuantity;
        }

        public List<Requisition> FindRequisitionsByDepartment(Department department)
        {
            return requisitionRepository.FindByDepartment(department).ToList();
        }

        /// <summary>
        /// Approves requisition if in the correct status
        /// </summary>
        /// <param name="requisitionId"></param>
        public void ApproveRequisition(string requisitionId, string email, string remarks)
        {
            if (!requisitionRepository.ExistsById(requisitionId))
                throw new ArgumentException("Requisition not found");

            if (requisitionRepository.FindById(requisitionId).Status.StatusId == 5 ||
                requisitionRepository.FindById(requisitionId).Status.StatusId == 6)
                throw new ArgumentException("Requisition has already been approved or rejected");

            // Change values
            var requisition = requisitionRepository.FindById(requisitionId);
            requisition.HeadRemarks = remarks;
            requisition.Status = new StatusService(context).FindStatusByStatusId(6);
            requisition.ApprovedBy = new UserService(context).FindUserByEmail(email);
            requisition.ApprovedDateTime = DateTime.Now;

            // Update Requisition Detail Status
            requisition.RequisitionDetails.ForEach(rd => rd.Status = new StatusService(context).FindStatusByStatusId(6));

            // Save
            requisitionRepository.Save(requisition);
        }

        /// <summary>
        /// Rejects requisition if in the correc status
        /// </summary>
        /// <param name="requisitionId"></param>
        public void RejectRequisition(string requisitionId, string email, string remarks)
        {
            if (!requisitionRepository.ExistsById(requisitionId))
                throw new ArgumentException("Requisition not found");

            if (requisitionRepository.FindById(requisitionId).Status.StatusId == 5 ||
                requisitionRepository.FindById(requisitionId).Status.StatusId == 6)
                throw new ArgumentException("Requisition has already been approved or rejected");

            // Change values
            var requisition = requisitionRepository.FindById(requisitionId);
            requisition.HeadRemarks = remarks;
            requisition.Status = new StatusService(context).FindStatusByStatusId(5);
            requisition.ApprovedBy = new UserService(context).FindUserByEmail(email);
            requisition.ApprovedDateTime = DateTime.Now;

            // Update Requisition Detail Status
            requisition.RequisitionDetails.ForEach(rd => rd.Status = new StatusService(context).FindStatusByStatusId(5));

            // Save
            requisitionRepository.Save(requisition);
        }
        /// <summary>
        /// Updates the Status of the Requisition
        /// </summary>
        /// <param name="retId"></param>
        /// <param name="statusId"></param>
        /// <param name="email"></param>
        public void UpdateRequisitionStatus(string retId, int statusId, string email)
        {
            Requisition r = requisitionRepository.FindById(retId);
            r.Status = statusRepository.FindById(statusId);
            requisitionRepository.Save(r);
        }
    }
}