using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class DepartmentController : Controller
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        DepartmentService departmentService = new DepartmentService(context);
        CollectionPointService collectionPointService = new CollectionPointService(context);
        UserService userService = new UserService(context);
        
        // GET: Department
        [HttpGet]
        public ActionResult DepartmentOptions()
        {
            DepartmentViewModel dModel = new DepartmentViewModel();
            ConfigureViewModel(dModel);
            return View(dModel);
        }
        [HttpPost]
        public ActionResult DepartmentOptions(DepartmentViewModel dModel)
        {
            //assign selected value to database
            ApplicationUser user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            //collection point
            user.Department.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(dModel.SelectedCollectionPoint.ToString()));
            //representative
            List<ApplicationUser> usersByDepartment = departmentService.FindUsersByDepartment(user.Department);
            user.Department.Representative = usersByDepartment[Convert.ToInt32(dModel.SelectedRepresentative)];//is it in the same order
            //user.Department.Representative = dModel.usersByDepartmentList.ElementAt(Convert.ToInt32(dModel.SelectedRepresentative));
            //manager
            //usersByDepartment[Convert.ToInt32(dModel.SelectedManager)].
            return View(dModel);
        }

        private void ConfigureViewModel(DepartmentViewModel dModel)
        {
            //populating lists
            List<CollectionPoint> collectionPoints = collectionPointService.FindAllCollectionPoints();
            dModel.collectionPointList = new SelectList(collectionPoints,"CollectionPointId","Name");
            ApplicationUser user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            List<ApplicationUser> usersByDepartment = departmentService.FindUsersByDepartment(user.Department);
            dModel.usersByDepartmentList = new SelectList(usersByDepartment, "Id", "FullName");
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}