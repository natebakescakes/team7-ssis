using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace team7_ssis.Models
{
    public class Item
    {
        [Key]
        [MaxLength(4)]
        public String ItemCode { get; set; }
        [MaxLength(30)]
        public String Name { get; set; }
        [MaxLength(200)]
        public String Description { get; set; }
        [MaxLength(30)]
        public String Uom { get; set; }
        //[ForeignKey("ItemCategoryId")]
        public ItemCategory itemCategory { get; set; }
        [MaxLength(8)]
        public String Bin { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
        //[ForeignKey("StatusId")]
        public Status Status { get; set; }
        [InverseProperty("Item")]
        public List<ItemPrice> ItemPrices { get; set; }
        [InverseProperty("Item")]
        public List<StockMovement> StockMovements { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public Inventory Inventory { get; set; }
    }
}