using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ApplicationDbContext context { get; set; }
        DepartmentService departmentService;
        CollectionPointService collectionPointService;
        UserService userService;
        StatusService statusService;
        DelegationService delegationService;

        public String CurrentUserName { get; set; }

        public DepartmentController()
        {
            context = new ApplicationDbContext();

            try
            {
                CurrentUserName = System.Web.HttpContext.Current.User.Identity.Name;
            }
            catch (NullReferenceException) { }

            context = new ApplicationDbContext();
            //departmentService = new DepartmentService(context);
            //collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
            //user = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            //statusService = new StatusService(context);
            //delegationService = new DelegationService(context);
            //userRepository = new UserRepository(context);
        }

        [HttpGet]
        public ActionResult DepartmentOptions()
        {
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);

            List<ApplicationUser> userByDepartmentList = departmentService.FindUsersByDepartment(userService.FindUserByEmail(CurrentUserName).Department);
            List<CollectionPoint> collectionPointList = collectionPointService.FindAllCollectionPoints();
            CollectionPoint collectionPoint = collectionPointService.FindCollectionPointByDepartment(userService.FindUserByEmail(CurrentUserName).Department);
            ApplicationUser departmentRep = userService.FindRepresentativeByDepartment(userService.FindUserByEmail(CurrentUserName).Department);
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
                CollectionPoint = collectionPoint.Name, //sets current value as default value on dropdownlist
                DepartmentRep = departmentRep.FirstName + " " + departmentRep.LastName
             
            });
        }
    
        public ActionResult Index()
        {
            statusService = new StatusService(context);
            userService = new UserService(context);
            collectionPointService = new CollectionPointService(context);

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
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
            statusService = new StatusService(context);
            delegationService = new DelegationService(context);

            bool status = false;
            Department dpt = departmentService.FindDepartmentByUser(userService.FindUserByEmail(CurrentUserName));
            Delegation delegation = new Delegation();
            
            dpt.UpdatedDateTime = DateTime.Now;
            dpt.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
            dpt.Representative = userService.FindUserByEmail(model.DepartmentRep);
            dpt.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(model.CollectionPoint));

            if (departmentService.Save(dpt) != null) status = true;

            delegation.Receipient = userService.FindUserByEmail(model.DelegationRecipient);

            if (delegation.Receipient != null)
            {
                delegation.DelegationId = IdService.GetNewDelegationId(context);
                delegation.StartDate = DateTime.Parse(model.StartDate, new CultureInfo("fr-FR", false));
                delegation.EndDate = DateTime.Parse(model.EndDate, new CultureInfo("fr-FR", false));
                delegation.CreatedDateTime = DateTime.Now;
                //delegation.CreatedBy = user;
                delegation.CreatedBy = userService.FindUserByEmail(CurrentUserName); 
                delegation.Status = statusService.FindStatusByStatusId(1);
                delegationService.DelegateManager(delegation);
            }
            return new JsonResult { Data = new { status = status } };
           
        }
        [HttpPost]
        public ActionResult SaveStatus(DepartmentViewModel model)
        {
            statusService = new StatusService(context);
            delegationService = new DelegationService(context);

            bool status = false;
            Delegation delegation = delegationService.FindDelegationByDelegationId(model.DelegationId);
            delegation.Status = statusService.FindStatusByStatusId(model.DelegationStatus);
            //delegation.UpdatedBy = user;
            delegation.CreatedBy = userService.FindUserByEmail(CurrentUserName);
            delegation.UpdatedDateTime = DateTime.Now;

            if (delegationService.DelegateManager(delegation) != null) status = true;

            return new JsonResult { Data = new { status = status } };
        }
        [HttpPost]
        public ActionResult Save(DepartmentViewModel model)
        {
            departmentService = new DepartmentService(context);
            collectionPointService = new CollectionPointService(context);
            userService = new UserService(context);
            statusService = new StatusService(context);

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
                //dpt.UpdatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                dpt.UpdatedBy = userService.FindUserByEmail(CurrentUserName);

            }
            
            dpt.Name = model.DepartmentName;
            dpt.Head = userService.FindUserByEmail(model.EmailHead);
            
            dpt.CollectionPoint = collectionPointService.FindCollectionPointById(Convert.ToInt32(model.CollectionPointId));
            dpt.ContactName = model.ContactName;
            dpt.PhoneNumber = model.PhoneNumber;
            dpt.FaxNumber = model.FaxNumber;
            dpt.Status = statusService.FindStatusByStatusId(model.Status);
            
            //save info to database
            if (departmentService.Save(dpt) != null) status = true;
            
            return new JsonResult { Data = new { status = status } };
        }
    }
}