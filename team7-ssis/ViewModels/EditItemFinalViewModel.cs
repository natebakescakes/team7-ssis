using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace team7_ssis.ViewModels
{
    public class EditItemFinalViewModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string ItemCategoryName { get; set; }
        public string Bin { get; set; }
        public string Uom { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public IEnumerable<SelectListItem> SupplierName { get; set; }
        public string SupplierName1 { get; set; }
        public string SupplierName2 { get; set; }
        public string SupplierName3 { get; set; }
        public decimal SupplierUnitPrice1 { get; set; }
        public decimal SupplierUnitPrice2 { get; set; }
        public decimal SupplierUnitPrice3 { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public int CategoryId { get; set; }
        public string ItemName { get; set; }
        public int ReorderLevel { get; set; }
        public int ReorderQuantity { get; set; }
    }
}