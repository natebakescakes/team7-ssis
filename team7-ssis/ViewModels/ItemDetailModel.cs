using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class ItemDetailModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string ItemCategoryName { get; set; }
        public string Bin { get; set; }
        public string Uom { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public List<ItemDetailsSupplierInfoViewModel> SupplierInfo { get; set; }
    }
}