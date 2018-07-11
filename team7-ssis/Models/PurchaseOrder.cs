using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class PurchaseOrder
    {
        [Key]
        [MaxLength(6)]
        public string PurchaseOrderNo { get; set; }
        //[ForeignKey("SupplierCode")]
        public Supplier Supplier { get; set; }
        [InverseProperty("PurchaseOrder")]
        public List<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime ApprovedDateTime { get; set; }
    }
}