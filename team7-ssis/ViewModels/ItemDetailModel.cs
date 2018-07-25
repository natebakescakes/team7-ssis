using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public int Status { get; set; }
    }
}