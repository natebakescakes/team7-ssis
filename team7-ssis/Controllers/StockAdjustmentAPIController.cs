using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    [Route("api/stockadjustment")]
    public class StockAdjustmentAPIController : ApiController
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        StockAdjustmentService stockAdjustmentService = new StockAdjustmentService(context);

        // GET: api/stockadjustment
        [HttpGet]
        public IQueryable Get()
        {

            return stockAdjustmentService.FindAllStockAdjustment().AsQueryable();

        }

    }
}
