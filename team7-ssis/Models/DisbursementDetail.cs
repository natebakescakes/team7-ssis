using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class DisbursementDetail
    {
        [Key]
        [Column(Order = 0)]
        public int DisbursementId { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 1)]
        public string ItemCode { get; set; }
        [ForeignKey("DisbursementId")]
        public Disbursement Disbursement { get; set; }
        [ForeignKey("ItemCode")]
        public Item Item { get; set; }
        public int Quantity { get; set; }
        [MaxLength(8)]
        public string Bin { get; set; }
        [InverseProperty("DisbursementDetail")]
        public List<StockMovement> StockMovements { get; set; }
    }
}