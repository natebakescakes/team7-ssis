using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using team7_ssis.Services;


namespace team7_ssis.Controllers
{ 
    public class PurchaseOrderController : Controller
    {
        private ApplicationDbContext context;
        private PurchaseOrderService purchaseOrderService;

        public PurchaseOrderController()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
        }

        public ActionResult Index()
        {
            return View("Manage");
        }


        [HttpPost]
        public ActionResult Details(string poNum)
        {
            PurchaseOrder po = purchaseOrderService.FindPurchaseOrderById(poNum);
            PurchaseOrderViewModel podModel = new PurchaseOrderViewModel();

            podModel.PurchaseOrderNo = po.PurchaseOrderNo;
            podModel.SupplierName = po.Supplier.Name;
            podModel.CreatedDate = po.CreatedDateTime.ToShortDateString() + " " + po.CreatedDateTime.ToShortTimeString();
            podModel.Status = po.Status.Name;

            return View(podModel);
        }




    }
}