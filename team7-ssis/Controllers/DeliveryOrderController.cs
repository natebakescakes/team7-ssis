using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class DeliveryOrderController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        DeliveryOrderService deliveryOrderService = new DeliveryOrderService(context);
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);
        PurchaseOrderService purchaseOrderDetailService = new PurchaseOrderService(context);
      

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //public ActionResult PurchaseOrderDetails(string ponum)
        public ActionResult PurchaseOrderDetails()
        {
            DeliveryOrderViewModel DOVM = new DeliveryOrderViewModel();
            //PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById(ponum);
            PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById("TEST");

            DOVM.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;

            DOVM.SupplierName = purchaseOrder.Supplier.Name;

            DOVM.OrderDate = purchaseOrder.CreatedDateTime;

            DOVM.Status = purchaseOrder.Status.Name;

            return View(DOVM);
        }

        public ActionResult ReceiveGoods()
        {
            return View();
        }


        [HttpPost]

        public ActionResult ImageUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    int i = deliveryOrderService.UploadDeliveryOrderFile(file);
                    if (i == 1)
                    {
                        ViewBag.Message = "File uploaded successfully";
                    }

                    else
                    {
                        ViewBag.Message = "File uploading Unsuccessful";
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }

            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return RedirectToAction("Index");
        }

        public ActionResult DOConfirmationPage()
        {
            DeliveryOrderViewModel DOVM = new DeliveryOrderViewModel();
            //PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById(ponum);
            PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById("TEST");

            DOVM.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;
            DOVM.SupplierName = purchaseOrder.Supplier.Name;
            DOVM.OrderDate = purchaseOrder.CreatedDateTime;
            DOVM.Status = purchaseOrder.Status.Name;
            //DOVM.InvoiceFileName=
            //DOVM.DeliverOrderFileName=
            //DOVM.CreatedBy=
        
            return View(DOVM);
        }
    }
}