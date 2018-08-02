using System;
using System.Collections.Generic;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;
using System.Web;
using Microsoft.AspNet.Identity;
namespace team7_ssis.Controllers
{
    public class StockAdjustmentController : Controller
    {
        public ApplicationDbContext context { get; set; }
        StockAdjustmentService stockAdjustmentService;
        UserService userService;        
        public String CurrentUserName { get; set; }
        public StockAdjustmentController()
        {
            context =new ApplicationDbContext();
            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }
  
        }


        // GET: StockAdjustment
        public ActionResult Index(string create)
        {
            return View();
        }



        public ActionResult New()
        {

            userService = new UserService(context);
            StockAdjustmentViewModel viewmodel = new StockAdjustmentViewModel();
         
            Department d = userService.FindUserByEmail(CurrentUserName).Department;

            List<ApplicationUser> supervisors=new List<ApplicationUser>();

            List<ApplicationUser> managers =new List<ApplicationUser>();

            if (d != null)
            {
                supervisors = userService.FindSupervisorsByDepartment(d);
                managers = new List<ApplicationUser>() { d.Head };
            }

            viewmodel.supervisors = supervisors;
            viewmodel.managers = managers;
            
            return View(viewmodel);
        }      


        public ActionResult Process(string Id)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
           //get the stockadjustment
           StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(Id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager == null)
            {
                sv.ApprovedBySupervisor = "";
            }
            else if (sa.ApprovedBySupervisor != null && sa.ApprovedByManager == null)
            { sv.ApprovedBySupervisor = sa.ApprovedBySupervisor.FirstName + " " + sa.ApprovedBySupervisor.LastName; }
            else if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager != null)
            {
                sv.ApprovedBySupervisor = sa.ApprovedByManager.FirstName + " " + sa.ApprovedByManager.LastName;
            }
            if (sa.UpdatedDateTime != null)
            {
                DateTime updatetime = (DateTime)sa.UpdatedDateTime;
                sv.UpdateDateTime = updatetime.ToString("yyyy-MM-dd HH: mm:ss");
            }
            else { }
            return View(sv);
        }

        public ActionResult DetailsNoEdit(string id)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
           //get the stockadjustment
           StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");

            if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager == null)
            {
                sv.ApprovedBySupervisor = "";
            }
            else if (sa.ApprovedBySupervisor != null && sa.ApprovedByManager == null)
            { sv.ApprovedBySupervisor = sa.ApprovedBySupervisor.FirstName + " " + sa.ApprovedBySupervisor.LastName; }
            else if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager != null)
            {
                sv.ApprovedBySupervisor = sa.ApprovedByManager.FirstName + " " + sa.ApprovedByManager.LastName;
                    }

            if (sa.UpdatedDateTime != null)
            {
                DateTime updatetime = (DateTime)sa.UpdatedDateTime;
                sv.UpdateDateTime = updatetime.ToString("yyyy-MM-dd HH: mm:ss");
            }
            else { }

            return View(sv);
        }


        public ActionResult DetailsEdit(string Id)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);
            //get the stockadjustment
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(Id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager == null)
            {
                sv.ApprovedBySupervisor = "";
            }
            else if (sa.ApprovedBySupervisor != null && sa.ApprovedByManager == null)
            { sv.ApprovedBySupervisor = sa.ApprovedBySupervisor.FirstName + " " + sa.ApprovedBySupervisor.LastName; }
            else if (sa.ApprovedBySupervisor == null && sa.ApprovedByManager != null)
            {
                sv.ApprovedBySupervisor = sa.ApprovedByManager.FirstName + " " + sa.ApprovedByManager.LastName;
            }
            if (sa.UpdatedDateTime != null)
            {
                DateTime updatetime = (DateTime)sa.UpdatedDateTime;
                sv.UpdateDateTime = updatetime.ToString("yyyy-MM-dd HH: mm:ss");
            }
            else { }

            //string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Department d = userService.FindUserByEmail(CurrentUserName).Department;

            List<ApplicationUser> supervisors = new List<ApplicationUser>();

            List<ApplicationUser> managers = new List<ApplicationUser>();

            if (d != null)
            {
                supervisors = userService.FindSupervisorsByDepartment(d);
                managers = new List<ApplicationUser>() { d.Head };
            }

            sv.supervisors = supervisors;
            sv.managers = managers;

            return View(sv);
        }

    }
}