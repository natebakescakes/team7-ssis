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
    public class DisbursementAPIController : ApiController
    {
        DisbursementService disbursementService;

        public DisbursementAPIController()
        {
            Context = new ApplicationDbContext();
            disbursementService = new DisbursementService(Context);
        }

        public ApplicationDbContext Context { get; set; }

        /// <summary>
        /// Retrieves all Disbursement Details for the Disbursement
        /// </summary>
        [Route("api/disbursement/{did}")]
        [HttpGet]
        public IEnumerable<DisbursementFormTableViewModel> FindDisbursementDetailsByDisbursement(string did)
        {
            Disbursement d = disbursementService.FindDisbursementById(did);
            List<DisbursementFormTableViewModel> viewModel = new List<DisbursementFormTableViewModel>();
            foreach (DisbursementDetail dd in d.DisbursementDetails)
            {
                viewModel.Add(new DisbursementFormTableViewModel
                {
                    ItemCode = dd.ItemCode,
                    Description = dd.Item.Description,
                    Qty = dd.ActualQuantity
                });
            }
            return viewModel;
        }

        /// <summary>
        /// Get all disbursements for mobile view
        /// </summary>
        /// <returns></returns>
        [Route("api/disbursement")]
        public IHttpActionResult GetAllDisbursements()
        {
            var disbursements = disbursementService.FindAllDisbursements();

            if (disbursements.Count == 0) return NotFound();

            return Ok(disbursements.Select(disbursement => new DisbursementMobileViewModel()
            {
                DisbursementId = disbursement.DisbursementId,
                Department = disbursement.Department != null ? disbursement.Department.Name : "",
                CollectionPoint = disbursement.Retrieval.Requisitions.Where(r => r.Department.Name == disbursement.Department.Name).FirstOrDefault().CollectionPoint.Name,
                CreatedDate = disbursement.CreatedDateTime.ToShortDateString(),
                Status = disbursement.Status != null ? disbursement.Status.Name : "",
                DisbursementDetails = disbursement.DisbursementDetails.Select(d => new DisbursementFormTableViewModel()
                {
                    ItemCode = d.ItemCode,
                    Description = d.Item.Description,
                    Qty = d.ActualQuantity,
                    Uom = d.Item.Uom,
                }).ToList()
            }));
        }

        [Route("api/disbursement/collect")]
        public IHttpActionResult ConfirmCollection([FromBody] DisbursementIdViewModel disbursementId)
        {
            try
            {
                new DisbursementService(Context).ConfirmCollection(disbursementId.DisbursementId);
            }
            catch (ArgumentException)
            {
                return BadRequest("Disbursement already collected!");
            }

            return Ok("Success");
        }
    }
}
