using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace team7_ssis.ViewModels
{
    public class DepartmentViewModel
    {
        //[Display(Name = "Collection Point")]
        //public int? SelectedCollectionPoint { get; set; }
        //[Display(Name = "Representative")]
        //public int? SelectedRepresentative { get; set; }
        //[Display(Name = "Manager Role")]
        //public int? SelectedManager { get; set; }
        //public SelectList collectionPointList {get;set;}
        //public SelectList usersByDepartmentList { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public string ContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public int Status { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; }
        public string DepartmentHead { get; set; }
        public string DepartmentRep { get; set; }
        public string CollectionPoint { get; set; }
        public string DelegateManager { get; set; }
        public IEnumerable<SelectListItem> UsersByDepartment { get; set; }
        public IEnumerable<SelectListItem> AllUsers { get; set; }
        public IEnumerable<SelectListItem> CollectionPoints { get; set; }
        public string Email { get; set; }



    }
}