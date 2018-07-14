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
        public virtual Supplier Supplier { get; set; }
        [InverseProperty("PurchaseOrder")]
        public virtual List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        [InverseProperty("PurchaseOrder")]
        public virtual List<DeliveryOrder> DeliveryOrders { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
    }
}