using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class DeliveryOrderDetail
    {
        [Key]
        [MaxLength(20)]
        [Column(Order = 0)]
        public string DeliveryOrderNo { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 1)]
        public string ItemCode { get; set; }
        public int PlanQuantity { get; set; }
        public int ActualQuantity { get; set; }
        [MaxLength(200)]
        public string Remarks { get; set; }
        public Status Status { get; set; }
        public ApplicationUser UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("DeliveryOrderDetail")]
        public List<StockMovement> StockMovements { get; set; }
        [ForeignKey("DeliveryOrderNo")]
        public DeliveryOrder DeliveryOrder { get; set; }
        [ForeignKey("ItemCode")]
        public Item Item { get; set; }
    }
}