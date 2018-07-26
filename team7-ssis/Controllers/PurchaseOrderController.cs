using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using team7_ssis.Services;
using Microsoft.AspNet.Identity;


namespace team7_ssis.Controllers
{ 
    public class PurchaseOrderController : Controller
    {
        private ApplicationDbContext context;
        private PurchaseOrderService purchaseOrderService;
        StatusService statusService;
        ItemService itemService;
        UserService userService;
        ItemPriceService itemPriceService;
        PurchaseOrderService purchaseOrderDetail;

        public PurchaseOrderController()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            statusService = new StatusService(context);
            itemService = new ItemService(context);
            itemPriceService = new ItemPriceService(context);
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
        public string Update(string purchaseOrderNum, string itemCode, int quantity)
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




        [HttpPost]
        public ActionResult Save(List<PurchaseOrderDetailsViewModel> purchaseOrderDetailList)
        {
            List<Supplier> supList = new List<Supplier>();
            foreach(PurchaseOrderDetailsViewModel pod in purchaseOrderDetailList)
            {
                Item item=itemService.FindItemByItemCode(pod.ItemCode);
                ItemPrice itemPrice= itemPriceService.FindSingleItemPriceByPriority(item, pod.SupplierPriority);

                if (!supList.Contains(itemPrice.Supplier))
                {
                    supList.Add(itemPrice.Supplier);
                }  
            }


            List<PurchaseOrder> poList = purchaseOrderService.CreatePOForEachSupplier(supList);


            List<string> purchaseOrderIds = new List<string>();
            foreach(PurchaseOrder pOrder in poList)
            {
                purchaseOrderIds.Add(pOrder.PurchaseOrderNo);
            }
            
            foreach (PurchaseOrderDetailsViewModel pod in purchaseOrderDetailList)
            {
                PurchaseOrderDetail poDetail = new PurchaseOrderDetail();
                poDetail.ItemCode = pod.ItemCode;
                poDetail.Quantity = pod.QuantityOrdered;
                poDetail.Status = statusService.FindStatusByStatusId(11);
                poDetail.Status.StatusId = 11;
                poDetail.UpdatedDateTime = DateTime.Now;
                poDetail.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                
                foreach(PurchaseOrder po in poList)
                {
                    Item item = itemService.FindItemByItemCode(pod.ItemCode);
                    ItemPrice itemPrice = itemPriceService.FindSingleItemPriceByPriority(item, pod.SupplierPriority);
                    if (itemPrice.SupplierCode == po.SupplierCode)
                    {
                        poDetail.PurchaseOrder = po; 
                        po.PurchaseOrderDetails.Add(poDetail);

                        purchaseOrderService.Save(po);
                        break;
                    }
                }

                purchaseOrderDetail.SavePurchaseOrderDetail(poDetail);
                
            }

            return View("Success",purchaseOrderIds);
            

        }

    }
}