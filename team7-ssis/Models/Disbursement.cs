using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Disbursement
    {
        [Key]
        [MaxLength(20)]
        public String DisbursementId { get; set; }
        public virtual Department Department { get; set; }
        public virtual Retrieval Retrieval { get; set; }
        [InverseProperty("Disbursement")]
        public virtual List<DisbursementDetail> DisbursementDetails { get; set; }
        [MaxLength(200)]
        public String Remarks { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ApplicationUser CollectedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? CollectedDateTime { get; set; }
    }
}