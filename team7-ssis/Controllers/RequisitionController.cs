using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    public class RequisitionController : Controller
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        RequisitionService requisitionService = new RequisitionService(context); 
        
        // GET: /Requisition
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Requisiton/RequisitionDetails
        public ActionResult RequisitionDetails()
        {
            return View();
        }
        // GET: /Requisiton/StationeryRetrieval
        public ActionResult StationeryRetrieval()
        {
            return View();
        }
        // GET: /Requisiton/StationeryDisbursement
        public ActionResult StationeryDisbursement()
        {
            return View();
        }
    }
}