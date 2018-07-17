using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class RequisitionDetail
    {
        [Key]
        [Column(Order = 1)]
        [MaxLength(20)]
        public string RequisitionId { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public string ItemCode { get; set; }
        [ForeignKey("RequisitionId")]
        public virtual Requisition Requisition { get; set; }
        [ForeignKey("ItemCode")]
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}