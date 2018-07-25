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
        public string Status { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public string SupplierName { get; set; }
        public double SupplierUnitPrice { get; set; }
    }
}