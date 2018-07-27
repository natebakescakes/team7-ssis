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
      ItemService itemService;
        ItemPriceService itemPriceService;
        UserService userService;
        public StockAdjustmentAPIController()
        {
            context=new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
             itemService = new ItemService(context);
             itemPriceService = new ItemPriceService(context);
            userService = new UserService(context);
          
        }


        // Get All stockadjustment in clerk page
        [Route("api/stockadjustment/all")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> stockadjustments()
        {
            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustment();
            List<StockAdjustmentViewModel > sadj= new List<StockAdjustmentViewModel>();


            foreach (StockAdjustment s in list)
            {
                StockAdjustmentViewModel savm = new StockAdjustmentViewModel();

                savm.StockAdjustmentId = s.StockAdjustmentId;
                savm.CreatedBy = s.CreatedBy.FirstName + " " + s.CreatedBy.LastName;
                savm.ApprovedBySupervisor = s.ApprovedBySupervisor == null ? "" : s.ApprovedBySupervisor.FirstName + " " + s.ApprovedBySupervisor.LastName;
                savm.CreatedDateTime = s.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
                savm.StatusName = s.Status.Name;
                sadj.Add(savm);
                }
            return sadj;
        }

        // Get All stockadjustment in manager/superviosr page
        [Route("api/stockadjustment/allExceptDraftAndCancelled")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> stockadjustmentsExceptDraft()
        {
            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustmentExceptDraft();
            List<StockAdjustmentViewModel> sadj = new List<StockAdjustmentViewModel>();


            foreach (StockAdjustment s in list)
            {
                StockAdjustmentViewModel savm = new StockAdjustmentViewModel();

                savm.StockAdjustmentId = s.StockAdjustmentId;                
                savm.CreatedBy = s.CreatedBy.FirstName + " " + s.CreatedBy.LastName;
                savm.ApprovedBySupervisor = s.ApprovedBySupervisor == null?"": s.ApprovedBySupervisor.FirstName + " " + s.ApprovedBySupervisor.LastName;
                savm.CreatedDateTime = s.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
                savm.StatusName = s.Status.Name;
                sadj.Add(savm);
            }

        

            return sadj;
        }


        [Route("api/stockadjustment/save")]
        [HttpPost]
        public void SaveStockAdjustmentAsDraft(List<ViewModelFromNew> list)
        {
           
            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.CreatedDateTime = DateTime.Now;
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            stockAdjustmentService.updateStockAdjustment(s);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            s.CreatedBy = userService.FindUserByEmail(UserName);
         
            foreach (ViewModelFromNew v in list )
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
        public void CreatePendingStockAdjustment(List<ViewModelFromNew> list)
        {
            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            s.CreatedBy = userService.FindUserByEmail(UserName);
            s.CreatedDateTime = DateTime.Now;
        
            foreach (ViewModelFromNew v in list)
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
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);
           

            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            ApplicationUser currentUser = userService.FindUserByEmail(UserName);
            sd.UpdatedBy = currentUser;
            sd.UpdatedDateTime = DateTime.Now;

            stockAdjustmentService.updateStockAdjustment(sd);

        }


        [Route("api/stockadjustment/reject")]
        [HttpGet]
        public void RejectStockAdjustment(string id)
        {
            stockAdjustmentService.RejectStockAdjustment(id);
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);
           


        }

        [Route("api/stockadjustment/approve")]
        [HttpGet]
        public void ApproveStockAdjustment(string id)
        {
            stockAdjustmentService.ApproveStockAdjustment(id);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);
            ApplicationUser currentUser= userService.FindUserByEmail(UserName);
            if (currentUser.Roles.Where(role => role.RoleId == "5").Count() > 0) // if Manager
            {
                sd.ApprovedByManager = userService.FindUserByEmail(UserName);
                sd.ApprovedManagerDateTime = DateTime.Now;
            } else if (currentUser.Roles.Where(role => role.RoleId == "4").Count() > 0)
            {
                sd.ApprovedBySupervisor = currentUser;
                sd.ApprovedSupervisorDateTime = DateTime.Now;
            }
          
            stockAdjustmentService.updateStockAdjustment(sd);

        }

        [Route("api/stockadjustment/update_to_draft")]
        [HttpPost]
        public void UpdateStockAdjustmentAsDraft(List<ViewModelFromEditDetail> list)
        {
        
            foreach (ViewModelFromEditDetail v in list)
            {
                StockAdjustmentDetail sd = stockAdjustmentService.findStockAdjustmentDetailById(v.StockAdjustmentID, v.Itemcode);
                int d = v.Adjustment;
                sd.AfterQuantity = sd.OriginalQuantity + v.Adjustment;
                sd.Reason = v.Reason;             
                stockAdjustmentService.updateStockAdjustmentDetail(sd);
            }
            string stockadjustmentid = list.First().StockAdjustmentID;
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(stockadjustmentid);
            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();
            ApplicationUser currentUser = userService.FindUserByEmail(UserName);
            sa.UpdatedBy = currentUser;
            sa.UpdatedDateTime = DateTime.Now;
            stockAdjustmentService.updateToDraftStockAdjustment(sa);
        }

        [Route("api/stockadjustment/update_to_pending")]
        [HttpPost]
        public void UpdateStockAdjustmentAsPending(List<ViewModelFromEditDetail> list)
        {


           
            foreach (ViewModelFromEditDetail v in list)
            {
                StockAdjustmentDetail sd = stockAdjustmentService.findStockAdjustmentDetailById(v.StockAdjustmentID, v.Itemcode);
                int d = v.Adjustment;
                sd.AfterQuantity = sd.OriginalQuantity + v.Adjustment;
                sd.Reason = v.Reason;               
                stockAdjustmentService.updateStockAdjustmentDetail(sd);
            }
            string stockadjustmentid = list.First().StockAdjustmentID;
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(stockadjustmentid);

            string UserName = System.Web.HttpContext.Current.User.Identity.GetUserName();      
            ApplicationUser currentUser = userService.FindUserByEmail(UserName);
            sa.UpdatedBy = currentUser;
            sa.UpdatedDateTime = DateTime.Now;
            stockAdjustmentService.updateToPendingStockAdjustment(sa);
        }


        [Route("api/stockadjustment/detail/{StockAdjustmentId}")]
        [HttpGet]
        public IEnumerable<StockAdjustmentDetailViewModel>  GetStockAdjustmentDetail(string StockAdjustmentId)
        {
   

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

