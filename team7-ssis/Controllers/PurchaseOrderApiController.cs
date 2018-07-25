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

        public PurchaseOrderApiController()
        {
            context = new ApplicationDbContext();
            purchaseOrderService = new PurchaseOrderService(context);
        }

        [Route("api/purchaseOrder/all")]
        [HttpGet]
        public List<PurchaseOrderViewModel> PurchaseOrders()
        {
            return purchaseOrderService.FindAllPurchaseOrders().Select(po => new PurchaseOrderViewModel()
            {
                PNo = po.PurchaseOrderNo,
                SupplierName=po.Supplier.Name,
                CreatedDate=po.CreatedDateTime.ToShortDateString() + " "+ po.CreatedDateTime.ToShortTimeString(),
                Status=po.Status.Name
            }).ToList();
        }

        [Route("api/purchaseOrder/{id}")]
        [HttpPost]
        public List<PurchaseOrderDetailsViewModel> PurchaseOrderDetails(string poNum)
        {
            PurchaseOrder po = purchaseOrderService.FindPurchaseOrderById(poNum);
            return po.PurchaseOrderDetails.Select(pod => new PurchaseOrderDetailsViewModel()
            {
                ItemCode = pod.Item.ItemCode,
                Description = pod.Item.Description,
                QuantityOrdered = pod.Quantity,
                UnitPrice = pod.Item.ItemPrices.Where(x => x.PrioritySequence == 1).First().Price,
                Amount = pod.Quantity * pod.Item.ItemPrices.Where(x => x.PrioritySequence == 1).First().Price,
                RemainingQuantity=0,
               // ReceivedQuantity = po.DeliveryOrders.ForEach(x => x.DeliveryOrderDetails.Where(y=>y.Item.ItemCode==pod.Item.ItemCode).Select(z => z!=null?z.PlanQuantity:0),
               // ReceivedQuantity = po.DeliveryOrders
                


            }).ToList();

        }




    }
}
