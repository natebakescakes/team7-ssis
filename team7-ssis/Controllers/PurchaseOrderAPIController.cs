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

        [Route("api/purchaseOrders/all")]
        [HttpGet]
        public IEnumerable<PurchaseOrderViewModel> PurchaseOrders()
        {
           //context.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            List<PurchaseOrder> poList = purchaseOrderService.FindAllPurchaseOrders();

            List<PurchaseOrderViewModel> data = new List<PurchaseOrderViewModel>();

            foreach(PurchaseOrder p in poList)
            {
                data.Add(new PurchaseOrderViewModel
                {
                    PNo = p.PurchaseOrderNo,
                    SupplierName = p.Supplier.Name,
                    CreatedDate = p.CreatedDateTime,
                    Status = p.Status.Name
                
                });
 
            }

            return data;
        }
       

    }
}
