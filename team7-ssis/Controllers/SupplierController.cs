using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class SupplierController : Controller
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        SupplierService supplierService = new SupplierService(context);

        // GET: Supplier
        public ActionResult Index()
        {
            return View();
        }
    }
}
