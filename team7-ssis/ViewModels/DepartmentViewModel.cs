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
        [Required(ErrorMessage = "Please select a collection point")]
        [Display(Name = "Collection Point")]
        public int? SelectedCollectionPoint { get; set; }
        public SelectList collectionPointList {get;set;}
        public SelectList usersByDepartmentList { get; set; }
        
    }
}