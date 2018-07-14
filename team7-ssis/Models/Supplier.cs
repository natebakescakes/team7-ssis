using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Supplier
    {
        [Key]
        [MaxLength(4)]
        public string SupplierCode { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Address { get; set; }
        [MaxLength(30)]
        public string ContactName { get; set; }
        [MaxLength(30)]
        public string PhoneNumber { get; set; }
        [MaxLength(30)]
        public string FaxNumber { get; set; }
        [MaxLength(30)]
        public string GstRegistrationNo { get; set; }
        [InverseProperty("Supplier")]
        public virtual List<ItemPrice> ItemPrices { get; set; }
        [InverseProperty("Supplier")]
        public virtual List<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}