using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StationeryRetrievalViewModel
    {
        public string ProductID { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
    }
}