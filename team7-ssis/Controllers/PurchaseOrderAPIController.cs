using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using team7_ssis.Services;
using System.Web.Mvc;

namespace team7_ssis.Controllers
{
    public class PurchaseOrderApiController : ApiController
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        PurchaseOrderService purchaseOrderService = new PurchaseOrderService(context);

        public IEnumerable<PurchaseOrderViewModel> LoadData()
        {
           context.Configuration.LazyLoadingEnabled = false; // if your table is relational, contain foreign key
            List<PurchaseOrder> poList = purchaseOrderService.FindAllPurchaseOrders();

            
            

        }
       

    }
}
