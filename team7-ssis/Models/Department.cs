using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class Department
    {
        [Key]
        [MaxLength(4)]
        public string DepartmentCode { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public virtual ApplicationUser Representative { get; set; }
        public virtual ApplicationUser Head { get; set; }
        public virtual CollectionPoint CollectionPoint { get; set; }
        [MaxLength(30)]
        public string ContactName { get; set; }
        [MaxLength(30)]
        public string PhoneNumber { get; set; }
        [MaxLength(30)]
        public string FaxNumber { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("Department")]
        public virtual List<ApplicationUser> Employees { get; set; }
        [InverseProperty("Department")]
        public virtual List<Requisition> Requisitions { get; set; }
        [InverseProperty("Department")]
        public virtual List<Disbursement> Disbursements { get; set; }
    }
}