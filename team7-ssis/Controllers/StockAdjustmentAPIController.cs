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
        public ApplicationDbContext context { get; set; }
        StockAdjustmentService stockAdjustmentService;
        ItemService itemService;
        ItemPriceService itemPriceService;
        UserService userService;
        NotificationService notificationService;
        public string CurrentUserName { get; set; }

      
        public StockAdjustmentAPIController()
        {
            context=new ApplicationDbContext();

            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }

        }


        // Get All stockadjustment in clerk page
        [Route("api/stockadjustment/all")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> stockadjustments()
        {
            stockAdjustmentService = new StockAdjustmentService(context);

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
            stockAdjustmentService = new StockAdjustmentService(context);

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

        //saving stock adjustment requests from mobile
        [Route("api/stockadjustment/mobile/save")]
        [HttpPost]
        public IHttpActionResult Save(List<MobileSADViewModel> models)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);
            itemService = new ItemService(context);
            StockAdjustment SA;
            try
            {
                //create new StockAdjustment object
                SA = new StockAdjustment()
                {
                    StockAdjustmentId = IdService.GetNewStockAdjustmentId(context),
                    StockAdjustmentDetails = new List<StockAdjustmentDetail>(),
                    CreatedDateTime = DateTime.Now,   
                    CreatedBy = userService.FindUserByEmail(models.First().UserName)          
                    
                };

                //convert viewmodels to StockAdmustmentDetails list and link to stockadjustment object
                foreach (MobileSADViewModel m in models)
                { Item item = itemService.FindItemByItemCode(m.ItemCode);
                    SA.StockAdjustmentDetails.Add(new StockAdjustmentDetail()
                    {
                        StockAdjustment = SA,
                        ItemCode = m.ItemCode,
                        Item = item,
                        OriginalQuantity = item.Inventory.Quantity,
                        AfterQuantity = item.Inventory.Quantity + m.QuantityAdjusted,
                        Reason =m.Reason

                    });
                }
                //save SA object into database 
                stockAdjustmentService.updateToPendingStockAdjustment(SA);
            }
            catch (ArgumentException)
            {
                return BadRequest("Unable to save Stock Adjustments!");
            }

            return Ok(SA.StockAdjustmentId);

        }

        [Route("api/stockadjustment/save")]
        [HttpPost]
        public void SaveStockAdjustmentAsDraft(List<ViewModelFromNew> list)
        {
            userService = new UserService(context);
            stockAdjustmentService = new StockAdjustmentService(context);
            itemService = new ItemService(context);
           
            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.CreatedDateTime = DateTime.Now;
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);
            stockAdjustmentService.updateStockAdjustment(s);
            s.CreatedBy = userService.FindUserByEmail(CurrentUserName);
         
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
            userService = new UserService(context);
            stockAdjustmentService = new StockAdjustmentService(context);
            itemService = new ItemService(context);
            notificationService = new NotificationService(context);

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(context);   
            s.CreatedBy = userService.FindUserByEmail(CurrentUserName);
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

            int flag = 0;
            foreach (ViewModelFromNew v in list)
            {
               
                if (v.Unitprice.CompareTo("250") == 1)
                {
                    flag = 1;break;
                }
                                   
            }
            string supervisor = list.First().Supervisor;
            string manager = list.First().Manager;
            if(flag==1)
            {
                notificationService.CreateNotification(s, userService.FindUserByEmail(manager));
            }
            if(flag==0)
            {
                notificationService.CreateNotification(s, userService.FindUserByEmail(supervisor));
            }

        }


        
        [Route("api/stockadjustment/delete")]
        [HttpGet]
        public void DeleteDraftStockAdjustment(string id)
        {
            userService = new UserService(context);
            stockAdjustmentService = new StockAdjustmentService(context);

            stockAdjustmentService.CancelDraftOrPendingStockAdjustment(id);
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);

            
            ApplicationUser currentUser = userService.FindUserByEmail(CurrentUserName);
            sd.UpdatedBy = currentUser;
            sd.UpdatedDateTime = DateTime.Now;

            stockAdjustmentService.updateStockAdjustment(sd);

        }


        [Route("api/stockadjustment/reject")]
        [HttpGet]
        public void RejectStockAdjustment(string id)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            stockAdjustmentService.RejectStockAdjustment(id);
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);

        }

        [Route("api/stockadjustment/approve")]
        [HttpGet]
        public void ApproveStockAdjustment(string id)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);

            stockAdjustmentService.ApproveStockAdjustment(id);
        
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(id);
            ApplicationUser currentUser= userService.FindUserByEmail(CurrentUserName);

            if (currentUser.Roles.Where(role => role.RoleId == "5").Count() > 0) // if Manager
            {
                sd.ApprovedByManager = userService.FindUserByEmail(CurrentUserName);
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
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);

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
           
            ApplicationUser currentUser = userService.FindUserByEmail(CurrentUserName);
            sa.UpdatedBy = currentUser;
            sa.UpdatedDateTime = DateTime.Now;
            stockAdjustmentService.updateToDraftStockAdjustment(sa);
        }

        [Route("api/stockadjustment/update_to_pending")]
        [HttpPost]
        public void UpdateStockAdjustmentAsPending(List<ViewModelFromEditDetail> list)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            userService = new UserService(context);

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
 
            ApplicationUser currentUser = userService.FindUserByEmail(CurrentUserName);
            sa.UpdatedBy = currentUser;
            sa.UpdatedDateTime = DateTime.Now;
            stockAdjustmentService.updateToPendingStockAdjustment(sa);

            // decide who to sent notification
            int flag = 0;
            foreach (ViewModelFromEditDetail v in list)
            {

                if (v.Unitprice.CompareTo("250") == 1)
                {
                    flag = 1; break;
                }

            }
            string supervisor = list.First().Supervisor;
            string manager = list.First().Manager;
            if (flag == 1)
            {
                notificationService.CreateNotification(sa, userService.FindUserByEmail(manager));
            }
            if (flag == 0)
            {
                notificationService.CreateNotification(sa, userService.FindUserByEmail(supervisor));
            }
        }


        [Route("api/stockadjustment/detail/{StockAdjustmentId}")]
        [HttpGet]
        public IEnumerable<StockAdjustmentDetailViewModel>  GetStockAdjustmentDetail(string StockAdjustmentId)
        {
            stockAdjustmentService = new StockAdjustmentService(context);
            itemPriceService = new ItemPriceService(context);
            itemService = new ItemService(context);

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

