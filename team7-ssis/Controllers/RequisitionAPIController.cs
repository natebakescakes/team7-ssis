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
        static ApplicationDbContext context = new ApplicationDbContext();
        RequisitionService requisitionService = new RequisitionService(context);
        RequisitionRepository requisitionRepository = new RequisitionRepository(context); 

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
            string message;
            foreach (string s in reqIdList)
            {
                reqList.Add(requisitionRepository.FindById(s));
            }
            try
            {
                message = requisitionService.ProcessRequisitions(reqList);
            } catch
            {
                message = "Please select Requisitions to be processed.";
            }
            return Ok( new { message = message });
        }
    }
}
