using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;
using Microsoft.AspNet.Identity;

namespace team7_ssis.Controllers
{
    public class RequisitionAPIController : ApiController
    {
        ApplicationDbContext context;
        RequisitionService requisitionService;
        RequisitionRepository requisitionRepository;
        RetrievalService retrievalService;
        DisbursementService disbursementService;
        StatusService statusService;
        ItemService itemService;
        DepartmentService departmentService;
        UserRepository userRepository;
        StatusRepository statusRepository;

        public RequisitionAPIController()
        {
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
            requisitionRepository = new RequisitionRepository(context);
            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);
            statusService = new StatusService(context);
            itemService = new ItemService(context);
            departmentService = new DepartmentService(context);
            userRepository = new UserRepository(context);
            statusRepository = new StatusRepository(context);

        }

        public ApplicationDbContext Context { get { return context; } set { context = value; } }

        [Route("api/reqdetail/{rid}")]
        [HttpGet]
        public IEnumerable<RequisitionDetailVTableiewModel> GetAllRequisitionDetails(string rid)
        {
            List<RequisitionDetail> reqDetailList = requisitionService.GetRequisitionDetails(rid);
            List<RequisitionDetailVTableiewModel> viewModel = new List<RequisitionDetailVTableiewModel>();

            foreach (RequisitionDetail r in reqDetailList)
            {
                viewModel.Add(new RequisitionDetailVTableiewModel
                {
                    ItemCode = r.ItemCode,
                    Description = r.Item.Description,
                    Quantity = r.Quantity,
                    Status = r.Status.Name
                });
            }

            return viewModel;
        }

        [Route("api/requisitions")]
        [HttpGet]
        public IEnumerable<ManageRequisitionsViewModel> GetAllRequisitions()
        {
            List<Requisition> reqList = requisitionService.FindAllRequisitions();
            List<ManageRequisitionsViewModel> viewModel = new List<ManageRequisitionsViewModel>();

            foreach (Requisition r in reqList)
            {
                viewModel.Add(new ManageRequisitionsViewModel
                {
                    Requisition = r.RequisitionId,
                    Status = r.Status.Name
                });
            }

            return viewModel;
        }

        [Route("api/requisitions")]
        [HttpPost]
        public IHttpActionResult GetSelectedRequisitions(List<int> statusIdList)
        {
            List<ManageRequisitionsViewModel> viewModel = new List<ManageRequisitionsViewModel>();
            List<Requisition> reqList;

            // convert ID array to Statuses
            List<Status> statusList = new List<Status>();
            foreach (int i in statusIdList)
            {
                statusList.Add(statusRepository.FindById(i));

            }
            try
            {
                reqList = requisitionService.FindRequisitionsByStatus(statusList);
            }
            catch (ArgumentException)
            {
                return Ok();
            }

            foreach (Requisition r in reqList)
            {
                viewModel.Add(new ManageRequisitionsViewModel
                {
                    Requisition = r.RequisitionId,
                    Status = r.Status.Name
                });
            }
            return Ok(viewModel);
        }

        [Route("api/processrequisitions")]
        [HttpPost]
        public IHttpActionResult ProcessRequisitions(List<string> reqIdList)
        {
            List<Requisition> reqList = new List<Requisition>();
            string rid;
            foreach (string s in reqIdList)
            {
                reqList.Add(requisitionRepository.FindById(s));
            }
            try
            {
                rid = requisitionService.ProcessRequisitions(reqList);
            }
            catch
            {
                return BadRequest();
            }
            return Ok(rid);
        }

        [Route("api/stationerydisbursement/{rId}")]
        [HttpGet]
        public IEnumerable<StationeryDisbursementViewModel> StationeryDisbursement(string rId)
        {
            // TODO: Write Test

            List<StationeryDisbursementViewModel> viewModel = new List<StationeryDisbursementViewModel>();
            List<Disbursement> dList = disbursementService.FindDisbursementsByRetrievalId(rId);
            foreach (Disbursement d in dList)
            {
                var clerk = d.Department.CollectionPoint.ClerkInCharge;
                string disbursedBy = String.Format("{0} {1}", clerk.FirstName, clerk.LastName);

                StationeryDisbursementViewModel vm = new StationeryDisbursementViewModel();
                vm.DisbursementID = d.DisbursementId;
                vm.Department = d.Department.Name;
                vm.CollectionPoint = d.Department.CollectionPoint.Name;
                vm.DisbursedBy = disbursedBy;
                vm.Status = d.Status.Name;

                viewModel.Add(vm);

            }

            return viewModel;
        }
        [Route("api/createrequisition")]
        [HttpPost]
        public IHttpActionResult CreateRequisition(List<CreateRequisitionJSONViewModel> itemList)
        {
            ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());

            if (itemList.Count < 1)
            {
                return BadRequest("An unexpected error occured.");
            }

            Requisition r = new Requisition();
            r.RequisitionId = IdService.GetNewRequisitionId(context);
            r.RequisitionDetails = new List<RequisitionDetail>();
            r.Status = statusService.FindStatusByStatusId(4);
            r.CreatedDateTime = DateTime.Now;
            r.Department = user.Department;
            r.CollectionPoint = user.Department.CollectionPoint;
            r.CreatedBy = user;

            foreach (CreateRequisitionJSONViewModel dd in itemList)
            {
                r.RequisitionDetails.Add(new RequisitionDetail
                {
                    ItemCode = dd.ItemCode,
                    Item = itemService.FindItemByItemCode(dd.ItemCode),
                    Quantity = dd.Qty,
                    Status = statusService.FindStatusByStatusId(4)
                });
            }
            try
            {
                requisitionService.Save(r);
            }
            catch
            {
                return BadRequest("An unexpected error occured.");
            }

            // Create Notification
            new NotificationService(context).CreateNotification(r, user.Department.Head);

            return Ok(r.RequisitionId);

        }

        [Route("api/editrequisition")]
        [HttpPost]
        public IHttpActionResult EditRequisition(UpdateRequisitionJSONViewModel json)
        {
            ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());

            if (json.ItemList.Count < 1)
            {
                return BadRequest("An unexpected error occured.");
            }

            try
            {
                Requisition r = requisitionRepository.FindById(json.RequisitionId);
                List<RequisitionDetail> reqList = requisitionRepository.FindRequisitionDetails(json.RequisitionId).ToList();

                // Load exisiting repository
                requisitionRepository.FindRequisitionDetails(json.RequisitionId);
                r.RequisitionDetails = new List<RequisitionDetail>();

                foreach (CreateRequisitionJSONViewModel dd in json.ItemList)
                {
                    r.RequisitionDetails.Add(new RequisitionDetail
                    {
                        ItemCode = dd.ItemCode,
                        Item = itemService.FindItemByItemCode(dd.ItemCode),
                        Quantity = dd.Qty,
                        Status = statusService.FindStatusByStatusId(4)
                    });
                }
                requisitionService.Save(r);
            }
            catch
            {
                return BadRequest("An unexpected error occured.");
            }
            return Ok(json.RequisitionId);

        }

        /// <summary>
        /// Get requisitions requested by the caller's department for mobile view
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/requisition/department")]
        public IHttpActionResult GetRelatedRequisitions([FromBody] EmailViewModel model)
        {
            var requisitions = requisitionService.FindRequisitionsByDepartment(new UserService(context).FindUserByEmail(model.Email).Department);

            if (requisitions.Count == 0) return NotFound();

            return Ok(requisitions.Select(requisition => new RequisitionMobileViewModel()
            {
                RequisitionId = requisition.RequisitionId,
                RequestorName = $"{requisition.CreatedBy.FirstName} {requisition.CreatedBy.LastName}",
                RequestedDate = requisition.CreatedDateTime.ToShortDateString(),
                Remarks = requisition.EmployeeRemarks == null ? "" : requisition.EmployeeRemarks,
                HeadRemarks = requisition.HeadRemarks == null ? "" : requisition.HeadRemarks,
                Status = requisition.Status != null ? requisition.Status.Name : "",
                RequisitionDetails = requisition.RequisitionDetails.Select(d => new RequisitionDetailMobileViewModel()
                {
                    ItemCode = d.ItemCode,
                    Description = d.Item.Description,
                    Qty = d.Quantity,
                    Uom = d.Item.Uom,
                }).ToList()
            }));
        }

        [Route("api/requisition/approve")]
        public IHttpActionResult ApproveRequisition([FromBody] RequisitionIdViewModel model)
        {
            try
            {
                new RequisitionService(Context).ApproveRequisition(model.RequisitionId, model.Email, model.Remarks);
            }
            catch (ArgumentException)
            {
                return BadRequest("Requisition already approved!");
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully approved"
            });
        }

        [Route("api/requisition/reject")]
        public IHttpActionResult RejectRequisition([FromBody] RequisitionIdViewModel model)
        {
            try
            {
                new RequisitionService(Context).RejectRequisition(model.RequisitionId, model.Email, model.Remarks);
            }
            catch (ArgumentException)
            {
                return BadRequest("Requisition already approved!");
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully rejected"
            });
        }
    }
}
