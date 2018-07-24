using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using Microsoft.AspNet.Identity;

namespace team7_ssis.Controllers
{
    public class DeliveryOrderController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        DeliveryOrderService deliveryOrderService = new DeliveryOrderService(context);
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);
        PurchaseOrderService purchaseOrderDetailService = new PurchaseOrderService(context);
        UserService userService = new UserService(context);
        StatusService statusService = new StatusService(context);
      
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //public ActionResult ReceiveGoodsView(string ponum)
        public ActionResult ReceiveGoodsView()
        {
            DeliveryOrderViewModel DOVM = new DeliveryOrderViewModel();
           // PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById(ponum);
            PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById("TEST");

            DOVM.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;

            DOVM.SupplierName = purchaseOrder.Supplier.Name;

            DOVM.CreatedDate = purchaseOrder.CreatedDateTime;

            DOVM.Status = purchaseOrder.Status.Name;

            return View(DOVM);
        }

        [HttpGet]
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

            return RedirectToAction("ReceiveGoodsView");
        }

        public ActionResult DOConfirmationView()
        {
            DeliveryOrderViewModel DOVM = new DeliveryOrderViewModel();
            //PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById(ponum);
            PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById("TEST");
            DeliveryOrder deliveryOrder = deliveryOrderService.FindDeliveryOrderById("TEST");

           // DOVM.DeliveryOrderNo=
            DOVM.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;
            DOVM.SupplierName = purchaseOrder.Supplier.Name;
            DOVM.CreatedDate = purchaseOrder.CreatedDateTime;
            DOVM.Status = purchaseOrder.Status.Name;
            DOVM.InvoiceFileName = deliveryOrder.InvoiceFileName;
            DOVM.DeliverOrderFileName = deliveryOrder.DeliveryOrderFileName;
            DOVM.CreatedBy = deliveryOrder.CreatedBy.FirstName +" " + deliveryOrder.CreatedBy.LastName;
        
            return View(DOVM);
        }

        [HttpPost]

        public ActionResult Submit(DeliveryOrderViewModel model)
        {
            bool status = false;

            DeliveryOrder deliveryOrder = new DeliveryOrder();

            if (deliveryOrderService.FindDeliveryOrderById(model.DeliveryOrderNo) == null)

            {
                deliveryOrder.DeliveryOrderNo = model.DeliveryOrderNo;

                deliveryOrder.CreatedDateTime= DateTime.Now;

                deliveryOrder.CreatedBy= userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            }

            else

            {

              deliveryOrder = deliveryOrderService.FindDeliveryOrderById(model.DeliveryOrderNo);

              deliveryOrder.UpdatedDateTime = DateTime.Now;

              deliveryOrder.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            }

            deliveryOrder.PurchaseOrder.PurchaseOrderNo = model.PurchaseOrderNo;

            deliveryOrder.Supplier.SupplierCode = model.SupplierCode;

            deliveryOrder.InvoiceFileName = model.InvoiceFileName;

            deliveryOrder.DeliveryOrderFileName = model.DeliveryOrderFileName;

            deliveryOrder.Status = statusService.FindStatusByStatusId(1);

            if (deliveryOrderService.Save(deliveryOrder) != null) status = true;

            return new JsonResult { Data = new { status = status } };

        }
    }
}