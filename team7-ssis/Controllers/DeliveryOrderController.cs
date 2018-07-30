using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using Microsoft.AspNet.Identity;
using System.IO;

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

        public ActionResult Save(List<DeliveryOrderDetailsViewModel> deliveryOrderDetailViewList)
        {
            DeliveryOrder deliveryOrder = new DeliveryOrder();

            deliveryOrder.DeliveryOrderDetails = new List<DeliveryOrderDetail>();

            deliveryOrder.PurchaseOrder = purchaseOrderService.FindPurchaseOrderById(deliveryOrderDetailViewList[0].PurchaseOrderNo);


            deliveryOrder.DeliveryOrderNo = IdService.GetNewDeliveryOrderNo(context);
            deliveryOrder.CreatedDateTime = DateTime.Now;


            deliveryOrder.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            deliveryOrder.Supplier = supplierService.FindSupplierById(deliveryOrder.PurchaseOrder.SupplierCode);


            string fileName = Path.GetFileNameWithoutExtension(deliveryOrderDetailViewList[0].DeliveryOrderFileName);

            string extension = Path.GetExtension(deliveryOrderDetailViewList[0].DeliveryOrderFileName);

            fileName = Path.Combine(Server.MapPath("~/Images/DeliveryOrder/") + fileName);

            deliveryOrder.DeliveryOrderFileName = fileName;


            string fileName1 = Path.GetFileNameWithoutExtension(deliveryOrderDetailViewList[0].InvoiceFileName);

            string extension1 = Path.GetExtension(deliveryOrderDetailViewList[0].InvoiceFileName);

            fileName1 = Path.Combine(Server.MapPath("~/Images/InvoiceFile/") + fileName1);

            deliveryOrder.InvoiceFileName = fileName;


            deliveryOrder.Status = statusService.FindStatusByStatusId(1);

            deliveryOrderService.Save(deliveryOrder);

            deliveryOrder.PurchaseOrder.DeliveryOrders.Add(deliveryOrder);

            deliveryOrderService.Save(deliveryOrder);
            


            foreach (DeliveryOrderDetailsViewModel dovm in deliveryOrderDetailViewList)
            {
                PurchaseOrderDetail purchaseOrderDetail = purchaseOrderService.FindPurchaseOrderDetailbyIdItem(deliveryOrder.PurchaseOrder.PurchaseOrderNo, dovm.ItemCode);

                if (dovm.CheckboxStatus == dovm.ItemCode)
                {
                    purchaseOrderDetail.Status = statusService.FindStatusByStatusId(3);
                    break;
                }

                if (dovm.ReceivedQty != 0)
                { 

                    DeliveryOrderDetail deliveryOrderDetail = new DeliveryOrderDetail();

                    deliveryOrderDetail.DeliveryOrderNo = deliveryOrder.DeliveryOrderNo;

                    deliveryOrderDetail.Item = itemService.FindItemByItemCode(dovm.ItemCode);

                    deliveryOrderDetail.ItemCode = dovm.ItemCode;

                    deliveryOrderDetail.PlanQuantity = dovm.QtyOrdered;

                    deliveryOrderDetail.ActualQuantity = dovm.ReceivedQty;

                    deliveryOrderDetail.Status = statusService.FindStatusByStatusId(1);

                    deliveryOrderDetail.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

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

            DOVM.Status = deliveryOrder.PurchaseOrder.Status.Name;

            DOVM.DeliverOrderFileName = deliveryOrder.DeliveryOrderFileName;

            DOVM.InvoiceFileName = deliveryOrder.InvoiceFileName;

            DOVM.CreatedBy = deliveryOrder.CreatedBy.FirstName + deliveryOrder.CreatedBy.LastName;

            DOVM.CreatedDate = deliveryOrder.CreatedDateTime;

            return View(DOVM);
        }
    }
}