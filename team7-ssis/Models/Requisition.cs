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
        public Department Department { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
        [InverseProperty("Requisition")]
        public List<RequisitionDetail> RequisitionDetails { get; set; }
        public Retrieval Retrieval { get; set; }
        [MaxLength(200)]
        public String EmployeeRemarks { get; set; }
        [MaxLength(200)]
        public String HeadRemarks { get; set; }
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime ApprovedDateTime { get; set; }
    }
}