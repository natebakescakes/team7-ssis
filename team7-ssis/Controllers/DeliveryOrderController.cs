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
        private ApplicationDbContext context;
        private DeliveryOrderService deliveryOrderService;
        private PurchaseOrderService purchaseOrderService;
        private PurchaseOrderService purchaseOrderDetailService;
        private UserService userService;
        private StatusService statusService;
        private SupplierService supplierService;
       
       
        public DeliveryOrderController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            purchaseOrderService = new PurchaseOrderService(context);
            purchaseOrderDetailService = new PurchaseOrderService(context);
            userService = new UserService(context);
            statusService = new StatusService(context);
            supplierService = new SupplierService(context);
        }

        // GET: DeliveryOrder
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReceiveGoods()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ReceivedGoodsPurchaseOrderView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReceiveGoodsView(string ponum)
        {
            PurchaseOrderViewModel POVM = new PurchaseOrderViewModel();
            PurchaseOrder purchaseOrder = purchaseOrderDetailService.FindPurchaseOrderById(ponum);

            POVM.PurchaseOrderNo = purchaseOrder.PurchaseOrderNo;

            POVM.SupplierName = purchaseOrder.Supplier.Name;

            POVM.CreatedDate = purchaseOrder.CreatedDateTime.ToShortDateString();

            POVM.Status = purchaseOrder.Status.Name;

            return View(POVM);
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


        [HttpPost]

        public ActionResult Save(DeliveryOrderViewModel model)
        {
            bool status = false;

            DeliveryOrder deliveryOrder = new DeliveryOrder();
            PurchaseOrder purchaseOrder= purchaseOrderService.FindPurchaseOrderById(model.PurchaseOrderNo);
            deliveryOrder.PurchaseOrder = purchaseOrder;
            deliveryOrder.Supplier = supplierService.FindSupplierById(purchaseOrder.SupplierCode);
            //deliveryOrder.DeliveryOrderDetails=

            if (deliveryOrderService.FindDeliveryOrderById(model.DeliveryOrderNo) == null)

            {
                deliveryOrder.DeliveryOrderNo = IdService.GetNewDeliveryOrderNo(context);

                deliveryOrder.CreatedDateTime= DateTime.Now;

                deliveryOrder.CreatedBy= userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            }

            else

            {

              deliveryOrder = deliveryOrderService.FindDeliveryOrderById(model.DeliveryOrderNo);

              deliveryOrder.UpdatedDateTime = DateTime.Now;

              deliveryOrder.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            }

            deliveryOrder.InvoiceFileName = model.InvoiceFileName;

            deliveryOrder.DeliveryOrderFileName = model.DeliveryOrderFileName;

            deliveryOrder.Status = statusService.FindStatusByStatusId(1);

            if (deliveryOrderService.Save(deliveryOrder) != null) status = true;

            return new JsonResult { Data = new { status = status } };

        }

        [HttpPost]
        public ActionResult DOConfirmationView(string dno)
        {
            DeliveryOrderViewModel DOVM = new DeliveryOrderViewModel();
            DeliveryOrder deliveryOrder = deliveryOrderService.FindDeliveryOrderById(dno);
            DOVM.DeliveryOrderNo = dno;

            DOVM.PurchaseOrderNo = deliveryOrder.PurchaseOrder.PurchaseOrderNo;

            DOVM.SupplierName=deliveryOrder.Supplier.Name;

            DOVM.Status = deliveryOrder.Status.Name;

            DOVM.DeliverOrderFileName = deliveryOrder.DeliveryOrderFileName;

            DOVM.InvoiceFileName = deliveryOrder.InvoiceFileName;

            DOVM.CreatedBy = deliveryOrder.CreatedBy.FirstName + deliveryOrder.CreatedBy.LastName;

            DOVM.CreatedDate = deliveryOrder.CreatedDateTime;

            return View(DOVM);
        }
    }
}