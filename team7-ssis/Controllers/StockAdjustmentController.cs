﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Tests.Services;

namespace team7_ssis.Controllers
{
    public class StockAdjustmentController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        StockAdjustmentService stockAdjustmentService = new StockAdjustmentService(context);


        // GET: StockAdjustment
        public ActionResult Index()
         {
            return View();
          }



        public ActionResult Add()
        {
            return View();
        }


      
           

    }
}