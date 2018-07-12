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
        public List<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        [MaxLength(200)]
        public string Remarks { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser ApprovedBySupervisor { get; set; }
        public ApplicationUser ApprovedByManager { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime ApprovedSupervisorDateTime { get; set; }
        public DateTime ApprovedManagerDateTime { get; set; }
    }
}