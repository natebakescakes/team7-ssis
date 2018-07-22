using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;

namespace team7_ssis.Controllers
{
    public class DeliveryOrderController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        DeliveryOrderService deliveryOrderService = new DeliveryOrderService(context);
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);

        // GET: DeliveryOrder
        public ActionResult Index()
        {
            return View("ReceiveGoods");
        }

        public ActionResult OutstandingItems()
        {
            return View();
        }
    }
}