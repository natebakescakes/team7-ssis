using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

}