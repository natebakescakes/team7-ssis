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
        static ApplicationDbContext context = new ApplicationDbContext();
        ItemService itemService = new ItemService(context);

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
    }
}