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
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    
    public class StockAdjustmentAPIController : ApiController
    {
        private ApplicationDbContext context;
        private StockAdjustmentService stockAdjustmentService;
        private StockAdjustmentRepository stockAdjustmentRepository;
        private UserService userService;
        private StatusService statusService;
        private ItemService itemService;

        public StockAdjustmentAPIController()
        {
            context = new ApplicationDbContext();
            stockAdjustmentService = new StockAdjustmentService(context);
            stockAdjustmentRepository = new StockAdjustmentRepository(context);
            userService = new UserService(context);
            statusService = new StatusService(context);
            itemService = new ItemService(context);
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
        public IHttpActionResult Save(List<MobileSADViewModel> models)
        {
            try
            {
                //create new StockAdjustment object
                StockAdjustment SA = new StockAdjustment()
                {
                    StockAdjustmentId = IdService.GetNewStockAdjustmentId(context),
                    CreatedDateTime = DateTime.Now,
                    Status = statusService.FindStatusByStatusId(4),
                    CreatedBy = userService.FindUserByEmail(models.First().UserName),          
                    
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
               
            }
            catch (ArgumentException)
            {
                return BadRequest("Unable to save Stock Adjustments!");
            }

            return Ok();

        }


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
