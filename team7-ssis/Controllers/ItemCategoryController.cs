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
        public ApplicationDbContext context { get; set; }
        StatusService statusService;
        UserService userService;
        ItemCategoryService itemcategoryService;
        public String CurrentUserName { get; set; }

        public ItemCategoryController()
        {
            context = new ApplicationDbContext();
            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }

        }

        // GET: ItemCategory
        public ActionResult Index()
        {
            statusService = new StatusService(context);

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

        //Save new or update existing ItemCategory
        [HttpPost]
        public ActionResult Save(ItemCategoryViewModel model)
        {
            bool status = false;

            statusService = new StatusService(context);
            itemcategoryService = new ItemCategoryService(context);
            userService = new UserService(context);

            ItemCategory s = new ItemCategory();

            if (itemcategoryService.FindItemCategoryByItemCategoryId(model.ItemCategoryId) == null)
            {
                //new item category 
                s.ItemCategoryId = IdService.GetNewItemCategoryId(context);
                //assign user info
                s.CreatedDateTime = DateTime.Now;
                s.CreatedBy = userService.FindUserByEmail(CurrentUserName);

            }

            else
            {
                //existing ItemCategory
                s = itemcategoryService.FindItemCategoryByItemCategoryId(model.ItemCategoryId);

                //assign user info into update fields
                s.UpdatedDateTime = DateTime.Now;
                s.UpdatedBy = userService.FindUserByEmail(CurrentUserName);

            }

            //assign item category info
            s.Name = model.Name;
            s.Description = model.Description;          
            s.Status = statusService.FindStatusByStatusId(model.Status);

            //save info to database
            if (itemcategoryService.Save(s) != null) status = true;

            //return RedirectToAction("Index", "ItemCategory");
            return new JsonResult { Data = new { status = status } };
        }

    }
}