using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class PurchaseOrderDetail
    {   
        [Key]
        [MaxLength(6)]
        [Column(Order = 1)]
        public string PurchaseOrderNo { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public string ItemCode { get; set; }
        [ForeignKey("PurchaseOrderNo")]
        public PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey("ItemCode")]
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}