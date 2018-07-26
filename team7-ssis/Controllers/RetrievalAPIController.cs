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

        public RetrievalAPIController()
        {
            context = new ApplicationDbContext();
            disbursementService = new DisbursementService(context);
        }

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
                //Bin = y.Key.Bin,
                Bin = "",
                QtyOrdered = y.Sum(dd => dd.PlanQuantity),
                Description = context.Item.Where(x => x.ItemCode == y.Key.ItemCode).First().Description
            }).ToList();

            return viewModel;
        }
        [Route("api/stationeryretrieval")]
        [HttpPost]
        public IHttpActionResult StationeryRetrieval(StationeryRetrievalTableJSONViewModel viewModel)
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
                    Needed = dd.PlanQuantity
                });
            }
            return viewModel;
        }


    }


}
