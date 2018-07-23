using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace team7_ssis.Controllers
{
    public class StockAdjustmentController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        StockAdjustmentService stockAdjustmentService = new StockAdjustmentService(context);
        UserService userService = new UserService(context);
        UserRepository userRepository = new UserRepository(context);

        // GET: StockAdjustment
        public ActionResult Index()
         {
            return View();
          }
    
        public ActionResult Home()
        {
            List<ApplicationUser> supervisors = userService.FindSupervisorsByDepartment(
                userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName()).Department);

            List<ApplicationUser> managers = new List<ApplicationUser>() {
                userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName()).Department.Head
            };


            List<SelectListItem> listItem_supervicor = new List<SelectListItem>();
            foreach(ApplicationUser a in supervisors)
            {
                SelectListItem item1 = new SelectListItem()
                {
                    Value =a.Id,
                    Text =a.FirstName.ToString()+" "+a.LastName.ToString()
                };
                listItem_supervicor.Add(item1);
            }
            SelectList select1 = new SelectList(listItem_supervicor, "Value", "Text");
            ViewBag.select1 = select1;

            List<SelectListItem> listItem_managers = new List<SelectListItem>();

            foreach (ApplicationUser a in managers)
            {
                SelectListItem item2 = new SelectListItem()
                {
                    Value = a.Id,
                    Text = a.FirstName.ToString()+" "+a.LastName.ToString()
                };
                listItem_managers.Add(item2);
            }
            SelectList select2 = new SelectList(listItem_managers, "Value", "Text");
            ViewBag.select2 = select2;
            return View();
        }

        public ActionResult SaveAsDraft()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }


      
           

    }
}