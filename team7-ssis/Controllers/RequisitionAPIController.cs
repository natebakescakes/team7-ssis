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
        DepartmentRepository departmentRepository;
        UserRepository userRepository;
        StatusRepository statusRepository;
        CollectionPointRepository collectionPointRepository;

        public RequisitionAPIController()
        {
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
            requisitionRepository = new RequisitionRepository(context);
            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);
            statusService = new StatusService(context);
            itemService = new ItemService(context);
            departmentRepository = new DepartmentRepository(context);
            userRepository = new UserRepository(context);
            statusRepository = new StatusRepository(context);
            collectionPointRepository = new CollectionPointRepository(context);

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
            // get current user
            ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());

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
                // find Requisition By Status
                reqList = requisitionService.FindRequisitionsByStatus(statusList);

                // if user is Employee or Department Head
                if (user.Roles.Where(x => x.RoleId == "1" || x.RoleId == "2").Count() > 0) {
                    reqList = reqList.Where(x => x.Department == user.Department).ToList();
                }
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
            List<string> errorList = new List<string>();

            try
            {
                foreach (string s in reqIdList)
                {
                    Requisition req = requisitionRepository.FindById(s);
                    if (req.Status.StatusId == 6)
                    {
                        reqList.Add(req);
                    } else
                    {
                        errorList.Add(req.RequisitionId);
                    }
                }

                // create retrieval only if there are Requisitions to be processed
                if (reqList.Count > 0)
                {
                    rid = requisitionService.ProcessRequisitions(reqList);
                } else
                {
                    throw new Exception("No Requisitions could be processed.");
                } 
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok( new { rid, count = reqList.Count } );
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
        public IHttpActionResult CreateRequisition(UpdateRequisitionJSONViewModel json)
        {
            ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());

            if (json.ItemList.Count < 1)
            {
                return BadRequest("An unexpected error occured.");
            }

            // create the requisition
            Requisition r = new Requisition();
            r.RequisitionId = IdService.GetNewRequisitionId(context);
            r.RequisitionDetails = new List<RequisitionDetail>();
            if (json.IsDraft == true)
            {
                r.Status = statusService.FindStatusByStatusId(3);
            } else
            {
                r.Status = statusService.FindStatusByStatusId(4);
            }
            r.CreatedDateTime = DateTime.Now;
            r.Department = user.Department;
            r.CollectionPoint = user.Department.CollectionPoint;
            r.CreatedBy = user;

            // create requisition details
            foreach (UpdateRequisitionTableJSONViewModel dd in json.ItemList)
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

        //[Route("api/createdraftrequisition/")]
        //[HttpPost]
        //public IHttpActionResult CreateDraftRequisition([FromBody] string rid)
        //{
        //    ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());
        //    // for testing
        //    //ApplicationUser user = userRepository.FindById("446a381c-ff6c-4332-ba50-747af26d996e");

        //    Requisition existingReq = requisitionRepository.FindById(rid);

        //    // create the requisition
        //    Requisition r = new Requisition();
        //    r.RequisitionId = IdService.GetNewRequisitionId(context);
        //    r.RequisitionDetails = new List<RequisitionDetail>();
        //    r.Status = statusService.FindStatusByStatusId(3); // create a draft
        //    r.CreatedDateTime = DateTime.Now;
        //    r.Department = user.Department;
        //    r.CollectionPoint = user.Department.CollectionPoint;
        //    r.CreatedBy = user;

        //    // create requisition details
        //    foreach (RequisitionDetail dd in existingReq.RequisitionDetails)
        //    {
        //        r.RequisitionDetails.Add(new RequisitionDetail
        //        {
        //            ItemCode = dd.ItemCode,
        //            Item = itemService.FindItemByItemCode(dd.ItemCode),
        //            Quantity = dd.Quantity,
        //            Status = statusService.FindStatusByStatusId(4)
        //        });
        //    }
        //    try
        //    {
        //        requisitionService.Save(r);
        //    }
        //    catch
        //    {
        //        return BadRequest("An unexpected error occured.");
        //    }

        //    // Create Notification
        //    new NotificationService(context).CreateNotification(r, user.Department.Head);

        //    return Ok(r.RequisitionId);
        //}

        [Route("api/editrequisition")]
        [HttpPost]
        public IHttpActionResult EditRequisition(UpdateRequisitionJSONViewModel json)
        {
            ApplicationUser user = userRepository.FindById(RequestContext.Principal.Identity.GetUserId());

            // update the collection point
            Department d = departmentRepository.FindById(user.Department.DepartmentCode);
            d.CollectionPoint = collectionPointRepository.FindById(json.CollectionPointId);
            departmentRepository.Save(d);

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

                foreach (UpdateRequisitionTableJSONViewModel dd in json.ItemList)
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
