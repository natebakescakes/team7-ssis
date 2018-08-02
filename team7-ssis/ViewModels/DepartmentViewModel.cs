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
     
        public int SelectedCollectionPoint { get; set; }
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
        public string DelegationRecipient { get; set; }
        public IEnumerable<SelectListItem> UsersByDepartment { get; set; }
        public IEnumerable<SelectListItem> AllUsers { get; set; }
        public IEnumerable<SelectListItem> CollectionPoints { get; set; }
        public string EmailHead { get; set; }
        public string EmailRep { get; set; }
        public int CollectionPointId { get; set; }

        //Delegation properties
        public string Recipient { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int DelegationId { get; set; }
        public int DelegationStatus { get; set; }
     
    }

    public class DepartmentOptionsViewModel
    {
        public string Department { get; set; }
        public string Representative { get; set; }
        public List<DelegationMobileViewModel> Delegations { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }
    }

    public class EmployeeViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ChangeRepresentativeViewModel
    {
        public string RepresentativeEmail { get; set; }
        public string HeadEmail { get; set; }
    }
}