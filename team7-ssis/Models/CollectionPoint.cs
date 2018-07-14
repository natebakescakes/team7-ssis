using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace team7_ssis.Models
{
    public class CollectionPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CollectionPointId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        public virtual ApplicationUser ClerkInCharge { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("CollectionPoint")]
        public virtual List<Department> Departments { get; set; }
        public virtual Status Status { get; set; }
    }
}