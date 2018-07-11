using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace team7_ssis.Models
{
    [Table("Inventory")]
    public class Inventory
    {
        [Key]
        [MaxLength(4)]
        [ForeignKey("Item")]
        public String ItemCode { get; set; }
        public Item Item { get; set; }
        public int Quantity { get; set; }
    }
}