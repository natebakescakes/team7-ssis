using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class RequisitionDetail
    {
        [Key]
        [Column(Order = 1)]
        public int RequisitionId { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public string ItemCode { get; set; }
        [ForeignKey("RequisitionId")]
        public Requisition Requisition { get; set; }
        [ForeignKey("ItemCode")]
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}