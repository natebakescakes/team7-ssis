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
            //context.Configuration.LazyLoadingEnabled.Equals("false");
            return purchaseOrderService.FindAllPurchaseOrders().Select(po => new PurchaseOrderViewModel()
            {
                PNo = po.PurchaseOrderNo,
                SupplierName=po.Supplier.Name,
                CreatedDate=po.CreatedDateTime.ToShortDateString(),
                Status=po.Status.Name
            }).ToList();
        }
       

    }
}
