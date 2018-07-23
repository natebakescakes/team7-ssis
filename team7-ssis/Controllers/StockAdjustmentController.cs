using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.Tests.Services;

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
            ApplicationUser user = userRepository.FindById("166ad865 - 7f9a - 4673 - 8838 - 68e51e0fcd3e");
            ViewBag.supervisors = userService.listSupervisorsByUser(user);
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