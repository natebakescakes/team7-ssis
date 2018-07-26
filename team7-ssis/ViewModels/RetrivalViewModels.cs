using System;
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
    }
    public class StationeryRetrievalTableViewModel
    {
        public bool AllRetrieved { get; set; }
        public string ProductID { get; set; }
        public string Bin { get; set; }
        public string Description { get; set; }
        public int QtyOrdered { get; set; }
    }
    public class StationeryRetrievalTableJSONViewModel
    {
        public string RetrievalID { get; set; }
        public List<StationeryRetrievalTableRowJSONViewModel> Data { get; set; }
    }
    public class StationeryRetrievalTableRowJSONViewModel
    {
        public bool AllRetrieved { get; set; }
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
    }
}