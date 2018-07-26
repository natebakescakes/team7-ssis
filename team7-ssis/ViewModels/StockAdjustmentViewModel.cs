using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{

    public class ViewModelOfDetail
    {
        public string Itemcode;
        public string Reason;
        public int Adjustment;
    }

    public class StockAdjustmentViewModel
    {
        public string StockAdjustmentId { get; set; }  
        public string CreatedBy { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public  string ApprovedBySupervisor { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public  string  StatusName { get; set; }
        public  string Link { get; set; }

        public List<ApplicationUser> supervisors { get; set; }

        public List<ApplicationUser> managers { get; set; }



    }
    public class StockAdjustmentListViewModel
    {
        public List<StockAdjustmentViewModel> StockAdjustments { get; set; }
        // public string UserName { get; set; } //current login user name
    }

    public class StockAdjustmentDetailViewModel
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }

        public string PriceColor { get; set; }
        public string UnitPrice { get; set; }
        public string Adjustment { get; set; }

    }

    public class StockAdjustmentDetailListViewModel
    {
        public StockAdjustmentViewModel StockAdjustmentModel { get; set; }
        public List<StockAdjustmentDetailViewModel> StockAdjustmentDetailsModel { get; set; }

    }



}