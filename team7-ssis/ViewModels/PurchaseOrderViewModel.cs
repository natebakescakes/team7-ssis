using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public string PNo { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
    }
}