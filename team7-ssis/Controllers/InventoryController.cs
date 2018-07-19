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

        public ActionResult Manage()
        {
            //List<Item> items = itemService.FindAllItems();
            //ViewData["Items"] = items;
            //return View();

            using (var client = new HttpClient())
            {
                var inventoryItemUrl = Url.RouteUrl(
                    "DefaultApi",
                    new { httproute = "", controller = "InventoryAPI" },
                    Request.Url.Scheme
                );
                var model = client
                            .GetAsync(inventoryItemUrl)
                            .Result
                            .Content.ReadAsAsync<Item>().Result;

                return View(model);
            }
        }

        
    }
}