using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;

namespace team7_ssis.Controllers
{
    public class InventoryController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        ItemService itemService = new ItemService(context);

        // GET: Inventory
        public ActionResult Index()
        {
            return View();
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