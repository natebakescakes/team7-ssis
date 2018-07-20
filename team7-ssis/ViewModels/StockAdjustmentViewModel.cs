using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{
    public class StockAdjustmentViewModel
    {
        public String StockAdjustmentId { get; set; }

  
        public string CreatedBy { get; set; }
        public string ApprovedBySupervisor { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string StatusId { get; set; }


    }
}