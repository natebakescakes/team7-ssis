using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class RequisitionController : Controller
    {
        ApplicationDbContext context;
        RequisitionRepository requisitionRepository;
        UserRepository userRepository;

        RequisitionService requisitionService;
        RetrievalService retrievalService;
        ItemService itemService;
        CollectionPointService collectionPointService;
        DepartmentService departmentService;
        StatusService statusService;

        UserManager<ApplicationUser> userManager;

        public RequisitionController()
        {
            context = new ApplicationDbContext();
            requisitionRepository = new RequisitionRepository(context);
            userRepository = new UserRepository(context);

            requisitionService = new RequisitionService(context);
            retrievalService = new RetrievalService(context);
            itemService = new ItemService(context);
            collectionPointService = new CollectionPointService(context);
            departmentService = new DepartmentService(context);
            statusService = new StatusService(context);

            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        // GET/POST: /Requisition/ManageRequisitions
        public ActionResult ManageRequisitions(string msg)
        {
            if (msg != null)
            {
                ViewBag.Info = msg;
            }
            // To pass messages from another controller
            if (TempData["cancel"] != null)
            {
                ViewBag.Danger = TempData["cancel"];
            }
            if (TempData["approve"] != null)
            {
                ViewBag.Success = TempData["approve"];
            }
            if (TempData["reject"] != null)
            {
                ViewBag.Danger = TempData["reject"];
            }
            if (TempData["draft"] != null)
            {
                ViewBag.Success = TempData["draft"];
            }
            if (TempData["error"] != null)
            {
                ViewBag.Danger = TempData["error"];
            }

            // pass the statuses for the appropriate Role
            HashSet<int> adminSet = new HashSet<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 21 };
            HashSet<int> empSet = new HashSet<int> { 2, 3, 4, 5, 6, 7, 8, 9, 10, 21 };
            HashSet<int> deptHeadSet = new HashSet<int> { 3, 4, 5, 6, 21 };
            HashSet<int> storeClerkSet = new HashSet<int> { 6, 7, 8, 9, 10, 21 };

            HashSet<int> statuses = new HashSet<int>();
            foreach( string role in userManager.GetRoles(User.Identity.GetUserId()))
            {
                if (role == "Employee")
                {
                    statuses.UnionWith(empSet);
                }
                if (role == "DepartmentHead")
                {
                    statuses.UnionWith(deptHeadSet);
                }
                if (role == "Store Clerk")
                {
                    statuses.UnionWith(storeClerkSet);
                }
                if (role == "Admin")
                {
                    statuses.UnionWith(adminSet);
                }
            }
            // Convert to List to allow JS to easily parse
            List<int> statusList = statuses.ToList();

            return View(statusList);
        }

        // GET: /Requisiton/RequisitionDetails
        public ActionResult RequisitionDetails(string rid)
        {
            Requisition r = requisitionRepository.FindById(rid);
            RequisitionDetailViewModel viewModel = new RequisitionDetailViewModel();
            try
            {
                viewModel.Status = r.Status.Name;
                viewModel.RequisitionID = r.RequisitionId;
                try
                {
                    viewModel.DisbursementId = r.Retrieval.Disbursements.Where(x => x.Department.DepartmentCode == r.Department.DepartmentCode).First().DisbursementId;
                } catch {
                    viewModel.DisbursementId = "";
                }
                viewModel.Department = r.Department == null ? "" : r.Department.Name;
                viewModel.CollectionPoint = r.CollectionPoint == null ? "" : r.CollectionPoint.Name;
                viewModel.CreatedBy = r.CreatedBy == null ? "" : String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName);
                viewModel.CreatedTime = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
                viewModel.UpdatedBy = r.UpdatedBy == null ? "" : String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName);
                viewModel.UpdatedTime = r.UpdatedDateTime == null ? "" : String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString());
                viewModel.ApprovedBy = r.ApprovedBy == null ? "" : String.Format("{0} {1}", r.ApprovedBy.FirstName, r.ApprovedBy.LastName);
                viewModel.ApprovedTime = r.ApprovedDateTime == null ? "" : String.Format("{0} {1}", r.ApprovedDateTime.Value.ToShortDateString(), r.ApprovedDateTime.Value.ToShortTimeString());
                viewModel.Remarks = r.HeadRemarks;
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
            return View(viewModel);
        }
        // GET (or POST): /Requisiton/StationeryRetrieval
        public ActionResult StationeryRetrieval(string rid, string message)
        {
            Retrieval r = retrievalService.FindRetrievalById(rid);
            StationeryRetrievalViewModel viewModel = new StationeryRetrievalViewModel();

            if (TempData["message"] != null)
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            else
            {
                ViewBag.Message = message;
            }

            try
            {
                viewModel.StatusId = r.Status.StatusId;
                viewModel.RetrievalID = r.RetrievalId;
                viewModel.CreatedBy = r.CreatedBy != null ? String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName) : "";
                viewModel.CreatedOn = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
                viewModel.UpdatedBy = r.UpdatedBy != null ? String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName) : "";
                viewModel.UpdatedOn = r.UpdatedDateTime != null ? String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString()) : "";
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
            return View(viewModel);
        }
        // GET: /Requisiton/StationeryDisbursement
        public ActionResult StationeryDisbursement(string rid)
        {
            if (rid == null)
            {
                return new HttpStatusCodeResult(400);
            }
            else if (TempData["did"] != null)
            {
                ViewBag.DisbursementId = TempData["did"];
            }
            return View();
        }

        // GET: /Requisiton/CreateRequisition
        public ActionResult CreateRequisition()
        {
            CreateRequisitionViewModel viewModel = new CreateRequisitionViewModel();
            viewModel.SelectCollectionPointList = collectionPointService.FindAllCollectionPoints();

            try
            {
                viewModel.Representative = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).ContactName;
            }
            catch
            {
                viewModel.Representative = "";
            }
            try
            {
                viewModel.CollectionPointId = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).CollectionPoint.CollectionPointId;
            } catch
            {
                viewModel.CollectionPointId = -1;
            }

            return View(viewModel);
        }

        // GET: /Requisiton/EditRequisition
        public ActionResult EditRequisition(string rid)
        {
            if (rid == null)
            {
                return new HttpStatusCodeResult(400);
            }

            EditRequisitionViewModel viewModel = new EditRequisitionViewModel();
            viewModel.SelectCollectionPointList = collectionPointService.FindAllCollectionPoints();
            viewModel.RequisitionId = rid;
            viewModel.StatusId = requisitionRepository.FindById(rid).Status.StatusId;

            try
            {
                viewModel.Representative = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).ContactName;
            }
            catch
            {
                viewModel.Representative = "";
            }
            try
            {
                viewModel.CollectionPointId = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).CollectionPoint.CollectionPointId;
            }
            catch
            {
                viewModel.CollectionPointId = -1;
            }

            return View(viewModel);
        }

        // POST: /Requisition/Approve
        public ActionResult Approve(string rid, string email, string remarks)
        {
            var checkEmail = email;
            if (checkEmail == "")
                checkEmail = System.Web.HttpContext.Current.User.Identity.GetUserName();

            requisitionService.ApproveRequisition(rid, checkEmail, remarks);
            TempData["approve"] = String.Format("Requisition #{0} approved.", rid);
            return RedirectToAction("ManageRequisitions", "Requisition" );
        }

        // POST: /Requisition/Reject
        public ActionResult Reject(string rid, string email, string remarks)
        {
            var checkEmail = email;
            if (checkEmail == "")
                checkEmail = System.Web.HttpContext.Current.User.Identity.GetUserName();

            requisitionService.RejectRequisition(rid, checkEmail, remarks);
            TempData["reject"] = String.Format("Requisition #{0} rejected.", rid);
            return RedirectToAction("ManageRequisitions", "Requisition");
        }

        // POST: /Requisition/Cancel
        public ActionResult Cancel(string rid, string email)
        {
            try
            {
                requisitionService.UpdateRequisitionStatus(rid, 2, "");
                TempData["cancel"] = String.Format("Requisition #{0} cancelled.", rid);
            } catch
            {
                return RedirectToAction("ManageRequisitions", "EditRequisition", new { rid });
            }
            return RedirectToAction("ManageRequisitions", "Requisition");
        }

        // POST: /Requisition/Draft
        public ActionResult Draft(string rid)
        {
            ApplicationUser user = userRepository.FindById(User.Identity.GetUserId());
            // for testing
            //ApplicationUser user = userRepository.FindById("446a381c-ff6c-4332-ba50-747af26d996e");

            Requisition existingReq = requisitionRepository.FindById(rid);

            // create the requisition
            Requisition r = new Requisition();
            r.RequisitionId = IdService.GetNewRequisitionId(context);
            r.RequisitionDetails = new List<RequisitionDetail>();
            r.Status = statusService.FindStatusByStatusId(3); // create a draft
            r.CreatedDateTime = DateTime.Now;
            r.Department = user.Department;
            r.CollectionPoint = user.Department.CollectionPoint;
            r.CreatedBy = user;

            // create requisition details
            foreach (RequisitionDetail dd in existingReq.RequisitionDetails)
            {
                r.RequisitionDetails.Add(new RequisitionDetail
                {
                    ItemCode = dd.ItemCode,
                    Item = itemService.FindItemByItemCode(dd.ItemCode),
                    Quantity = dd.Quantity,
                    Status = statusService.FindStatusByStatusId(4)
                });
            }
            try
            {
                requisitionService.Save(r);

                // Create Notification
                new NotificationService(context).CreateNotification(r, user.Department.Head);
            }
            catch
            {
                TempData["error"] = "error";
                return RedirectToAction("ManageRequisitions", "Requisition");
            }

            TempData["draft"] = String.Format("Requisition #{0} created.", r.RequisitionId);
            return RedirectToAction("ManageRequisitions", "Requisition");
        }

    }
}