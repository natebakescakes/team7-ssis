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
        ApplicationDbContext context;
        StockAdjustmentService stockAdjustmentService;
        UserService userService;
        
        ItemPriceService itemPriceService;
        public StockAdjustmentController()
        {
            context =new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);
           
            itemPriceService = new ItemPriceService(context);
        }
       

        // GET: StockAdjustment
        public ActionResult Index()
        {
            return View();
        }

   

        public ActionResult New()
        {
            StockAdjustmentViewModel viewmodel = new StockAdjustmentViewModel();
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            Department d = userService.FindUserByEmail(UserName).Department;

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


        public ActionResult AddItem()
        {
            string user = System.Web.HttpContext.Current.User.Identity.GetUserId();
            List<string> Session_list = new List<string>();
            if (System.Web.HttpContext.Current.Session[user+"stock"] != null) //already has sessionID 
            {
                Session_list = (List<string>)System.Web.HttpContext.Current.Session[user+"stock"];
            }
            else
            {
                System.Web.HttpContext.Current.Session[user+"stock"] = Session_list;
            }

            return View();
        }

        public ActionResult Process(string Id)
        {
            //get the stockadjustment
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(Id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            sv.ApprovedBySupervisor = sa.ApprovedBySupervisor == null ? "" : sa.ApprovedBySupervisor.FirstName + " "
                + sa.ApprovedBySupervisor.LastName;
            return View(sv);
        }
        public ActionResult Details(string id)
        {
            //get the stockadjustment
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            sv.ApprovedBySupervisor = sa.ApprovedBySupervisor == null ? "" : sa.ApprovedBySupervisor.FirstName + " "
                + sa.ApprovedBySupervisor.LastName;           
            return View(sv);
        }

        public ActionResult DetailsNoEdit(string id)
        {
            //get the stockadjustment
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            sv.ApprovedBySupervisor = sa.ApprovedBySupervisor == null ? "" : sa.ApprovedBySupervisor.FirstName + " "
                + sa.ApprovedBySupervisor.LastName;
            
            return View(sv);
        }


        public ActionResult DetailsEdit(string Id)
        {
            //get the stockadjustment
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(Id);
            StockAdjustmentViewModel sv = new StockAdjustmentViewModel();
            sv.StockAdjustmentId = sa.StockAdjustmentId;
            sv.CreatedBy = (sa.CreatedBy == null) ? "" : sa.CreatedBy.FirstName + " " + sa.CreatedBy.LastName;
            sv.CreatedDateTime = sa.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
            sv.ApprovedBySupervisor = sa.ApprovedBySupervisor == null ? "" : sa.ApprovedBySupervisor.FirstName + " "
                + sa.ApprovedBySupervisor.LastName;
            return View(sv);
        }

    }
}