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
        static ApplicationDbContext context = new ApplicationDbContext();
        DeliveryOrderService deliveryOrderService = new DeliveryOrderService(context);
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);
        ItemService itemService = new ItemService(context);


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


        [Route("api/receivegoods/{id}")]

        [HttpPost]

        public List<DeliveryOrderDetailsViewModel> DeliveryOrderDetails(string poNum)
        {

           // DeliveryOrder deliveryOrder = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo(poNum);

            DeliveryOrder deliveryOrder = deliveryOrderService.FindDeliveryOrderByPurchaseOrderNo("TEST");

            return deliveryOrder.DeliveryOrderDetails.Select(dod => new DeliveryOrderDetailsViewModel()

            {

                ItemCode = dod.Item.ItemCode,

                Description = dod.Item.Description,

                QtyOrdered = dod.PlanQuantity,

                ReceivedQty = 0

            }).ToList();

        }

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
    }
}
