using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class DeliveryOrderViewModel
    {
        public string DeliveryOrderNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string SupplierName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
    }
}
