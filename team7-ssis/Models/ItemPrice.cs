using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class ItemPrice
    {
        [Key]
        [MaxLength(4)]
        [Column(Order = 1)]
        public String ItemCode { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public String SupplierCode { get; set; }
        [ForeignKey("ItemCode")]
        public virtual Item Item { get; set; }
        [ForeignKey("SupplierCode")]
        public virtual Supplier Supplier { get; set; }
        public int PrioritySequence { get; set; }
        public decimal Price { get; set; }
        //[ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}