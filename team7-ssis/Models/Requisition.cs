using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Requisition
    {
        [Key]
        [MaxLength(20)]
        public String RequisitionId { get; set; }
        public virtual Department Department { get; set; }
        public virtual CollectionPoint CollectionPoint { get; set; }
        [InverseProperty("Requisition")]
        public virtual List<RequisitionDetail> RequisitionDetails { get; set; }
        public virtual Retrieval Retrieval { get; set; }
        [MaxLength(200)]
        public String EmployeeRemarks { get; set; }
        [MaxLength(200)]
        public String HeadRemarks { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public virtual ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime? ApprovedDateTime { get; set; }
    }
}