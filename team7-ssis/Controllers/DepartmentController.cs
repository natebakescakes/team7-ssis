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
        public ApplicationDbContext context;
        DepartmentService departmentService;
        CollectionPointService collectionPointService;
        UserService userService;
        ApplicationUser user;
        StatusService statusService;
        
        public DepartmentController()
        {
            context = new ApplicationDbContext();
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
            user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            statusService = new StatusService(context);

        }

        // GET: Department
        [HttpGet]
        public ActionResult DepartmentOptions()
        {
            //DepartmentViewModel dModel = new DepartmentViewModel();
            //ConfigureViewModel(dModel);
            //return View(dModel);
            List<ApplicationUser> userByDepartmentList = departmentService.FindUsersByDepartment(user.Department);

            List<CollectionPoint> collectionPointList = collectionPointService.FindAllCollectionPoints();

            return View(new DepartmentViewModel
            {
                UsersByDepartment = new SelectList(
                     userByDepartmentList.Select(x => new { value = x.Email, text = x.FirstName + " " + x.LastName }),
                     "Value",
                     "Text"
                ),
                CollectionPoints = new SelectList(
                     collectionPointList.Select(x => new { value = x.CollectionPointId, text = x.Name }),
                     "Value",
                     "Text"
                )
            });
        }
        //[HttpPost]
        //public ActionResult DepartmentOptions(DepartmentViewModel dModel)
        //{
        //    //assign selected value to database
        //    //collection point
        //    user.Department.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(dModel.SelectedCollectionPoint.ToString()));
        //    //representative
        //    List<ApplicationUser> usersByDepartment = departmentService.FindUsersByDepartment(user.Department);
        //    user.Department.Representative = usersByDepartment[Convert.ToInt32(dModel.SelectedRepresentative)];//is it in the same order
        //    //user.Department.Representative = dModel.usersByDepartmentList.ElementAt(Convert.ToInt32(dModel.SelectedRepresentative));
        //    //manager
        //    //usersByDepartment[Convert.ToInt32(dModel.SelectedManager)].
        //    return View(dModel);
        //}

        //private void ConfigureViewModel(DepartmentViewModel dModel)
        //{
        //    //populating lists
        //    List<CollectionPoint> collectionPoints = collectionPointService.FindAllCollectionPoints();
        //    dModel.collectionPointList = new SelectList(collectionPoints,"CollectionPointId","Name");
        //    List<ApplicationUser> usersByDepartment = departmentService.FindUsersByDepartment(user.Department);
        //    dModel.usersByDepartmentList = new SelectList(usersByDepartment, "Id", "FullName");
        //}
        public ActionResult Index()
        {
            List<Status> list = new List<Status>();
            list.Add(statusService.FindStatusByStatusId(0));
            list.Add(statusService.FindStatusByStatusId(1));
            List<ApplicationUser> allUsersList = userService.FindAllUsers();
            List<CollectionPoint> collectionPointList = collectionPointService.FindAllCollectionPoints();


            return View(new DepartmentViewModel
            {
                Statuses = new SelectList(
                    list.Select(x => new { Value = x.StatusId, Text = x.Name }),
                     "Value",
                    "Text"

                ),
                
                AllUsers = new SelectList(
                     allUsersList.Select(x => new { value = x.Email, text = x.FirstName+ " " +x.LastName}),
                     "Value",
                     "Text"
                ),
                CollectionPoints = new SelectList(
                     collectionPointList.Select(x => new { value = x.CollectionPointId, text = x.Name }),
                     "Value",
                     "Text"
                )
            });
          

        }
        public ActionResult SaveOptions(DepartmentViewModel model)
        {
            bool status = false;
            Department dpt = departmentService.FindDepartmentByDepartmentCode(model.DepartmentCode);
          
            dpt.UpdatedDateTime = DateTime.Now;
            dpt.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            dpt.Representative = userService.FindUserByEmail(model.DepartmentRep);
            dpt.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(model.CollectionPoint));
         
            

             if (departmentService.Save(dpt) != null) status = true;

            
            return new JsonResult { Data = new { status = status } };



        }
        public ActionResult Save(DepartmentViewModel model)
        {
            bool status = false;
            Department dpt = new Department();

            if (departmentService.FindDepartmentByDepartmentCode(model.DepartmentCode) == null)
            {
                //new dpt
                dpt.DepartmentCode = model.DepartmentCode;
                dpt.CreatedDateTime = DateTime.Now;
                dpt.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            }

            else
            {
                //edit dpt
                dpt = departmentService.FindDepartmentByDepartmentCode(model.DepartmentCode);

                //assign user info into update fields
                dpt.UpdatedDateTime = DateTime.Now;
                dpt.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            }

            
            dpt.Name = model.DepartmentName;
            dpt.Head = userService.FindUserByEmail(model.DepartmentHead);
            dpt.ContactName = model.ContactName;
            dpt.PhoneNumber = model.PhoneNumber;
            dpt.FaxNumber = model.PhoneNumber;
            dpt.Status = statusService.FindStatusByStatusId(model.Status);
            
            

            //save info to database
            if (departmentService.Save(dpt) != null) status = true;

            
            return new JsonResult { Data = new { status = status } };
        }

    }
}