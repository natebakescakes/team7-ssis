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
            Retrieval r = retrievalService.FindRetrievalById(retId);

            viewModel.ProductID = i.ItemCode;
            viewModel.Name = i.Name;
            viewModel.Bin = i.Bin;
            viewModel.Status = r.Status.StatusId;

            return View(viewModel);
        }

        // GET: Retrieval/Manage
        public ActionResult Manage(string msg)
        {
            if (msg != null)
            {
                ViewBag.Success = msg;
            }
            return View();
        }
    }
}