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
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("DeliveryOrder")]
        public List<DeliveryOrderDetail> DeliveryOrderDetails { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public Supplier Supplier { get; set; }
        public Status Status { get; set; }
    }
}