using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class DisbursementController : Controller
    {
        private ApplicationDbContext context;
        private DisbursementService disbursementService;

        public DisbursementController()
        {
            context = new ApplicationDbContext();
            disbursementService = new DisbursementService(context);
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
                Disbursement d = disbursementService.FindDisbursementById(did);
                viewModel.DisbursementId = d.DisbursementId;
                viewModel.Representative = d.CollectedBy == null ? "" : String.Format("{0} {1}", d.CollectedBy.FirstName, d.CollectedBy.LastName);
                viewModel.Department = d.Department.Name;
                viewModel.OrderTime = String.Format("{0} {1}", d.CreatedDateTime.ToShortDateString(), d.CreatedDateTime.ToShortTimeString());
                viewModel.CollectionPoint = d.Department.CollectionPoint.Name;
                viewModel.Status = d.Status.StatusId;
            } catch
            {
                return new HttpStatusCodeResult(400);
            }
            return View(viewModel);
        }
    }
}