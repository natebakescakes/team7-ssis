using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class StockAdjustment
    {
        [Key]
        [MaxLength(20)]
        public String StockAdjustmentId { get; set; }
        [InverseProperty("StockAdjustment")]
        public virtual List<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        [MaxLength(200)]
        public string Remarks { get; set; }
        //[ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ApplicationUser ApprovedBySupervisor { get; set; }
        public virtual ApplicationUser ApprovedByManager { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? ApprovedSupervisorDateTime { get; set; }
        public DateTime? ApprovedManagerDateTime { get; set; }
    }
}