using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StockAdjustmentDetailListViewModel
    {
        public StockAdjustmentViewModel StockAdjustmentModel { get; set; }
        public List<StockAdjustmentDetailViewModel> StockAdjustmentDetailsModel { get; set; }

    }
}