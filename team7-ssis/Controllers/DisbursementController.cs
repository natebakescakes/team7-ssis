using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;
using Rotativa;

namespace team7_ssis.Controllers
{
    public class DisbursementController : Controller
    {
        ApplicationDbContext context;
        DisbursementService disbursementService;
        DisbursementRepository disbursementRepository;
        ItemService itemService;

        public DisbursementController()
        {
            context = new ApplicationDbContext();
            disbursementService = new DisbursementService(context);
            disbursementRepository = new DisbursementRepository(context);
            itemService = new ItemService(context);
        }

        // GET: Disbursement/Manage
        public ActionResult Manage()
        {
            return View();
        }

        // GET: Disbursement/DisbursementDetails
        public ActionResult DisbursementDetails(string did)
        {
            DisbursementFormViewModel viewModel = new DisbursementFormViewModel();
            try
            {
                if (TempData["error"] != null)
                {
                    ViewBag.Error = (bool)TempData["error"];
                }

                Disbursement d = disbursementService.FindDisbursementById(did);
                viewModel.DisbursementId = d.DisbursementId;
                viewModel.Representative = d.CollectedBy == null ? "" : String.Format("{0} {1}", d.CollectedBy.FirstName, d.CollectedBy.LastName);
                viewModel.Department = d.Department.Name;
                viewModel.OrderTime = String.Format("{0} {1}", d.CreatedDateTime.ToShortDateString(), d.CreatedDateTime.ToShortTimeString());
                viewModel.CollectionPoint = d.Department.CollectionPoint.Name;
                viewModel.Remarks = d.Remarks;
                viewModel.Status = d.Status.StatusId;
            } catch
            {
                return new HttpStatusCodeResult(400);
            }
            return View(viewModel);
        }

        // POST: Disbursement/Collect
        [HttpPost]
        public ActionResult Collect(string did)
        {
            Disbursement d;
            try
            {
                disbursementService.ConfirmCollection(did);
                d = disbursementRepository.FindById(did);
                
            } catch {
                TempData["error"] = true;
                return RedirectToAction("DisbursementDetails", "Disbursement", new { did } );
            }
            TempData["did"] = did;
            return RedirectToAction("StationeryDisbursement", "Requisition", new { rid = d.Retrieval.RetrievalId });

        }

        [HttpPost]
        public ActionResult PrintDisbursementPDF(string dbmNumber)
        {
            Disbursement d = disbursementService.FindDisbursementById(dbmNumber);
            List<DisbursementFormTableViewModel> viewModel = new List<DisbursementFormTableViewModel>();
            viewModel = d.DisbursementDetails.Select(x => new DisbursementFormTableViewModel()
            {
                ItemCode = x.ItemCode,
                Qty = x.ActualQuantity,
                Description = itemService.FindItemByItemCode(x.ItemCode).Description
            }).ToList();


            ViewBag.Disbursement = d;
            var a = new ViewAsPdf("DisbursmentPrintView", viewModel) { FileName = dbmNumber + ".pdf" };
            a.Cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);
            a.FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            a.CustomSwitches = "--load-error-handling ignore";
            return a;
        }

        //public ActionResult DisbursmentPrint(string dbm)
        //{
           
        //    return View("DisbursmentPrintView", viewModel);

        //}
    }
}