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

        public RequisitionAPIController()
        {
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
            requisitionRepository = new RequisitionRepository(context);
            retrievalService = new RetrievalService(context);
            disbursementService = new DisbursementService(context);
            statusService = new StatusService(context);
            itemService = new ItemService(context);
        }

        [Route("api/reqdetail/all")]
        [HttpGet]
        public IEnumerable<ManageRequisitionsViewModel> Requisitions()
        {
            List<RequisitionDetail> reqDetailList = requisitionService.FindAllRequisitionDetail();
            List<ManageRequisitionsViewModel> viewModel = new List<ManageRequisitionsViewModel>();

            foreach (RequisitionDetail r in reqDetailList)
            {
                string status;
                if (r.Status != null)
                {
                    int statusId = r.Status.StatusId;
                    status = context.Status.Where(x => x.StatusId == statusId).First().Name;
                } else
                {
                    status = "";
                }

                viewModel.Add(new ManageRequisitionsViewModel
                {
                    Requisition = r.RequisitionId,
                    ItemCode = r.ItemCode,
                    Description = r.Item.Description,
                    Quantity = r.Quantity,
                    Status = status
                });
            }

            return viewModel;
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
            } catch
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

                viewModel.Add(new StationeryDisbursementViewModel
                {
                    DisbursementID = d.DisbursementId,
                    Department = d.Department.Name,
                    //CollectionPoint = d.Department.CollectionPoint.Name,
                    //DisbursedBy = disbursedBy,
                    //Status = d.Status.Name
                });
            }

            return viewModel;
        }
        [Route("api/createrequisition")]
        [HttpPost]
        public IHttpActionResult CreateRequisition(List<CreateRequisitionJSONViewModel> itemList)
        {
            if (itemList.Count < 1)
            {
                return BadRequest("An unexpected error occured.");
            }

            Requisition r = new Requisition();
            r.RequisitionId = IdService.GetNewRequisitionId(context);
            r.RequisitionDetails = new List<RequisitionDetail>();
            r.Status = statusService.FindStatusByStatusId(4);
            r.CreatedDateTime = DateTime.Now;
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
            } catch
            {
                return BadRequest("An unexpected error occured.");
            }
            return Ok(r.RequisitionId);
            
        }
    }
}
