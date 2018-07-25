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
        static ApplicationDbContext context = new ApplicationDbContext();
        RequisitionService requisitionService = new RequisitionService(context);
        RequisitionRepository requisitionRepository = new RequisitionRepository(context);
        RetrievalService retrievalService = new RetrievalService(context);
        ItemService itemService = new ItemService(context);
        CollectionPointService collectionPointService = new CollectionPointService(context);
        DepartmentService departmentService = new DepartmentService(context);
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        // GET: /Requisition
        public ActionResult ManageRequisitions()
        {
            return View();
        }

        // GET: /Requisiton/RequisitionDetails
        public ActionResult RequisitionDetails(string reqId)
        {
            Requisition r = requisitionRepository.FindById(reqId);
            if (reqId == null)
            {
                return new HttpStatusCodeResult(400);
            }

            RequisitionDetailViewModel viewModel = new RequisitionDetailViewModel();
            
            viewModel.RequisitionID = r.RequisitionId;
            viewModel.Department = r.Department == null ? "" : r.Department.Name;
            viewModel.CollectionPoint = r.CollectionPoint == null ? "" : r.CollectionPoint.Name;
            viewModel.CreatedBy = r.CreatedBy == null ? "" : String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName);
            viewModel.CreatedTime = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
            viewModel.UpdatedBy = r.UpdatedBy == null ? "" : String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName);
            viewModel.UpdatedTime = r.UpdatedDateTime == null ? "" : String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString());
            viewModel.ApprovedBy = r.ApprovedBy == null ? "" : String.Format("{0} {1}", r.ApprovedBy.FirstName, r.ApprovedBy.LastName);
            viewModel.ApprovedTime = r.ApprovedDateTime == null ? "" : String.Format("{0} {1}", r.ApprovedDateTime.Value.ToShortDateString(), r.ApprovedDateTime.Value.ToShortTimeString());

            return View(viewModel);
        }
        // GET: /Requisiton/StationeryRetrieval
        [Route("/Requisition/StationeryRetrieval")]
        public ActionResult StationeryRetrieval(string rid)
        {
            if (rid == null)
            {
                return new HttpStatusCodeResult(400);
            }

            Retrieval r = retrievalService.FindRetrievalById(rid);
            StationeryRetrievalViewModel viewModel = new StationeryRetrievalViewModel();
            viewModel.RetrievalID = r.RetrievalId;

            try { viewModel.CreatedBy = String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName); }
            catch { viewModel.CreatedBy = ""; }
            viewModel.CreatedOn = String.Format("{0} {1}", r.CreatedDateTime.ToShortDateString(), r.CreatedDateTime.ToShortTimeString());
            try
            {
                viewModel.UpdatedBy = String.Format("{0} {1}", r.UpdatedBy.FirstName, r.UpdatedBy.LastName);
            } catch
            {
                viewModel.UpdatedBy = "";
            }
            try
            {
                viewModel.UpdatedOn = String.Format("{0} {1}", r.UpdatedDateTime.Value.ToShortDateString(), r.UpdatedDateTime.Value.ToShortTimeString());
            } catch
            {
                viewModel.UpdatedOn = "";
            }

            return View(viewModel);
        }
        // GET: /Requisiton/StationeryDisbursement
        public ActionResult StationeryDisbursement(string rid)
        {
            ViewBag.RetrievalID = rid;
            return View();
        }
        
        public ActionResult CreateRequisition()
        {
            CreateRequisitionsViewModel viewModel = new CreateRequisitionsViewModel();
            try
            {
                viewModel.Representative = departmentService.FindDepartmentByUser(userManager.FindById(User.Identity.GetUserId())).ContactName;
            } catch
            {
                viewModel.Representative = "";
            }
            viewModel.SelectCollectionPointList = collectionPointService.FindAllCollectionPoints();
            return View(viewModel);
        }
    }
}