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
        public string CreatedDate { get; set; }
        public string Status { get; set; }
    }

    public class PurchaseOrderDetailsViewModel
    {
        
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int ReceivedQuantity { get; set; }
        public int RemainingQuantity { get; set; }
        public int SupplierPriority { get; set; }


    }

    public class PurchaseOrderDetailsListViewModel
    {
        public List<PurchaseOrderDetailsViewModel> Purchaseorders{ get; set; }
    }
}