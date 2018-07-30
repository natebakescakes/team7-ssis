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
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Status { get; set; }
        public string DeliverOrderFileName { get; set; }
        public string CreatedBy { get; set; }
        public string InvoiceFileName { get; set; }
        public string DeliveryOrderFileName { get; set; }
    }

    public class DeliveryOrderDetailsViewModel
    {
        public string DeliveryOrderNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
        public int ReceivedQty { get; set; }
        public int RemainingQuantity { get; set; }
        public string CheckboxStatus { get; set; }
        public string InvoiceFileName { get; set; }
        public string DeliveryOrderFileName { get; set; }
    }
}
