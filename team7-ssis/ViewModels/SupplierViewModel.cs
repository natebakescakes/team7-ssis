using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace team7_ssis.ViewModels
{
    public class SupplierViewModel
    {
         public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Address { get; set; }

        public string GSTNumber { get; set; }
        public int Status { get; set; }

        public int Priority { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
    }

}