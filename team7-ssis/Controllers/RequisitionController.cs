using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class RequisitionController : Controller
    {
        private ApplicationDbContext context;
        private RequisitionService requisitionService;
        private RequisitionRepository requisitionRepository;
        private RetrievalService retrievalService;
        private ItemService itemService;
        private CollectionPointService collectionPointService;
        private DepartmentService departmentService;
        private UserManager<ApplicationUser> userManager;

        public RequisitionController()
        {
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
            requisitionRepository = new RequisitionRepository(context);
            retrievalService = new RetrievalService(context);
            itemService = new ItemService(context);
            collectionPointService = new CollectionPointService(context);
            departmentService = new DepartmentService(context);
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        }

        // GET: /Requisition/ManageRequisitions
        public ActionResult ManageRequisitions(string msg)
        {
            if (TempData["cancel"] != null)
            {
                ViewBag.Cancel = TempData["cancel"];
            }
            return View();
        }

        // GET: /Requisiton/RequisitionDetails
        public ActionResult RequisitionDetails(string rid)
        {
            Requisition r = requisitionRepository.FindById(rid);
            RequisitionDetailViewModel viewModel = new RequisitionDetailViewModel();
            try
            {
                viewModel.RequisitionID = r.RequisitionId;
                viewModel.Department = r.Department == null ? "" : r.Department.Name;
                viewModel.CollectionPoint = r.CollectionPoint == null ? "" : r.CollectionPoint.Name;
                viewModel.CreatedBy = r.CreatedBy == null ? "" : String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName);
                viewModel.CreatedTime = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
                viewModel.UpdatedBy = r.UpdatedBy == null ? "" : String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName);
                viewModel.UpdatedTime = r.UpdatedDateTime == null ? "" : String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString());
                viewModel.ApprovedBy = r.ApprovedBy == null ? "" : String.Format("{0} {1}", r.ApprovedBy.FirstName, r.ApprovedBy.LastName);
                viewModel.ApprovedTime = r.ApprovedDateTime == null ? "" : String.Format("{0} {1}", r.ApprovedDateTime.Value.ToShortDateString(), r.ApprovedDateTime.Value.ToShortTimeString());
            }
            catch
            {
                return new HttpStatusCodeResult(400);
            }
            return View(viewModel);
        }
        // GET: /Requisiton/StationeryRetrieval
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
            viewModel.Action = "Create";
            viewModel.SelectCollectionPointList = collectionPointService.FindAllCollectionPoints();

            try
            {
                viewModel.Representative = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).ContactName;
            }
            catch
            {
                viewModel.Representative = "";
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
            viewModel.Action = "Edit";
            viewModel.SelectCollectionPointList = collectionPointService.FindAllCollectionPoints();
            viewModel.RequisitionId = rid;

            try
            {
                viewModel.Representative = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).ContactName;
            }
            catch
            {
                viewModel.Representative = "";
            }

            return View(viewModel);
        }

        // POST: /Requisition/Approve
        public ActionResult Approve(string rid, string email, string remarks)
        {
            requisitionService.ApproveRequisition(rid, email, remarks);
            return View("../Requisition/ManageRequisitions");
        }

        // POST: /Requisition/Reject
        public ActionResult Reject(string rid, string email, string remarks)
        {
            requisitionService.RejectRequisition(rid, email, remarks);
            return View("../Requisition/ManageRequisitions");
        }

        // POST: /Requisition/Cancel
        public ActionResult Cancel(string rid, string email)
        {
            try
            {
                requisitionService.UpdateRequisitionStatus(rid, 2, "");
                TempData["cancel"] = rid;
            } catch
            {
                return RedirectToAction("ManageRequisitions", "EditRequisition", new { rid });
            }
            return RedirectToAction("ManageRequisitions", "Requisition");
        }

    }
}