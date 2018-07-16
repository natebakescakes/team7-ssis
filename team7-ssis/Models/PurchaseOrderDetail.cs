﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class PurchaseOrderDetail
    {   
        [Key]
        [MaxLength(20)]
        [Column(Order = 1)]
        public string PurchaseOrderNo { get; set; }
        [Key]
        [MaxLength(4)]
        [Column(Order = 2)]
        public string ItemCode { get; set; }
        [ForeignKey("PurchaseOrderNo")]
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        [ForeignKey("ItemCode")]
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}