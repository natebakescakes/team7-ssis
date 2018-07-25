using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;


namespace team7_ssis.Controllers
{
    public class DeliveryOrderApiController : ApiController
    {
        private ApplicationDbContext context;
        private DeliveryOrderService deliveryOrderService;
        private ItemService itemService;

        public DeliveryOrderApiController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            itemService = new ItemService(context);
        }


        [Route("api/receivegoods/all")]
        [HttpGet]
        public List<DeliveryOrderViewModel> DeliveryOrders()
        {
            return deliveryOrderService.FindAllDeliveryOrders().Select(x => new DeliveryOrderViewModel()
            {
                DeliveryOrderNo = x.DeliveryOrderNo,

                PurchaseOrderNo = x.PurchaseOrder.PurchaseOrderNo,

                SupplierName = x.Supplier.Name,

                CreatedDate = x.PurchaseOrder.CreatedDateTime,

                Status = x.PurchaseOrder.Status.Name

            }).ToList();
        }

        // return all pending po - attributes
        [Route("api/outstandingpo/all")]
        [HttpGet]
        public List<PurchaseOrderViewModel> PurchaseOrders()
        {
            int[] myIntArray = new int[] { 11, 12 };

            return purchaseOrderService.FindPurchaseOrderByStatus(myIntArray).Select(x => new PurchaseOrderViewModel()
            {
                PurchaseOrderNo = x.PurchaseOrderNo,

                SupplierName = x.Supplier.Name,

                CreatedDate = x.CreatedDateTime.ToShortDateString(),

                Status = x.Status.Name

            }).ToList();
        }


        //returns all outstanding items
        [Route("api/outstandingitems/all")]
        [HttpGet]
        public List<PurchaseOrderDetailsViewModel> OutstandingItems()
        {
            int[] myIntArray = new int[] { 11, 12 };
            List<PurchaseOrder> list = purchaseOrderService.FindPurchaseOrderByStatus(myIntArray);
            List<PurchaseOrderDetail> plist = new List<PurchaseOrderDetail>();
            foreach (PurchaseOrder po in list)
            {
                foreach (PurchaseOrderDetail pod in po.PurchaseOrderDetails)
                {
                    plist.Add(pod);
                }
            }

            return plist.Select(x => new PurchaseOrderDetailsViewModel()
            {
                ItemCode = x.ItemCode,
                Description = x.Item.Description,
                QuantityOrdered = x.Quantity
            }).ToList();
        }


        [Route("api/purchaseOrder/details/{purchaseOrderNo}")]
        [HttpGet]

        public List<PurchaseOrderDetailsViewModel> PurchaseOrderDetails(string purchaseOrderNo)
        {
            PurchaseOrder po = purchaseOrderService.FindPurchaseOrderById(purchaseOrderNo);

            return po.PurchaseOrderDetails.Select(pod => new PurchaseOrderDetailsViewModel()

            {
                ItemCode = pod.Item.ItemCode,

                Description = pod.Item.Description,

                QuantityOrdered = pod.Quantity,

                ReceivedQuantity = purchaseOrderService.FindReceivedQuantityByPurchaseOrderDetail(pod),

                RemainingQuantity = purchaseOrderService.FindRemainingQuantity(pod)

            }).ToList();
        }
    }
}
