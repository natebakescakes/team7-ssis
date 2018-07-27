using Microsoft.AspNet.Identity;
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
            List<Status> list = new List<Status>();
            list.Add(statusService.FindStatusByStatusId(0)); //add disabled option
            list.Add(statusService.FindStatusByStatusId(1)); //add enabled option
            return View(new ItemCategoryViewModel
            {
                Statuses = new SelectList(
                    list.Select(x => new { Value = x.StatusId, Text = x.Name }),
                     "Value",
                    "Text"

                )
            });
        }
    }
}