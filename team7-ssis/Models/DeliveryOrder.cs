using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class DeliveryOrder
    {
        [Key]
        [MaxLength(20)]
        public string DeliveryOrderNo { get; set; }
        [MaxLength(200)]
        public String DeliveryOrderFileName { get; set; }
        [MaxLength(200)]
        public String InvoiceFileName { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("DeliveryOrder")]
        public virtual List<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Status Status { get; set; }
    }
}