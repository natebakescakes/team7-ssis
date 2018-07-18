using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class RequisitionService
    {
        ApplicationDbContext context;
        RetrievalService retrievalService;

        public RequisitionService(ApplicationDbContext context)
        {
            this.context = context;
            retrievalService = new RetrievalService(context);
        }
        public List<Requisition> FindRequisitionsByStatus(List<Status> statusList)
        {
            throw new NotImplementedException();
        }

        public RequisitionDetail GetRequisitionDetails(string requisitionId)
        {
            throw new NotImplementedException();
        }

        public string ProcessRequisitions(List<Requisition> requestList)
        {
            // create one Retrieval
            Retrieval r = new Retrieval();
            r.RetrievalId = IdService.GetNewRetrievalId(context);

            // save the Retrieval
            retrievalService.Save(r);

            // create Disbursements, one for each department
            List<Disbursement> disbursementList = CreateDisbursementsByDepartment(requestList);

            // create DisbursementDetails, one for each item by department
            foreach (Disbursement d in disbursementList)
            {
                foreach (Requisition rq in requestList)
                {
                    if (rq.Department == d.Department)
                    {
                        foreach (RequisitionDetail rd in rq.RequisitionDetails)
                        {
                            foreach (DisbursementDetail dd in d.DisbursementDetails)
                            {
                                if (dd.ItemCode == rd.ItemCode)
                                {
                                    dd.PlanQuantity += rd.Quantity;
                                }
                                else
                                {
                                    DisbursementDetail newDd = new DisbursementDetail();
                                    newDd.ItemCode = rd.ItemCode;
                                    newDd.PlanQuantity = rd.Quantity;

                                    d.DisbursementDetails.Add(newDd);
                                }
                            }
                            
                        }
                    }
                }
            }

            // return retrievalId

            throw new NotImplementedException();
        }

        public List<Disbursement> CreateDisbursementsByDepartment(List<Requisition> requestList)
        {
            List<Disbursement> disbursementList = new List<Disbursement>();

            // select all distinct departments from disbursementList
            IEnumerable<Department> departments = disbursementList.Select(x => x.Department).Distinct();

            foreach (Requisition r in requestList)
            {
                if (!departments.Contains(r.Department)) {
                    // create new disbursement
                    Disbursement d = new Disbursement();
                    d.DisbursementId = IdService.GetNewDisbursementId(context);
                    d.Department = r.Department;

                    disbursementList.Add(d);
                }
            }

            return disbursementList;
        }

        public List<Item> AddItemsToRequisition(List<Item> items)
        {
            throw new NotImplementedException();
        }

        public Requisition CreateRequisition(Requisition req)
        {
            throw new NotImplementedException();
        }

        public Item AddItemsToRequisition(Item item)
        {
            throw new NotImplementedException();
        }

        public Requisition CancelRequisition(Requisition req)
        {
            throw new NotImplementedException();
        }

        public List<Requisition> ApproveRequisitions(List<Requisition> reqList)
        {
            throw new NotImplementedException();
        }

        public Requisition ApproveRequisitions(Requisition requisition)
        {
            throw new NotImplementedException();
        }

    }
}