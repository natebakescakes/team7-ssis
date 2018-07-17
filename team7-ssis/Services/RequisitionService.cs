using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class RequisitionService : Controller
    {
        public List<Requisition> FindRequisitionsByStatus(List<Status> statusList)
        {
            throw new NotImplementedException();
        }

        public RequisitionDetail GetRequisitionDetails(string requisitionId)
        {
            throw new NotImplementedException();
        }

        public void ProcessRequisitions(List<Requisition> requestList)
        {
            throw new NotImplementedException();

            //
            // Part 1. Create Retrieval Form w/Details
            //

            // receive list of requisitions
            // create HashMap<Item, int> map
            // get a map of the items you need to retrieve, and how many of them. Implements Retrieval

            // foreach (requisition in requestList):
            //      foreach (reqDetail in requisition.requisitionDetails):
            //          if reqDetail.itemCode not in map:
            //              put(reqDetail.itemCode, reqDetail.quantity)
            //          else:
            //              set(reqDetail.itemCode, quantity + reqDetail.quantity)

            // convert the map to a List
            // return this list for display.

            //
            // Part 2. Create Disbursement Forms
            //

            // collect how much each department wanted of each item
            // - get list of departments in the requestList called deptList

            // foreach (dept in deptList):

            //      create a new Disbursement, d
            //      d.DepartmentCode = dept.DepartmentCode

            //      foreach (req in reqList):
            //          foreach (requisitionDetail in req)
            //              foreach (disbursementDetail in d.DisbursementDetails)
            //                  if requsitionDetail.itemCode in disbursementDetail.itemCode.values()
            //                      disbursementDetail.plannedQuantity += requisitionDetail.quantity
            //                  else:
            //                      DisbursementDetail db = new DisbursemsentDetail();
            //                      db.itemCode = requisitionDetail.itemCode;
            //                      db.plannedQuantity = requisitionDetail.quantity;
            //                      d.DisbursementDetails.add(db)
            //
            //      persist disbursement

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