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
        private ItemService itemService;


        public DeliveryOrderController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            purchaseOrderService = new PurchaseOrderService(context);
            purchaseOrderDetailService = new PurchaseOrderService(context);
            userService = new UserService(context);
            statusService = new StatusService(context);
            supplierService = new SupplierService(context);
            itemService = new ItemService(context);
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

        [HttpPost]
        public ActionResult ReceivedGoodsPurchaseOrderView(string pon)
        {
            PurchaseOrderViewModel POVM = new PurchaseOrderViewModel();

            POVM.PurchaseOrderNo = pon;

            return View(POVM);
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
                   string path = deliveryOrderService.UploadDeliveryOrderFile(file);
                   string result = System.IO.Path.GetFileName(path);
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

        public ActionResult Save(List<DeliveryOrderDetailsViewModel> deliveryOrderDetailViewList)
        {
           // purchaseOrderDetail = deliveryOrderService.FindPurchaseOrderDetailbyIdItem(dovm.PurchaseOrderNo, dovm.ItemCode);


            DeliveryOrder deliveryOrder = new DeliveryOrder();
            deliveryOrder.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            deliveryOrder.PurchaseOrder = purchaseOrderService.FindPurchaseOrderById(deliveryOrderDetailViewList[0].PurchaseOrderNo);
            deliveryOrder.PurchaseOrder.DeliveryOrders.Add(deliveryOrder);

           // PurchaseOrderDetail purchaseOrderDetail;


            deliveryOrder.DeliveryOrderNo = IdService.GetNewDeliveryOrderNo(context);

            deliveryOrder.CreatedDateTime= DateTime.Now;

            deliveryOrder.CreatedBy= userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            deliveryOrder.Supplier = supplierService.FindSupplierById(deliveryOrder.PurchaseOrder.SupplierCode);
            //deliveryOrder.InvoiceFileName = model.InvoiceFileName;

            //deliveryOrder.DeliveryOrderFileName = model.DeliveryOrderFileName;

            deliveryOrder.Status = statusService.FindStatusByStatusId(1);
            deliveryOrderService.Save(deliveryOrder);


            foreach (DeliveryOrderDetailsViewModel dovm in deliveryOrderDetailViewList)
            {
                

                if (dovm.ReceivedQty != 0)
                    {

                    DeliveryOrderDetail deliveryOrderDetail = new DeliveryOrderDetail();

                    deliveryOrderDetail.DeliveryOrderNo = deliveryOrder.DeliveryOrderNo;
                    deliveryOrderDetail.Item = itemService.FindItemByItemCode(dovm.ItemCode);
                    deliveryOrderDetail.ItemCode = dovm.ItemCode;
                    deliveryOrderDetail.PlanQuantity = dovm.QtyOrdered;
                    deliveryOrderDetail.ActualQuantity = dovm.ReceivedQty;
                    deliveryOrderDetail.Status = statusService.FindStatusByStatusId(1);
                    deliveryOrderDetail.UpdatedBy= userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    deliveryOrderDetail.UpdatedDateTime = DateTime.Now;
                    deliveryOrder.DeliveryOrderDetails.Add(deliveryOrderDetail);
                    
                }
            }

            deliveryOrderService.CheckSave(deliveryOrder);

            return new JsonResult { Data = new { status = "Saved" } };

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