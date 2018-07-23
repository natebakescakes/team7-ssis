using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StockAdjustmentDetailViewModel
    {
        public  string ItemCode { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }

        public string PriceColor { get; set; }
        public string UnitPrice { get; set; }
        public string Adjustment { get; set; }

}
}