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
        public ApplicationDbContext context;
        PurchaseOrderService purchaseOrderService;
        StatusService statusService;

        public PurchaseOrderController()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            statusService = new StatusService(context);

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
            decimal totalAmount = 0;

            podModel.PNo = po.PurchaseOrderNo;
            podModel.SupplierName = po.Supplier.Name;
            podModel.CreatedDate = po.CreatedDateTime.ToShortDateString() + " " + po.CreatedDateTime.ToShortTimeString();
            podModel.Status = po.Status.Name;

            foreach(PurchaseOrderDetail pod in po.PurchaseOrderDetails)
            {
                totalAmount = totalAmount + purchaseOrderService.FindTotalAmountByPurchaseOrderDetail(pod);
            }
            ViewBag.Amount = totalAmount;
            return View(podModel);

        }

        [HttpPost]
        public string Save(string purchaseOrderNum, string itemCode, int quantity)
        {
            PurchaseOrder purchaseOrder = purchaseOrderService.FindPurchaseOrderById(purchaseOrderNum);
            foreach(PurchaseOrderDetail pod in purchaseOrder.PurchaseOrderDetails)
            {
                if (itemCode == pod.ItemCode)
                {
                    pod.Quantity = quantity;
                    purchaseOrderService.Save(purchaseOrder);
                    break;
                }
            }

            return "Updated";
        }


        [HttpPost]
        public string Delete(string purchaseOrderNum, string itemCode)
        {
            
            string[] itemCodeArray = new string[] { itemCode };

            PurchaseOrder purchaseOrder = purchaseOrderService.FindPurchaseOrderById(purchaseOrderNum);

            purchaseOrderService.DeleteItemFromPurchaseOrder(purchaseOrder,itemCodeArray);

            return "Deleted";
        }


        [HttpPost]
        public string Cancel(string purchaseOrderNum)
        {
            PurchaseOrder purchaseOrder = purchaseOrderService.FindPurchaseOrderById(purchaseOrderNum);
            purchaseOrder.Status = statusService.FindStatusByStatusId(2);
            purchaseOrderService.Save(purchaseOrder);

            return "Cancelled";
        }

        public ActionResult Generate()
        {
            return View();
        }



    }
}