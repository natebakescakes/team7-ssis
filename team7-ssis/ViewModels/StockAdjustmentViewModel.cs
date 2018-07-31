﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.ViewModels
{

    public class ViewModelFromNew
    {       
        public string Itemcode;
        public string Reason;
        public int Adjustment;
        public string Unitprice;
        public string Supervisor;
        public string Manager;
    }

    public class ViewModelFromEditDetail
    {
        public string StockAdjustmentID;
        public string Itemcode;
        public string Reason;
        public int Adjustment;
        public string Unitprice;
        public string Supervisor;
        public string Manager;
    }

    public class StockAdjustmentViewModel
    {
        public string StockAdjustmentId { get; set; }  
        public string CreatedBy { get; set; }
        public string UpdateDateTime { get; set; }
        public  string ApprovedBySupervisor { get; set; }
        public string CreatedDateTime { get; set; }
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
        public int  Adjustment { get; set; }

    }

    public class StockAdjustmentDetailListViewModel
    {
        public StockAdjustmentViewModel StockAdjustmentModel { get; set; }
        public List<StockAdjustmentDetailViewModel> StockAdjustmentDetailsModel { get; set; }

    }

    public class MobileSADViewModel
    {
        public string ItemCode { get; set; }

        public string Reason { get; set; }

        public string UserName { get; set; }

        public int QuantityAdjusted { get; set; }


    }

    public class StockAdjustmentRequestViewModel
    {
        public string StockAdjustmentId { get; set; }
        public string RequestorName { get; set; }
        public string RequestedDate { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public List<StockAdjustmentRequestDetailViewModel> StockAdjustmentRequestDetails { get; set; }
    }

    public class StockAdjustmentRequestDetailViewModel
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string OriginalQuantity { get; set; }
        public string AfterQuantity { get; set; }
        public string Reason { get; set; }
    }

    public class StockAdjustmentIdViewModel
    {
        public string StockAdjustmentId { get; set; }
        public string Email { get; set; }
    }
}