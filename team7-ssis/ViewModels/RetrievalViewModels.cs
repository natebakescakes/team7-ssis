﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StationeryRetrievalViewModel
    {
        public string RetrievalID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
        public int StatusId { get; set; }
    }
    public class StationeryRetrievalTableViewModel
    {
        public string RetrievedStatus { get; set; }
        public string ProductID { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
    }
    public class StationeryRetrievalJSONViewModel
    {
        public string RetrievalID { get; set; }
        public List<StationeryRetrievalTableRowJSONViewModel> Data { get; set; }
    }
    public class StationeryRetrievalTableRowJSONViewModel
    {
        public string RetrievedStatus { get; set; }
        public string ProductID { get; set; }
    }
    public class RetrievalDetailsJSON
    {
        public string retId { get; set; }
        public string itemId { get; set; }
    }
    public class RetrievalDetailsViewModel
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Bin { get; set; }
    }
    public class RetrievalDetailsTableViewModel
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public int Needed { get; set; }
        public int Actual { get; set; }
    }
    public class BreakdownByDepartment
    {
        public string DeptId { get; set; }
        public int Actual { get; set; }
    }
    public class SaveJson
    {
        public string RetId { get; set; }
        public string ItemCode { get; set; }

        public List<BreakdownByDepartment> List { get; set; }
    }
    public class ManageRetrievalsViewModel
    {
        public string RetrievalId { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
    }

    public class RetrievalMobileViewModel : ManageRetrievalsViewModel
    {
        public List<RetrievalDetailByDeptViewModel> RetrievalDetails { get; set; }
    }

    public class RetrievalDetailByDeptViewModel
    {
        public string Department { get; set; }
        public string DepartmentCode { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Bin { get; set; }
        public string Uom { get; set; }
        public string Status { get; set; }
        public string RetrievalStatus { get; set; }
        public int PlanQuantity { get; set; }
        public int ActualQuantity { get; set; }
    }

    public class UpdateActualQuantityViewModel
    {
        public string RetrievalId { get; set; }
        public string Email { get; set; }
        public string ItemCode { get; set; }
        public List<BreakdownByDepartment> RetrievalDetails { get; set; }
    }

    public class ConfirmRetrievalViewModel
    {
        public string RetrievalId { get; set; }
        public string Email { get; set; }
        public string ItemCode { get; set; }
    }
}