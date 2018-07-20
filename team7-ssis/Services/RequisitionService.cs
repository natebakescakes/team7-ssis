using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class RequisitionService
    {
        ApplicationDbContext context;
        RetrievalService retrievalService;
        DisbursementService disbursementService;

        RequisitionRepository requisitionRepository;

        public RequisitionService(ApplicationDbContext context)
        {
            this.context = context;
            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);
            requisitionRepository = new RequisitionRepository(context);
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
            r.CreatedDateTime = DateTime.Now;

            // save the Retrieval
            retrievalService.Save(r);

            // create Disbursements, one for each department
            List<Disbursement> emptyDisbursements = CreateDisbursementForEachDepartment(requestList);

            // create DisbursementDetails, one for each item by department
            List<Disbursement> filledDisbursements = AddDisbursementDetailsForEachDisbursement(emptyDisbursements, requestList);

            foreach (Disbursement d in filledDisbursements)
            {
                d.DisbursementId = IdService.GetNewDisbursementId(context);
                disbursementService.Save(d);
            }

            return r.RetrievalId;
        }

        private List<Disbursement> AddDisbursementDetailsForEachDisbursement(List<Disbursement> disbursementList, List<Requisition> requestList)
        {

            // create Disbursement Details, one for each item by department
            foreach (Disbursement d in disbursementList)
            {
                // prepare to populate DisbursementDetails
                d.DisbursementDetails = new List<DisbursementDetail>();

                // populate them
                foreach (Requisition rq in requestList)
                {
                    if (rq.Department == d.Department)
                    {
                        foreach (RequisitionDetail rd in rq.RequisitionDetails)
                        {
                            var query = d.DisbursementDetails.Where(x => x.ItemCode == rd.ItemCode);
                            // if a DisbursementDetail has the same ItemCode as the RequisitionDetail
                            if (query.Count() > 0)
                            {
                                DisbursementDetail existingDD = query.ToList().First();
                                existingDD.PlanQuantity += rd.Quantity;
                            }
                            else // Create a DD with the RD
                            {
                                DisbursementDetail newDD = new DisbursementDetail();
                                newDD.Item = rd.Item;
                                newDD.PlanQuantity = rd.Quantity;

                                // Add to the Disbursement
                                d.DisbursementDetails.Add(newDD);
                            }
                        }
                    }
                }
            }

            return disbursementList;
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
                disbursementList.Add(d);
            }

            //disbursementService.Save(disbursementList);
            return disbursementList;
        }

        //public List<Item> AddItemsToRequisition(List<Item> items)
        //{
        //    throw new NotImplementedException();
        //}

        //public Requisition CreateRequisition(Requisition req)
        //{
        //    throw new NotImplementedException();
        //}

        //public Item AddItemsToRequisition(Item item)
        //{
        //    throw new NotImplementedException();
        //}

        //public Requisition CancelRequisition(Requisition req)
        //{
        //    throw new NotImplementedException();
        //}

        //public List<Requisition> ApproveRequisitions(List<Requisition> reqList)
        //{
        //    throw new NotImplementedException();
        //}

        //public Requisition ApproveRequisitions(Requisition requisition)
        //{
        //    throw new NotImplementedException();
        //}

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

    }
}