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

    public class DeliveryOrderDetailsViewModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
        public int ReceivedQty { get; set; }
        public int RemainingQuantity { get; set; }
    }
}
