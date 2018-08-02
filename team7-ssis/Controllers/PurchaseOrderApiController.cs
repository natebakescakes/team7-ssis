using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using team7_ssis.Services;


namespace team7_ssis.Controllers
{
    public class PurchaseOrderApiController : ApiController
    {
        private ApplicationDbContext context;
        private PurchaseOrderService purchaseOrderService;
        private ItemPriceService itemPriceService;

        public PurchaseOrderApiController()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
            itemPriceService = new ItemPriceService(context);

        }



        [Route("api/purchaseOrder/all")]
        [HttpGet]
        public List<PurchaseOrderViewModel> PurchaseOrders()
        {
            if (purchaseOrderService.FindAllPurchaseOrders().Count != 0)
            {
                return purchaseOrderService.FindAllPurchaseOrders().Select(po => new PurchaseOrderViewModel()
                {
                    PurchaseOrderNo = (po.PurchaseOrderNo!=null)?po.PurchaseOrderNo:"",
                    SupplierName = (po.Supplier.Name!=null)?po.Supplier.Name:"",
                    CreatedDate = (po.CreatedDateTime!=null)?po.CreatedDateTime.ToShortDateString() + " " + po.CreatedDateTime.ToShortTimeString():"",
                    Status = (po.Status.Name!=null)?po.Status.Name:""
                }).ToList();
            }
            else
                return new List<PurchaseOrderViewModel>();
            
        }


        [Route("api/purchaseOrder/details/{purchaseOrderNo}")]
        [HttpGet]
        public List<PurchaseOrderDetailsViewModel> PurchaseOrderDetails(string purchaseOrderNo)
        {
            PurchaseOrder po = purchaseOrderService.FindPurchaseOrderById(purchaseOrderNo);

            return po.PurchaseOrderDetails.Select(pod => new PurchaseOrderDetailsViewModel()
            {
                ItemCode = (pod.Item.ItemCode!=null)? pod.Item.ItemCode:"",
                Description = (pod.Item.Description !=null)? pod.Item.Description:"",
                QuantityOrdered = pod.Quantity,
                UnitPrice = purchaseOrderService.FindUnitPriceByPurchaseOrderDetail(pod),
                Amount = purchaseOrderService.FindTotalAmountByPurchaseOrderDetail(pod),
                ReceivedQuantity = purchaseOrderService.FindReceivedQuantityByPurchaseOrderDetail(pod),
                RemainingQuantity = purchaseOrderService.FindRemainingQuantity(pod),
                Status = (pod.Status.Name!=null)?pod.Status.Name:""


            }).ToList();

        }

        [Route("api/purchaseOrder/getsupplier")]
        [HttpPost]
        public List<SupplierViewModel> GetItemSupplier([FromBody]string itemCode)
        {
            List<SupplierViewModel> suppliers = new List<SupplierViewModel>();
            List<ItemPrice> itemPriceList = itemPriceService.FindItemPriceByItemCode(itemCode);

            foreach (ItemPrice i in itemPriceList)
            {
                suppliers.Add(new SupplierViewModel {Name=i.Supplier.Name, Priority =i.PrioritySequence });
            }
            
            return suppliers;

        }


        [Route("api/purchaseOrder/success")]
        [HttpPost]
        public List<PurchaseOrderViewModel> Success([FromBody]string poNums)
         {
            List<PurchaseOrderViewModel> purchaseOrders = new List<PurchaseOrderViewModel>();

            if (poNums != null)
            {
                string[] PONums = poNums.Split(',');

                foreach (string s in PONums)
                {
                    PurchaseOrder p = purchaseOrderService.FindPurchaseOrderById(s);
                    PurchaseOrderViewModel purchaseOrder = new PurchaseOrderViewModel();
                    purchaseOrder.PurchaseOrderNo = (s!=null)?s:"";
                    purchaseOrder.SupplierName = (p.Supplier.Name!=null)?p.Supplier.Name:"";

                    purchaseOrders.Add(purchaseOrder);
                }
            }
            
            return purchaseOrders;

        }

        

        }
    
}
