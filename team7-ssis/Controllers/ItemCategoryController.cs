using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    public class ItemCategoryController : Controller
    {
        ApplicationDbContext context;
        StatusService statusService;
        UserService userService;
        ItemCategoryService itemcategoryService;

        public ItemCategoryController()
        {
            context = new ApplicationDbContext();
            statusService = new StatusService(context);
            userService = new UserService(context);
            itemcategoryService = new ItemCategoryService(context);

        }

        // GET: ItemCategory
        public ActionResult Index()
        {
            return View();
        }
    }
}