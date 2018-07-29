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
        DisbursementService disbursementService;
        RetrievalService retrievalService;

        public RetrievalAPIController()
        {
            Context = new ApplicationDbContext();
            disbursementService = new DisbursementService(Context);
            retrievalService = new RetrievalService(Context);
        }

        public ApplicationDbContext Context { get; set; }

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
                Context.SaveChanges();
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

        [Route("api/retrieval")]
        [HttpGet]
        public IEnumerable<ManageRetrievalsViewModel> GetAllRetrievals()
        {
            List<Retrieval> retList = retrievalService.FindAllRetrievals();
            List<ManageRetrievalsViewModel> viewModel = new List<ManageRetrievalsViewModel>();

            // TODO: Change this strange bug where r.Status.get is not returning anything without this line
            Status s = Context.Status.Where(x => x.StatusId == 17).First();

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
