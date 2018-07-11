using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Supplier
    {
        [Key]
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
        public List<ItemPrice> ItemPrices { get; set; }
        [InverseProperty("Supplier")]
        public List<PurchaseOrder> Orders { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}