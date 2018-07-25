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
    public class DepartmentController : Controller
    {
        private ApplicationDbContext context;
        private DepartmentService departmentService;
        private CollectionPointService collectionPointService;
        private UserService userService;

        public DepartmentController()
        {
            context = new ApplicationDbContext();
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
        }

        // GET: Department
        [HttpGet]
        public ActionResult DepartmentOptions()
        {
            //Department department = context.Department.Find(dptcode);
            //ViewBag.CollectionPointList = collectionPointService.FindAllCollectionPoints();
            //ApplicationUser user = new ApplicationUser(); //"current user"
            //department.DepartmentCode = "COMM";
            //user.Department = department;
            //ViewBag.UsersInDepartmentList = departmentService.FindUsersByDepartment(user.Department);
            //return View(department);

            DepartmentViewModel dModel = new DepartmentViewModel();
            ConfigureViewModel(dModel);
            return View(dModel);
        }
        [HttpPost]
        public ActionResult DepartmentOptions(DepartmentViewModel dModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        context.Entry(department).State = System.Data.Entity.EntityState.Modified;
        //        //context.Entry(department).CurrentValues.SetValues(department);
        //        context.SaveChanges();
        //    }
        //    return View(department);
        //}
        {
            if (!ModelState.IsValid)
            {
                return View(dModel);
            }
            // save and redirect
            return RedirectToAction("DepartmentOptions");
        }

        private void ConfigureViewModel(DepartmentViewModel dModel)
        {
            List<CollectionPoint> collectionPoints = collectionPointService.FindAllCollectionPoints();
            dModel.collectionPointList = new SelectList(collectionPoints,"CollectionPointId","Name");
            //ApplicationUser user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            //List<ApplicationUser> usersByDepartment = departmentService.FindUsersByDepartment(user.Department);
            //dModel.usersByDepartmentList = new SelectList(usersByDepartment, )
        }

    }
}