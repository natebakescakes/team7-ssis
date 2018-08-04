using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StockHistoryViewModel
    {
        public DateTime theDate { get; set; }
        public string host { get; set; }
        public int qty { get; set; }
        public string qtyString { get; set; }
        public int balance { get; set; }
    }
}