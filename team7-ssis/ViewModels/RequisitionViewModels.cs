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
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
    public class CreateRequisitionViewModel
    {
        public List<CollectionPoint> SelectCollectionPointList { get; set; }
        public string Representative { get; set; }
    }
    public class CreateRequisitionJSONViewModel
    {
        public string ItemCode { get; set; }
        public int Qty { get; set; }
    }
}