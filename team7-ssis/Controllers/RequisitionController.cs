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
    public class RequisitionController : Controller
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        RequisitionService requisitionService = new RequisitionService(context);
        RetrievalService retrievalService = new RetrievalService(context);
        ItemService itemService = new ItemService(context);
        
        // GET: /Requisition
        public ActionResult ManageRequisitions()
        {
            return View();
        }

        // GET: /Requisiton/RequisitionDetails
        public ActionResult RequisitionDetails()
        {
            return View();
        }
        // GET: /Requisiton/StationeryRetrieval
        [Route("/Requisition/StationeryRetrieval")]
        [HttpGet]
        public ActionResult StationeryRetrieval(string rid)
        {
            if (rid == null)
            {
                return new HttpStatusCodeResult(400);
            }

            Retrieval r = retrievalService.FindRetrievalById(rid);
            StationeryRetrievalViewModel viewModel = new StationeryRetrievalViewModel();
            viewModel.RetrievalID = r.RetrievalId;

            try { viewModel.CreatedBy = String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName); }
            catch { viewModel.CreatedBy = ""; }
            viewModel.CreatedOn = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
            try
            {
                viewModel.UpdatedBy = String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName);
            } catch
            {
                viewModel.UpdatedBy = "";
            }
            try
            {
                viewModel.UpdatedOn = String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString());
            } catch
            {
                viewModel.UpdatedOn = "";
            }

            return View(viewModel);
        }
        // GET: /Requisiton/StationeryDisbursement
        public ActionResult StationeryDisbursement(string rid)
        {
            ViewBag.RetrievalID = rid;
            return View();
        }
        [HttpGet]
        public ActionResult RetrievalDetails(string retId, string itemId)
        {
            // TODO: Remove hardcoded values
            retId = "RET-201807-001";
            itemId = "E032";

            Item i = itemService.FindItemByItemCode(itemId);

            ViewBag.RetrievalId = retId;
            ViewBag.Item = i;

            return View();
        }
        public ActionResult CreateRequisition()
        {
            return View();
        }
    }
}