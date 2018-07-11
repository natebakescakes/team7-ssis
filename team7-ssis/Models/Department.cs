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
        public ApplicationUser Representative { get; set; }
        public ApplicationUser Head { get; set; }
        public CollectionPoint CollectionPoint { get; set; }
        [MaxLength(30)]
        public string ContactName { get; set; }
        [MaxLength(30)]
        public string PhoneNumber { get; set; }
        [MaxLength(30)]
        public string FaxNumber { get; set; }
        public Status Status { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        [InverseProperty("Department")]
        public List<ApplicationUser> Employees { get; set; }
    }
}