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
using System.Text.RegularExpressions;

namespace team7_ssis.Controllers
{

    public class StockAdjustmentAPIController : ApiController
    {
        public ApplicationDbContext Context { get; set; }
        StockAdjustmentService stockAdjustmentService;
        ItemService itemService;
        ItemPriceService itemPriceService;
        UserService userService;
        NotificationService notificationService;
        public string CurrentUserName { get; set; }


        public StockAdjustmentAPIController()
        {
            Context = new ApplicationDbContext();

            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }

        }


        // Get All stockadjustment in clerk page
        [Route("api/stockadjustment/all")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> GetAllStockAdjustments()
        {
            stockAdjustmentService = new StockAdjustmentService(Context);

            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustment();
            List<StockAdjustmentViewModel> sadj = new List<StockAdjustmentViewModel>();


            foreach (StockAdjustment s in list)
            {
                StockAdjustmentViewModel savm = new StockAdjustmentViewModel();

                savm.StockAdjustmentId = s.StockAdjustmentId;
                savm.CreatedBy =s.CreatedBy==null?"":s.CreatedBy.FirstName + " " + s.CreatedBy.LastName;
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
        public IEnumerable<StockAdjustmentViewModel> GetAllStockAdjustmentsExceptDraft()
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            notificationService = new NotificationService(Context);
            userService = new UserService(Context);

            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustmentExceptDraft();
            List<StockAdjustmentViewModel> sadj = new List<StockAdjustmentViewModel>();
            //supervisor and manager process the stock adjustments which are sent to them
            List<Notification> notifications = new List<Notification>();
            List<string> Ids = new List<string>();

            if (notificationService.FindNotificationsByUser(userService.FindUserByEmail(CurrentUserName)).Count == 0)
            {

            }
            else
            {

                notifications = notificationService.FindNotificationsByUser(userService.FindUserByEmail(CurrentUserName));
                foreach (Notification n in notifications)
                {
                    var stockAdjustmentId = Regex.Match(n.Contents, @"ADJ-\d{6}-\d{3}");
                    Ids.Add(Convert.ToString(stockAdjustmentId));

                }
            }

            foreach (StockAdjustment s in list)
            {
                StockAdjustmentViewModel savm = new StockAdjustmentViewModel();

                savm.StockAdjustmentId = s.StockAdjustmentId;
                savm.CreatedBy = s.CreatedBy == null ? "" : s.CreatedBy.FirstName + " " + s.CreatedBy.LastName;
                savm.ApprovedBySupervisor = s.ApprovedBySupervisor == null ? "" : s.ApprovedBySupervisor.FirstName + " " + s.ApprovedBySupervisor.LastName;
                savm.CreatedDateTime = s.CreatedDateTime.ToString("yyyy-MM-dd HH: mm:ss");
                savm.StatusName = s.Status.Name;

                if (notificationService.GetCreatedFor(savm.StockAdjustmentId) != null)

                {
                    savm.ProcessBy = notificationService.GetCreatedFor(savm.StockAdjustmentId).FirstName + " " + notificationService.GetCreatedFor(savm.StockAdjustmentId).LastName;
                }
                else
                {
                    savm.ProcessBy = "";
                }


                if (Ids.Contains(savm.StockAdjustmentId))
                {
                    savm.IsSentFor = 1;
                }
                else
                {
                    savm.IsSentFor = 0;
                }



                sadj.Add(savm);
            }

            return sadj;
        }

        //saving stock adjustment requests from mobile
        [Route("api/stockadjustment/mobile/save")]
        [HttpPost]
        public IHttpActionResult Save(List<MobileSADViewModel> models)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            userService = new UserService(Context);
            itemService = new ItemService(Context);
            notificationService = new NotificationService(Context);

            StockAdjustment SA;
            try
            {
                //create new StockAdjustment object
                SA = new StockAdjustment()
                {
                    StockAdjustmentId = IdService.GetNewStockAdjustmentId(Context),
                    StockAdjustmentDetails = new List<StockAdjustmentDetail>(),
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = userService.FindUserByEmail(models.First().UserName)

                };

                //convert viewmodels to StockAdmustmentDetails list and link to stockadjustment object
                foreach (MobileSADViewModel m in models)
                {
                    Item item = itemService.FindItemByItemCode(m.ItemCode);
                    SA.StockAdjustmentDetails.Add(new StockAdjustmentDetail()
                    {
                        StockAdjustment = SA,
                        ItemCode = m.ItemCode,
                        Item = item,
                        OriginalQuantity = item.Inventory.Quantity,
                        AfterQuantity = item.Inventory.Quantity + m.QuantityAdjusted,
                        Reason = m.Reason

                    });
                }

                bool flag = false;
                foreach (StockAdjustmentDetail detail in SA.StockAdjustmentDetails)
                {
                    foreach (ItemPrice p in detail.Item.ItemPrices)
                    {
                        if (p.Price >= 250)
                        {
                            flag = true; break;
                        }
                    }
                }
   
                ApplicationUser supervisor = userService.FindUserByEmail(models.First().UserName).Supervisor;
                ApplicationUser manager = supervisor.Supervisor;
                if (flag == true)
                {
                    notificationService.CreateNotification(SA, manager);
                    
                }
                if (flag == false)
                {
                    notificationService.CreateNotification(SA, supervisor);
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

        //save as draft
        [Route("api/stockadjustment/save")]
        [HttpPost]
        public void SaveStockAdjustmentAsDraft(List<ViewModelFromNew> list)
        {
            userService = new UserService(Context);
            stockAdjustmentService = new StockAdjustmentService(Context);
            itemService = new ItemService(Context);

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.CreatedDateTime = DateTime.Now;
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(Context);
            stockAdjustmentService.updateStockAdjustment(s);
            s.CreatedBy = userService.FindUserByEmail(CurrentUserName);

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
            stockAdjustmentService.CreateDraftStockAdjustment(s);

        }


        //create pending 
        [Route("api/stockadjustment/confirm")]
        [HttpPost]
        public string CreatePendingStockAdjustment(List<ViewModelFromNew> list)
        {
            userService = new UserService(Context);
            stockAdjustmentService = new StockAdjustmentService(Context);
            itemService = new ItemService(Context);
            notificationService = new NotificationService(Context);

            List<StockAdjustmentDetail> detaillist = new List<StockAdjustmentDetail>();
            StockAdjustment s = new StockAdjustment();
            s.StockAdjustmentId = IdService.GetNewStockAdjustmentId(Context);
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
                    flag = 1; break;
                }

            }
            string supervisor = list.First().Supervisor;
            string manager = list.First().Manager;
            if (flag == 1)
            {
                notificationService.CreateNotification(s, userService.FindUserByEmail(manager));
            }
            if (flag == 0)
            {
                notificationService.CreateNotification(s, userService.FindUserByEmail(supervisor));
            }

            return s.StockAdjustmentId;
        }


        [Route("api/stockadjustment/cancel/{id}")]
        [HttpGet]
        public void CancelPendingStockAdjustment(string id)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);

            stockAdjustmentService.CancelDraftOrPendingStockAdjustment(id);
        }



        // reject with reason
        [Route("api/stockadjustment/reject")]
        [HttpPost]
        public void RejectStockAdjustment(List<ViewModelFromEditDetail> list)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);

            string stockadjustment_id = list.First().StockAdjustmentID;
            foreach (ViewModelFromEditDetail v in list)
            {
                StockAdjustmentDetail sad = stockAdjustmentService.findStockAdjustmentDetailById(v.StockAdjustmentID, v.Itemcode);               
               // sad.AfterQuantity = sad.OriginalQuantity + v.Adjustment;
                sad.Reason = v.Reason;
                stockAdjustmentService.updateStockAdjustmentDetail(sad);
            }

            stockAdjustmentService.RejectStockAdjustment(stockadjustment_id);
            StockAdjustment sa = stockAdjustmentService.FindStockAdjustmentById(stockadjustment_id);
            
        }

        //approve with reason
        [Route("api/stockadjustment/approve")]
        [HttpPost]
        public void ApproveStockAdjustment(List<ViewModelFromEditDetail> list)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            userService = new UserService(Context);
            string stockadjustment_id = list.First().StockAdjustmentID;
           
        
            StockAdjustment sd = stockAdjustmentService.FindStockAdjustmentById(stockadjustment_id);
            ApplicationUser currentUser= userService.FindUserByEmail(CurrentUserName);

            foreach (ViewModelFromEditDetail v in list)
            {
                StockAdjustmentDetail sad = stockAdjustmentService.findStockAdjustmentDetailById(v.StockAdjustmentID, v.Itemcode);
                sad.Reason = v.Reason;
                stockAdjustmentService.updateStockAdjustmentDetail(sad);
            }

            if (currentUser.Roles.Where(role => role.RoleId == "5").Count() > 0) // if Manager
            {
                sd.ApprovedByManager = userService.FindUserByEmail(CurrentUserName);
                sd.ApprovedManagerDateTime = DateTime.Now;
            }
            else if (currentUser.Roles.Where(role => role.RoleId == "4").Count() > 0)
            {
                sd.ApprovedBySupervisor = currentUser;
                sd.ApprovedSupervisorDateTime = DateTime.Now;
            }

            stockAdjustmentService.updateStockAdjustment(sd);
            stockAdjustmentService.ApproveStockAdjustment(stockadjustment_id);

        }

        //update draft
        [Route("api/stockadjustment/update_to_draft")]
        [HttpPost]
        public void UpdateStockAdjustmentAsDraft(List<ViewModelFromEditDetail> list)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            userService = new UserService(Context);

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

        //update darft to pending 
        [Route("api/stockadjustment/update_to_pending")]
        [HttpPost]
        public string UpdateStockAdjustmentAsPending(List<ViewModelFromEditDetail> list)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            userService = new UserService(Context);
            notificationService = new NotificationService(Context);

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
            return stockadjustmentid;
        }

        //show detail
        [Route("api/stockadjustment/detail/{StockAdjustmentId}")]
        [HttpGet]
        public IEnumerable<StockAdjustmentDetailViewModel> GetStockAdjustmentDetail(string StockAdjustmentId)
        {
            stockAdjustmentService = new StockAdjustmentService(Context);
            itemPriceService = new ItemPriceService(Context);
            itemService = new ItemService(Context);

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

                sadv.Adjustment = sad.AfterQuantity - sad.OriginalQuantity;
                detailListViewModel.Add(sadv);
            };


            return detailListViewModel;
        }

        /// <summary>
        /// Supervisor/Manager to view submitted stock adjustments
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/stockadjustment/supervisor")]
        public IHttpActionResult GetStockAdjustmentsForSupervisor([FromBody] EmailViewModel model)
        {
            // Does not discern between supervisor and manager at the moment
            var stockAdjustments = new StockAdjustmentService(Context).FindAllStockAdjustmentExceptDraft();

            if (stockAdjustments.Count == 0) return NotFound();

            return Ok(stockAdjustments.Select(stockAdjustment => new StockAdjustmentRequestViewModel()
            {
                StockAdjustmentId = stockAdjustment.StockAdjustmentId,
                RequestorName = stockAdjustment.CreatedBy != null ? $"{stockAdjustment.CreatedBy.FirstName} {stockAdjustment.CreatedBy.LastName}" : "",
                RequestedDate = stockAdjustment.CreatedDateTime.ToShortDateString(),
                Remarks = stockAdjustment.Remarks,
                Status = stockAdjustment.Status.Name,
                StockAdjustmentRequestDetails = stockAdjustment.StockAdjustmentDetails.Select(detail => new StockAdjustmentRequestDetailViewModel()
                {
                    ItemCode = detail.ItemCode,
                    ItemName = detail.Item.Description,
                    OriginalQuantity = detail.OriginalQuantity.ToString(),
                    AfterQuantity = detail.AfterQuantity.ToString(),
                    Reason = detail.Reason,
                }).ToList(),
            }));
        }

        [Route("api/stockadjustment/supervisor/approve")]
        public IHttpActionResult ApproveStockAdjustment([FromBody] StockAdjustmentIdViewModel model)
        {
            try
            {
                new StockAdjustmentService(Context).ApproveStockAdjustment(model.StockAdjustmentId, model.Email);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully approved"
            });
        }

        [Route("api/stockadjustment/supervisor/reject")]
        public IHttpActionResult RejectStockAdjustment([FromBody] StockAdjustmentIdViewModel model)
        {
            try
            {
                new StockAdjustmentService(Context).RejectStockAdjustment(model.StockAdjustmentId, model.Email);
            }
            catch (ArgumentException e) 
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully rejected"
            });
        }
    }
}

