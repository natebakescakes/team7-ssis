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
        DelegationService delegationService;
        
        public DepartmentController()
        {
            context = new ApplicationDbContext();
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
            user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            statusService = new StatusService(context);
            delegationService = new DelegationService(context);

        }

        [HttpGet]
        public ActionResult DepartmentOptions()
        {
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
                ),
                //SelectedCollectionPoint = 2
            });
        }
    
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
        [HttpPost]
        public ActionResult SaveOptions(DepartmentViewModel model)
        {
            bool status = false;
            Department dpt = departmentService.FindDepartmentByUser(user);
            Delegation delegation = new Delegation();
            
          
            dpt.UpdatedDateTime = DateTime.Now;
            dpt.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());

            dpt.Representative = userService.FindUserByEmail(model.DepartmentRep);
            dpt.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(model.CollectionPoint));

            delegation.Receipient = userService.FindUserByEmail(model.DelegationRecipient);

             if (departmentService.Save(dpt) != null) status = true;

             
            
            return RedirectToAction("DepartmentOptions");



        }
        [HttpPost]
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
                dpt.Representative = userService.FindUserByEmail(model.DepartmentRep);

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
            dpt.Head = userService.FindUserByEmail(model.EmailHead);
            
            dpt.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(model.CollectionPoint));
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