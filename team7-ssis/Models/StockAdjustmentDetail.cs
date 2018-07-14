using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class StockAdjustmentDetail
    {
        [Key]
        [Column(Order = 1)]
        [MaxLength(20)]
        public string StockAdjustmentId { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public string ItemCode { get; set; }
        [ForeignKey("StockAdjustmentId")]
        public virtual StockAdjustment StockAdjustment { get; set; }
        [ForeignKey("ItemCode")]
        public virtual Item Item { get; set; }
        public int OriginalQuantity { get; set; }
        public int AfterQuantity { get; set; }
        [MaxLength(200)]
        public string Reason { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("StockAdjustmentDetail")]
        public virtual List<StockMovement> StockMovements { get; set; }
    }
}