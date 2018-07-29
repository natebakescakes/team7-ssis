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
        private ApplicationDbContext context;
        private DeliveryOrderService deliveryOrderService;
        private PurchaseOrderService purchaseOrderService;

        public DeliveryOrderController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            purchaseOrderService = new PurchaseOrderService(context);
        }

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