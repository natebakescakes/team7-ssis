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
            disbursementService = new DisbursementService(context);
            retrievalService = new RetrievalService(context);
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
                AllRetrieved = (y.Sum(dd => dd.PlanQuantity)) == (y.Sum(dd => dd.ActualQuantity)) ? true : false,
                ProductID = y.Key.ItemCode,
                Bin = y.Key.Bin,
                QtyOrdered = y.Sum(dd => dd.PlanQuantity),
                Description = context.Item.Where(x => x.ItemCode == y.Key.ItemCode).First().Description
            }).ToList();

            return viewModel;
        }
        [Route("api/stationeryretrieval")]
        [HttpPost]
        public IHttpActionResult StationeryRetrieval(StationeryRetrievalJSONViewModel viewModel)
        {
            try
            {
                List<Disbursement> disbList = disbursementService.FindDisbursementsByRetrievalId(viewModel.RetrievalID);
                List<DisbursementDetail> ddList = disbList.SelectMany(x => x.DisbursementDetails).ToList();
                foreach (var row in viewModel.Data)
                {
                    if (row.AllRetrieved == true)
                    {
                        ddList
                        .Where(x => x.ItemCode == row.ProductID)
                        .ToList()
                        .ForEach(x => x.ActualQuantity = x.PlanQuantity);
                    }
                    else
                    {
                        ddList
                        .Where(x => x.ItemCode == row.ProductID)
                        .ToList()
                        .ForEach(x => x.ActualQuantity = 0);
                    }
                }
                context.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }
            return Ok(viewModel.RetrievalID);
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
            var retrievalService = new RetrievalService(context);
            var disbursementService = new DisbursementService(context);

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
    }


}
