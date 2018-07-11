using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Requisition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequisitionId { get; set; }
        //[ForeignKey("DepartmentCode")]
        public Department Department { get; set; }
        //[ForeignKey("CollectionPointId")]
        public CollectionPoint CollectionPoint { get; set; }
        [InverseProperty("Requisition")]
        public List<RequisitionDetail> RequisitionDetails { get; set; }
        [InverseProperty("Requisition")]
        public List<Disbursement> Disbursements { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public ApplicationUser ApprovedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public DateTime ApprovedDateTime { get; set; }
    }
}