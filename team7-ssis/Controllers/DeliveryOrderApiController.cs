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
        private PurchaseOrderService purchaseOrderService;

        public DeliveryOrderApiController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            itemService = new ItemService(context);
            purchaseOrderService = new PurchaseOrderService(context);
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


        //returns all deliveryorderdetails
        [Route("api/deliveryorderdetails/{deliveryorderno}")]
        [HttpGet]
        public List<DeliveryOrderDetailsViewModel> OutstandingItems(string donum)
        {
            DeliveryOrder deliveryOrder = deliveryOrderService.FindDeliveryOrderById(donum);
            List<DeliveryOrderDetail> dlist = new List<DeliveryOrderDetail>();
            foreach (DeliveryOrderDetail dod in deliveryOrder.DeliveryOrderDetails)
            {
                dlist.Add(dod);
            }

            return dlist.Select(x => new DeliveryOrderDetailsViewModel()
            {
                ItemCode = x.ItemCode,
                Description = x.Item.Description,
                QtyOrdered = x.PlanQuantity,
                ReceivedQty = x.ActualQuantity
            }).ToList();
        }


        [Route("api/purchaseorder/details/{purchaseorderno}")]
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
