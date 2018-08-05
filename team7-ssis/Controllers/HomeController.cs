using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
            Context = new ApplicationDbContext();
        }

        public ApplicationDbContext Context { get; set; }

        [Authorize]
        public ActionResult Index()
        {
            var department = new UserService(Context).FindUserByEmail(User.Identity.Name).Department;
            var representativeEmail = department.Representative != null ? department.Representative.Email : "";
            ViewBag.Representative = representativeEmail;
            Session["rep"] = representativeEmail;

            // If not Employee role or  Department Representative
            if (User.IsInRole("DepartmentHead") || representativeEmail == User.Identity.Name || User.IsInRole("Admin"))
                return RedirectToAction("ManageRequisitions", "Requisition");
            else if(User.IsInRole("Store Clerk") || User.IsInRole("Store Supervisor") || User.IsInRole("Store Manager"))
                return RedirectToAction("ManageRequisitions", "Requisition");
            return RedirectToAction("Unauthorized");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Inventory()
        {
            ViewBag.Message = "Manage Inventory";

            return View();
        }
    }
}