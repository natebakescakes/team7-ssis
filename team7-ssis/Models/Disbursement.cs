using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Disbursement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisbursementId { get; set; }
        //[ForeignKey("RequisitionId")]
        public Requisition Requisition { get; set; }
        [InverseProperty("Disbursement")]
        public List<DisbursementDetail> DisbursementDetails { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}