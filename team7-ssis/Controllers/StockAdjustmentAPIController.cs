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
        StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        ItemService itemService;
        ItemPriceService itemPriceService;
        UserService userService;
        public StockAdjustmentAPIController()
        {
            context=new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
             stockAdjustmentRepository = new StockAdjustmentRepository(context);
             itemService = new ItemService(context);
             itemPriceService = new ItemPriceService(context);
            userService = new UserService(context);
            stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
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
        [Route("api/stockadjustment/save")]
        [HttpPost]
        public void SaveStockAdjustmentAsDraft(List<ViewModelOfDetail> list)
        {
            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            s.CreatedBy = userService.FindUserByEmail(UserName);
            s.CreatedDateTime = DateTime.Now;

            foreach (ViewModelOfDetail v in list )
            {
                StockAdjustmentDetail sd = new StockAdjustmentDetail();
                string itemcode = v.Itemcode;
                Item item = itemService.FindItemByItemCode(itemcode);
                sd.ItemCode = itemcode;
                sd.Reason = (v.Reason==null)?"":v.Reason;
                sd.StockAdjustmentId = s.StockAdjustmentId;
                sd.OriginalQuantity =item.Inventory.Quantity;
                sd.AfterQuantity = v.Adjustment + sd.OriginalQuantity;
                detaillist.Add(sd);
             //  stockAdjustmentDetailRepository.Save(sd);
            }
            s.StockAdjustmentDetails = detaillist;           
            stockAdjustmentService.CreateDraftStockAdjustment(s);
            }



        [Route("api/stockadjustment/confirm")]
        [HttpPost]
        public void CreatePendingStockAdjustmentPending(List<ViewModelOfDetail> list)
        {
            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            s.CreatedBy = userService.FindUserByEmail(UserName);
            s.CreatedDateTime = DateTime.Now;

            foreach (ViewModelOfDetail v in list)
            {
                StockAdjustmentDetail sd = new StockAdjustmentDetail();
                string itemcode = v.Itemcode;
                Item item = itemService.FindItemByItemCode(itemcode);
                sd.ItemCode = itemcode;
                sd.Reason = (v.Reason == null) ? "" : v.Reason;
                sd.StockAdjustmentId = s.StockAdjustmentId;
                sd.OriginalQuantity = item.Inventory.Quantity;
                sd.AfterQuantity = v.Adjustment + sd.OriginalQuantity;
                detaillist.Add(sd);
                //  stockAdjustmentDetailRepository.Save(sd);
            }
            s.StockAdjustmentDetails = detaillist;
            stockAdjustmentService.CreatePendingStockAdjustment(s);
        }

        [Route("api/stockadjustment/delete")]
        [HttpPost]
        public void DeleteDraftStockAdjustment(string id)
        {
            stockAdjustmentService.CancelDraftOrPendingStockAdjustment(id);

        }





    }
}
