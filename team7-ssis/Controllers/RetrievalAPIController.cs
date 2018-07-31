using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class RetrievalAPIController : ApiController
    {
        ApplicationDbContext context;
        DisbursementService disbursementService;
        RetrievalService retrievalService;

        public RetrievalAPIController()
        {
            context = new ApplicationDbContext();
            disbursementService = new DisbursementService(Context);
            retrievalService = new RetrievalService(Context);
        }

        public ApplicationDbContext Context { get { return context; } set { context = value; } }

        [Route("api/stationeryretrieval/{rId}")]
        [HttpGet]
        public IEnumerable<StationeryRetrievalTableViewModel> StationeryRetrieval(string rId)
        {
            List<Disbursement> dList = disbursementService.FindDisbursementsByRetrievalId(rId);

            // get all the relevant Disbursement Details
            List<DisbursementDetail> ddList = dList.SelectMany(x => x.DisbursementDetails).ToList();

            // group the Disbursement Details by Item Code, and sum the quantity ordered
            var finalList = ddList.GroupBy(dd => new
            {
                dd.ItemCode,
                dd.Bin
            });
            List<StationeryRetrievalTableViewModel> viewModel = finalList.Select(y => new StationeryRetrievalTableViewModel
            {
                RetrievedStatus = y.Sum(dd => dd.PlanQuantity) == y.Sum(dd => dd.ActualQuantity) ? "Picked" :
                                    (y.Sum(dd => dd.ActualQuantity) == 0 ? "Awaiting Picking" : "Partially Picked"),
                ProductID = y.Key.ItemCode,
                Bin = y.Key.Bin,
                QtyOrdered = y.Sum(dd => dd.PlanQuantity),
                Description = Context.Item.Where(x => x.ItemCode == y.Key.ItemCode).First().Description
            }).ToList();

            return viewModel;
        }
        [Route("api/stationeryretrieval")]
        [HttpPost]
        public IHttpActionResult UpdateStationeryRetrieval(StationeryRetrievalJSONViewModel viewModel)
        {
            try
            {
                retrievalService.SaveRetrieval(viewModel);
                if (viewModel.IsConfirmed == true)
                {
                    retrievalService.ConfirmRetrieval(viewModel.RetrievalID, "");
                }
            } catch
            {
                return BadRequest();
            }
            return Ok();
        }

        /// <summary>
        /// Retrieves Disbursement Details linked to a given Retrieval and Item.
        /// </summary>
        /// <param name="retId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [Route("api/retrievaldetails")]
        [HttpPost]
        public IEnumerable<RetrievalDetailsTableViewModel> RetrievalDetails(RetrievalDetailsJSON json)
        {
            List<RetrievalDetailsTableViewModel> viewModel = new List<RetrievalDetailsTableViewModel>();

            List<Disbursement> dList = disbursementService.FindDisbursementsByRetrievalId(json.retId);
            List<DisbursementDetail> ddList = dList.SelectMany(x => x.DisbursementDetails).Where(x => x.ItemCode == json.itemId).ToList();

            foreach (DisbursementDetail dd in ddList)
            {
                viewModel.Add(new RetrievalDetailsTableViewModel
                {
                    DeptId = dd.Disbursement.Department.DepartmentCode,
                    DeptName = dd.Disbursement.Department.Name,
                    Needed = dd.PlanQuantity,
                    Actual = dd.ActualQuantity
                });
            }
            return viewModel;
        }

        [Route("api/updateretrievalform")]
        [HttpPost]
        public IHttpActionResult UpdateRetrievalForm(SaveJson json)
        {
            var retrievalService = new RetrievalService(Context);
            var disbursementService = new DisbursementService(Context);

            // string retId, string itemCode, List<BreakdownByDepartment> list
            try
            {
                Retrieval r = retrievalService.FindRetrievalById(json.RetId);
                foreach (BreakdownByDepartment bd in json.List)
                {
                    Disbursement d = r.Disbursements.Where(x => x.Department.DepartmentCode == bd.DeptId).First();
                    disbursementService.UpdateActualQuantityForDisbursementDetail(d.DisbursementId, json.ItemCode, bd.Actual);
                }
            } catch
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("api/retrievals/")]
        public IHttpActionResult GetRetrievals()
        {
            var retrievals = new RetrievalService(context).FindAllRetrievals();

            if (retrievals.Count == 0)
                return NotFound();

            return Ok(retrievals.Select(retrieval => new RetrievalMobileViewModel()
            {
                RetrievalId = retrieval.RetrievalId,
                CreatedBy = retrieval.CreatedBy != null ? $"{retrieval.CreatedBy.FirstName} {retrieval.CreatedBy.LastName}" : "",
                CreatedDate = retrieval.CreatedDateTime.ToShortDateString(),
                Status = retrieval.Status.Name,
                RetrievalDetails = retrieval.Disbursements.SelectMany(d => d.DisbursementDetails.Select(dd => new RetrievalDetailByDeptViewModel()
                {
                    Department = dd.Disbursement.Department.Name,
                    DepartmentCode = dd.Disbursement.Department.DepartmentCode,
                    ItemCode = dd.ItemCode,
                    ItemName = dd.Item.Name,
                    Bin = dd.Bin,
                    PlanQuantity = dd.PlanQuantity,
                    ActualQuantity = dd.ActualQuantity,
                    Status = dd.Status.Name,
                    Uom = dd.Item.Uom,
                })).ToList(),
            }));
        }

        [Route("api/retrieval/updatequantity")]
        public IHttpActionResult UpdateActualQuantity([FromBody] UpdateActualQuantityViewModel model)
        {
            try
            {
                new RetrievalService(Context).UpdateActualQuantity(model.RetrievalId, model.Email, model.ItemCode, model.RetrievalDetails);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully updated"
            });
        }

        [Route("api/retrieval/retrieveitem")]
        public IHttpActionResult RetrieveItem([FromBody] ConfirmRetrievalViewModel model)
        {
            try
            {
                new RetrievalService(Context).RetrieveItem(model.RetrievalId, model.Email, model.ItemCode);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully retrieved"
            });
        }

        [Route("api/retrieval/confirm")]
        public IHttpActionResult ConfirmRetrieval([FromBody] ConfirmRetrievalViewModel model)
        {
            try
            {
                new RetrievalService(Context).ConfirmRetrieval(model.RetrievalId, model.Email);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(new MessageViewModel()
            {
                Message = "Successfully confirmed"
            });
        }

        [Route("api/retrieval/{id}")]
        public IHttpActionResult GetRetrieval([FromBody] ConfirmRetrievalViewModel model)
        {
            var retrieval = new RetrievalService(context).FindRetrievalById(model.RetrievalId);

            if (retrieval == null)
                return NotFound();

            return Ok(retrieval.Disbursements
                .SelectMany(d => d.DisbursementDetails
                    .Select(dd => new RetrievalDetailByDeptViewModel()
                    {
                        Department = dd.Disbursement.Department.Name,
                        DepartmentCode = dd.Disbursement.Department.DepartmentCode,
                        ItemCode = dd.ItemCode,
                        ItemName = dd.Item.Name,
                        Bin = dd.Bin,
                        PlanQuantity = dd.PlanQuantity,
                        ActualQuantity = dd.ActualQuantity,
                        Status = dd.Status.Name,
                        Uom = dd.Item.Uom,
                    })
                ));
        }

        [Route("api/retrieval")]
        [HttpGet]
        public IEnumerable<ManageRetrievalsViewModel> GetAllRetrievals()
        {
            List<Retrieval> retList = retrievalService.FindAllRetrievals();
            List<ManageRetrievalsViewModel> viewModel = new List<ManageRetrievalsViewModel>();

            foreach(Retrieval r in retList)
            {
                viewModel.Add(new ManageRetrievalsViewModel
                {
                    RetrievalId = r.RetrievalId,
                    CreatedBy = r.CreatedBy != null ? String.Format("{0} {1}", r.CreatedBy.FirstName, r.CreatedBy.LastName) : "",
                    CreatedDate = r.CreatedDateTime.ToShortDateString(),
                    Status = r.Status.Name
                });
            }
            return viewModel;
        }
    }
}
