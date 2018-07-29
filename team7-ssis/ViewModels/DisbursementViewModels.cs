using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class DisbursementFormViewModel
    {
        public string DisbursementId { get; set; }
        public string Representative { get; set; }
        public string Department { get; set; }
        public string OrderTime { get; set; }
        public string CollectionPoint { get; set; }
        public int Status { get; set; }
    }

    public class DisbursementFormTableViewModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public string Uom { get; set; }
    }
    public class StationeryDisbursementViewModel
    {
        public string DisbursementID { get; set; }
        public string Department { get; set; }
        public string Time { get; set; }
        public string CollectionPoint { get; set; }
        public string DisbursedBy { get; set; }
        public string Status { get; set; }
    }

    public class DisbursementMobileViewModel
    {
        public string DisbursementId { get; set; }
        public string RetrievalId { get; set; }
        public string Department { get; set; }
        public string CollectionPoint { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public List<DisbursementFormTableViewModel> DisbursementDetails { get; set; }
    }

    public class DisbursementIdViewModel
    {
        public string DisbursementId { get; set; }
    }
}