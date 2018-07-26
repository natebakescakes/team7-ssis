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
        ApplicationDbContext context;
        DisbursementService disbursementService;

        public DisbursementAPIController()
        {
            context = new ApplicationDbContext();
            disbursementService = new DisbursementService(context);
        }
        
        /// <summary>
        /// Retrieves all Disbursement Details for the Disbursement
        /// </summary>
        [Route("api/disbursement/{did}")]
        [HttpGet]
        public IEnumerable<DisbursementFormTableViewModel> FindDisbursementDetailsByDisbursement (string did)
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
    }
}
