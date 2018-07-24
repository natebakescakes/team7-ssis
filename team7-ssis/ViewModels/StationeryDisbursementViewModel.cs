using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace team7_ssis.ViewModels
{
    public class StationeryDisbursementViewModel
    {
        public string DisbursementID { get; set; }
        public string Department { get; set; }
        public string CollectionPoint { get; set; }
        public string DisbursedBy { get; set; }
        public string Status { get; set; }
    }
}