using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class PurchaseOrder
    {
        [Key]
        [MaxLength(20)]
        public string PurchaseOrderNo { get; set; }
        [MaxLength(4)]
        public String SupplierCode { get; set; }
        [ForeignKey("SupplierCode")]
        public Supplier Supplier { get; set; }
        [InverseProperty("PurchaseOrder")]
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        [InverseProperty("PurchaseOrder")]
        public List<DeliveryOrder> DeliveryOrders { get; set; }
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime ApprovedDateTime { get; set; }
    }
}