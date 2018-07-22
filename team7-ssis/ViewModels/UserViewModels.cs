using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace team7_ssis.Models
{
    public class EmailNameViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class ViewProfileViewModel
    {
        public string Title { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Supervisor { get; set; }
    }

    public class EditProfileViewModel
    {
        public string Title { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string TitleId { get; set; }
        [Display(Name = "Title")]
        public IEnumerable<SelectListItem> Titles { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string DepartmentCode { get; set; }
        public string Department { get; set; }
        [Display(Name = "Department")]
        public IEnumerable<SelectListItem> Departments { get; set; }
        public string SupervisorEmail { get; set; }
        public string Supervisor { get; set; }
        [Display(Name = "Supervisor")]
        public IEnumerable<SelectListItem> Supervisors { get; set; }
    }
}