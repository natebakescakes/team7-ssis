using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{

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
    public class ManageRequisitionsViewModel
    {
        public string Requisition { get; set; }
        public string Status { get; set; }
    }
    public class CreateRequisitionViewModel
    {
        public string Action { get; set; }
        public List<CollectionPoint> SelectCollectionPointList { get; set; }
        public string Representative { get; set; }
    }
    public class CreateRequisitionJSONViewModel
    {
        public string ItemCode { get; set; }
        public int Qty { get; set; }
    }

    public class RequisitionMobileViewModel
    {
        public string RequisitionId { get; set; }
        public string RequestorName { get; set; }
        public string RequestedDate { get; set; }
        public string Remarks { get; set; }
        public string HeadRemarks { get; set; }
        public string Status { get; set; }
        public List<RequisitionDetailMobileViewModel> RequisitionDetails { get; set; }
    }

    public class RequisitionDetailMobileViewModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string Uom { get; set; }
    }

    public class RequisitionIdViewModel
    {
        public string RequisitionId { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
    }
}