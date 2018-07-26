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
        //StockAdjustmentRepository stockAdjustmentRepository;
        //StockAdjustmentDetailRepository stockAdjustmentDetailRepository;
        ItemService itemService;
        ItemPriceService itemPriceService;
        UserService userService;
        public StockAdjustmentAPIController()
        {
            context=new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
            // stockAdjustmentRepository = new StockAdjustmentRepository(context);
             itemService = new ItemService(context);
             itemPriceService = new ItemPriceService(context);
            userService = new UserService(context);
           // stockAdjustmentDetailRepository = new StockAdjustmentDetailRepository(context);
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
        [HttpGet]
        public void DeleteDraftStockAdjustment(string id)
        {
            stockAdjustmentService.CancelDraftOrPendingStockAdjustment(id);


        }


        [Route("api/stockadjustment/detail/{StockAdjustmentId}")]
        [HttpGet]
        public IEnumerable<StockAdjustmentDetailViewModel>  GetStockAdjustmentDetail(string StockAdjustmentId)
        {
            // StockAdjustmentDetailListViewModel viewModel = new StockAdjustmentDetailListViewModel();

            // StockAdjustmentViewModel stockAdjustmentViewModel = new StockAdjustmentViewModel();

            //stockAdjustmentViewModel.StockAdjustmentId = sd.StockAdjustmentId;
            //stockAdjustmentViewModel.CreatedBy = sd.CreatedBy == null ? "" : sd.CreatedBy.FirstName + " " + sd.CreatedBy.LastName;
            //stockAdjustmentViewModel.CreatedDateTime = sd.CreatedDateTime;
            //stockAdjustmentViewModel.ApprovedBySupervisor = sd.ApprovedBySupervisor == null ? "" : sd.ApprovedBySupervisor.FirstName + " " + sd.ApprovedBySupervisor.LastName;
            //stockAdjustmentViewModel.UpdateDateTime = sd.UpdatedDateTime == null ? DateTime.Now :(DateTime)sd.UpdatedDateTime;

        

            List<StockAdjustmentDetailViewModel> detailListViewModel = new List<StockAdjustmentDetailViewModel>();
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(StockAdjustmentId);
            List<StockAdjustmentDetail> detaillist = sd.StockAdjustmentDetails;

            foreach (StockAdjustmentDetail sad in detaillist)
            {
                StockAdjustmentDetailViewModel sadv = new StockAdjustmentDetailViewModel();
                string itemcode = sad.ItemCode;
                sadv.ItemCode = itemcode;
                Item item = itemService.FindItemByItemCode(itemcode);
                sadv.Description = item.Description == null ? "" : item.Description;
                sadv.Reason = sad.Reason == null ? "" : sad.Reason;

                sadv.UnitPrice = itemPriceService.GetDefaultPrice(item, 1);
                if (sadv.UnitPrice.CompareTo("250") == -1)
                    sadv.PriceColor = "-1";
                else
                {
                    sadv.PriceColor = "1";
                        }
                
                sadv.Adjustment =sad.AfterQuantity-sad.OriginalQuantity;
                detailListViewModel.Add(sadv);
                 };

          //  viewModel.StockAdjustmentModel = stockAdjustmentViewModel;
           // viewModel.StockAdjustmentDetailsModel = detailListViewModel;

            return detailListViewModel;
            }


        }
    }

