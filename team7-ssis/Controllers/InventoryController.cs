using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class InventoryController : Controller
    {
        public ApplicationDbContext context;
        ItemService itemService;
        StatusService statusService;

        public InventoryController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            statusService = new StatusService(context);
        }
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        //GET: Inventory Detail
        public ActionResult Details(string itemCode)
        {
            ViewBag.VB = itemCode;
            List<Status> list = new List<Status>();
            list.Add(statusService.FindStatusByStatusId(0));
            list.Add(statusService.FindStatusByStatusId(1));
            return View(new EditItemFinalViewModel {
                Statuses = new SelectList(
                    list.Select(x => new { Value = x.StatusId, Text = x.Name }),
                     "Value",
                    "Text"
                )
            });
        }



        [HttpPost]
        public ActionResult ImageUpload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    int i=itemService.UploadItemImage(file);
                    if (i == 1)
                    {
                        ViewBag.Message = "File uploaded successfully";
                    }
                    else
                    {
                        ViewBag.Message = "File uploaded unsuccessful!";
                    }
                   
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("Manage");
        }
    }
}