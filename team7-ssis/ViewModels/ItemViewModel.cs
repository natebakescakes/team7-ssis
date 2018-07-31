using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{
    public class ItemViewModel
    {
        public string ItemCode { get; set; }
        public string ItemCategoryName { get; set; }
        public string Description { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
        public string Uom { get; set; }
        public int Quantity { get; set; }
        public string UnitPrice { get; set; }

        public decimal UnitPriceDecimal { get; set; }
        public int AmountToReorder { get; set; }
        public decimal TotalPrice { get; set; }

        public string ImagePath { get; set; }
    }

    public class ItemPriceViewModel
    {
        public string ItemCode { get; set; }
        public string ItemCategoryName { get; set; }
        public string Description { get; set; }
        public string Uom { get; set; }

        public decimal Price { get; set; }

        public string SupplierCode { get; set; }
    }


    public class ItemCategoryViewModel
    {
        public int ItemCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Status { get; set; }

        public string StatusName { get; set; }

        public IEnumerable<SelectListItem> Statuses { get; set; }

    }
    
}