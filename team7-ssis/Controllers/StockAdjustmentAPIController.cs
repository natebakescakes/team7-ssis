using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;
using team7_ssis.Repositories;
using Microsoft.AspNet.Identity;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    
    public class StockAdjustmentAPIController : ApiController
    {
        ApplicationDbContext context;
        StockAdjustmentService stockAdjustmentService;
        StockAdjustmentRepository stockAdjustmentRepository;
        ItemService itemService;
        ItemPriceService itemPriceService;
        public StockAdjustmentAPIController()
        {
            context=new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
             stockAdjustmentRepository = new StockAdjustmentRepository(context);
             itemService = new ItemService(context);
             itemPriceService = new ItemPriceService(context);

        }



        [Route("api/stockadjustment/all")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> stockadjustments()
        {
            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustment();
            List<StockAdjustmentViewModel > sadj= new List<StockAdjustmentViewModel>();


            foreach (StockAdjustment s in list)
            {
                sadj.Add(new StockAdjustmentViewModel
                {
                    StockAdjustmentId = s.StockAdjustmentId,
                    CreatedBy = (s.CreatedBy == null) ? "" : s.CreatedBy.FirstName,
                    ApprovedBySupervisor = (s.ApprovedBySupervisor == null) ? "" : s.ApprovedBySupervisor.FirstName,
                    CreatedDateTime = s.CreatedDateTime,
                    StatusName = (s.Status == null) ? "" : s.Status.Name,
                    //Link = "/StockAdjustment/"+s.StockAdjustmentId
                });

            }

            return sadj;
        }

        [Route("api/stockadjustment/items")]
        [HttpPost]
        public HttpResponseMessage SaveInSession(string[] itemCodes)
        {
            string user = System.Web.HttpContext.Current.User.Identity.GetUserId();
            List<string> itemcodes_list = (List<string>)System.Web.HttpContext.Current.Session[user + "stock"];
            foreach (string i in itemCodes)
            {
                itemcodes_list.Add(i);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        //[Route("api/stockadjustment/restoreitems")]
        //[HttpGet]
        //public IEnumerable<ItemViewModel> RestoreItemFromSession()
        //{
        //    string user = System.Web.HttpContext.Current.User.Identity.GetUserId();
        //    List<ItemViewModel> ViewModel_list = new List<ItemViewModel>();
        //    List<Item> Item_list = new List<Item>();

        //    List<string> itemcodes_list = (List<string>)System.Web.HttpContext.Current.Session[user + "stock"];
        //    foreach(string code in itemcodes_list)
        //    {
        //        Item i = itemService.FindItemByItemCode(code);
        //        ItemViewModel iv = new ItemViewModel();
        //        iv.ItemCode = i.ItemCode;
        //        iv.Description = i.Description;
        //        iv.UnitPrice = itemPriceService.GetDefaultPrice(i, 1);
        //        ViewModel_list.Add(iv);
        //    }
        //    return ViewModel_list;
        //}

        //[Route("api/supervisor/all")]
        //[HttpGet]

        //public IEnumerable<ApplicationUser>  AllSupervisors()
        //{
        //    return null;
        //}
        //[Route("api/manager/all")]
        //[HttpGet]
        //public IEnumerable<ApplicationUser> AllManagers()
        //{
        //    return null;
        //}

    }
}
