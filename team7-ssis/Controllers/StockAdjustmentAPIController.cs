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

namespace team7_ssis.Controllers
{
    
    public class StockAdjustmentAPIController : ApiController
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        StockAdjustmentService stockAdjustmentService = new StockAdjustmentService(context);
        StockAdjustmentRepository stockAdjustmentRepository = new StockAdjustmentRepository(context); 



    
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
