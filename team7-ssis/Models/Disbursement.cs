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
        public Department Department { get; set; }
        public Retrieval Retrieval { get; set; }
        [InverseProperty("Disbursement")]
        public List<DisbursementDetail> DisbursementDetails { get; set; }
        [MaxLength(200)]
        public String Remarks { get; set; }
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser CollectedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime CollectedDateTime { get; set; }
    }
}