using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace team7_ssis.Models
{
    public class Retrieval
    {
        [Key]
        [MaxLength(20)]
        public String RetrievalId { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public virtual ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        [InverseProperty("Retrieval")]
        public virtual List<Requisition> Requisitions { get; set; }
        [InverseProperty("Retrieval")]
        public virtual List<Disbursement> Disbursements { get; set; }
    }
}