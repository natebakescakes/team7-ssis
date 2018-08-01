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
    public class RetrievalController : Controller
    {
        ApplicationDbContext context;
        ItemService itemService;
        RetrievalService retrievalService;

        public RetrievalController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            retrievalService = new RetrievalService(context);
        }

        // GET: Retrieval/RetrievalDetails
        public ActionResult RetrievalDetails(string retId, string itemId)
        {
            if (retId == null || itemId == null)
            {
                return new HttpStatusCodeResult(400);
            }

            RetrievalDetailsViewModel viewModel = new RetrievalDetailsViewModel();
            Item i = itemService.FindItemByItemCode(itemId);

            viewModel.ProductID = i.ItemCode;
            viewModel.Name = i.Name;
            viewModel.Bin = i.Bin;

            return View(viewModel);
        }

        // GET: Retrieval/Manage
        public ActionResult Manage()
        {
            return View();
        }

        // POST: Retrieval/Confirm
        [HttpPost]
        public ActionResult Confirm(string RetrievalID, List<StationeryRetrievalTableRowJSONViewModel> Data)
        {
            try
            {
                //retrievalService.SaveRetrieval(json);
                //retrievalService.ConfirmRetrieval(json.RetrievalID, "");
                //TempData["message"] = String.Format("Requisition #{0} confirmed.", json.RetrievalID);
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
            //return RedirectToAction("StationeryRetrieval","Requisition", new { rid = json.RetrievalID });
            return RedirectToAction("StationeryRetrieval", "Requisition", new { rid = RetrievalID });

        }
    }
}