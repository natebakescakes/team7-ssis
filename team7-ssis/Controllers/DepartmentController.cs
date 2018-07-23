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
        public static ApplicationDbContext context = new ApplicationDbContext();
        DepartmentService departmentService = new DepartmentService(context);
        CollectionPointService collectionPointService = new CollectionPointService(context);
        // GET: Department
        [HttpGet]
        public ActionResult DepartmentOptions()
        {
            //get current user
            //var user = User.Identity;
            //Department department = context.Department.Find(dptcode);
            //ViewBag.CollectionPointList = collectionPointService.FindAllCollectionPoints();
            //ApplicationUser user = new ApplicationUser(); //"current user"
            //department.DepartmentCode = "COMM";
            //user.Department = department;
            //ViewBag.UsersInDepartmentList = departmentService.FindUsersByDepartment(user.Department);
            //return View(department);
            DepartmentViewModel dModel = new DepartmentViewModel();
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
        }

    }
}