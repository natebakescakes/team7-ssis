using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StationeryRetrievalViewModel
    {
        public string RetrievalID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }
    public class StationeryRetrievalTableViewModel
    {
        public string ProductID { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
    }
    public class RequisitionDetailViewModel
    {
        public string RequisitionID { get; set; }
        public string Department { get; set; }
        public string CollectionPoint { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedTime { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedTime { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedTime { get; set; }
    }
    public class StationeryDisbursementViewModel
    {
        public string DisbursementID { get; set; }
        public string Department { get; set; }
        public string CollectionPoint { get; set; }
        public string DisbursedBy { get; set; }
        public string Status { get; set; }
    }
    public class ManageRequisitionsViewModel
    {
        public string Requisition { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }

}