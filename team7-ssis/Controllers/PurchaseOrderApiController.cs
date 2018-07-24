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
        public static ApplicationDbContext context = new ApplicationDbContext();
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);
        

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

        
        
        public List<PurchaseOrderDetailsViewModel> GetPurchaseOrder(string poNum)
        {
            PurchaseOrder po = purchaseOrderService.FindPurchaseOrderById(poNum);

            return po.PurchaseOrderDetails.Select(pod => new PurchaseOrderDetailsViewModel()
            {
                ItemCode = pod.Item.ItemCode,
                Description = pod.Item.Description,
                QuantityOrdered = pod.Quantity,
                UnitPrice = purchaseOrderService.FindUnitPriceByPurchaseOrderDetail(pod),
                Amount = purchaseOrderService.FindTotalAmountByPurchaseOrderDetail(pod),
                ReceivedQuantity = purchaseOrderService.FindReceivedQuantityByPurchaseOrderDetail(pod),
                RemainingQuantity = purchaseOrderService.FindRemainingQuantity(pod)
                
            }).ToList();

        }




    }
}
