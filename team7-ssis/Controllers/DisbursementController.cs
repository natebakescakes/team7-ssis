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
        public static ApplicationDbContext context = new ApplicationDbContext();
        DisbursementService disbursementService = new DisbursementService(context);

        // GET: Disbursement/DisbursementDetails
        public ActionResult DisbursementDetails(string did)
        {
            Disbursement d = disbursementService.FindDisbursementById(did);
            DisbursementFormViewModel viewModel = new DisbursementFormViewModel();
            viewModel.DisbursementId = d.DisbursementId;
            viewModel.Representative = d.CollectedBy == null ? "" : String.Format("{0} {1}", d.CollectedBy.FirstName, d.CollectedBy.LastName);
            viewModel.Department = d.Department.Name;
            viewModel.OrderTime = String.Format("{0} {1}", d.CreatedDateTime.ToShortDateString(), d.CreatedDateTime.ToShortTimeString());
            viewModel.CollectionPoint = d.Department.CollectionPoint.Name;
            return View(viewModel);
        }
    }
}