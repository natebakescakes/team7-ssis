using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class StockMovement
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StockMovementId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("DeliveryOrderDetail")]
        [MaxLength(20)]
        [Column(Order = 1)]
        public String DeliveryOrderNo { get; set; }
        [ForeignKey("DeliveryOrderDetail")]
        [MaxLength(4)]
        [Column(Order = 2)]
        public String DeliveryOrderDetailItemCode { get; set; }
        [ForeignKey("DisbursementDetail")]
        [Column(Order = 3)]
        [MaxLength(20)]
        public String DisbursementId { get; set; }
        [ForeignKey("DisbursementDetail")]
        [Column(Order = 4)]
        [MaxLength(4)]
        public String DisbursementDetailItemCode { get; set; }
        [ForeignKey("StockAdjustmentDetail")]
        [Column(Order = 5)]
        [MaxLength(20)]
        public string StockAdjustmentId { get; set; }
        [ForeignKey("StockAdjustmentDetail")]
        [Column(Order = 6)]
        [MaxLength(4)]
        public String StockAdjustmentDetailItemCode { get; set; }
        public int OriginalQuantity { get; set; }
        public int AfterQuantity { get; set; }
        public DeliveryOrderDetail DeliveryOrderDetail { get; set; }
        public DisbursementDetail DisbursementDetail { get; set; }
        public StockAdjustmentDetail StockAdjustmentDetail { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}