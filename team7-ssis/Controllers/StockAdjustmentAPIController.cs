using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Tests.Services;
using team7_ssis.ViewModels;
using team7_ssis.Repositories;
using Microsoft.AspNet.Identity;
using team7_ssis.Services;

namespace team7_ssis.Controllers
{
    
    public class StockAdjustmentAPIController : ApiController
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        StockAdjustmentService stockAdjustmentService = new StockAdjustmentService(context);
        StockAdjustmentRepository stockAdjustmentRepository = new StockAdjustmentRepository(context);
        UserService userService = new UserService(context);
        UserRepository userRepository = new UserRepository(context);



        [Route("api/stockadjustment/all")]
        [HttpGet]
        public IEnumerable<StockAdjustmentViewModel> stockadjustments()
        {
            List<StockAdjustment> list = stockAdjustmentService.FindAllStockAdjustment();
            List<StockAdjustmentViewModel > sadj= new List<StockAdjustmentViewModel>();


            foreach (StockAdjustment s in list)
            {
                sadj.Add(new StockAdjustmentViewModel
                {
                    StockAdjustmentId = s.StockAdjustmentId,
                    CreatedBy = (s.CreatedBy == null) ? "" : s.CreatedBy.FirstName,
                    ApprovedBySupervisor = (s.ApprovedBySupervisor == null) ? "" : s.ApprovedBySupervisor.FirstName,
                    CreatedDateTime = s.CreatedDateTime,
                    StatusName = (s.Status == null) ? "" : s.Status.Name,
                    Link = "/StockAdjustment/"+s.StockAdjustmentId
                });

            }

            return sadj;
        }

        [Route("api/manageitem/selectitems")]
        [HttpGet]
        public IEnumerable<Item> SelectedItems()
        {
            List<Item> list = new List<Item>();


            return null;
        }

        [Route("api/supervisor/all")]
        [HttpGet]
        public IEnumerable<SupervisorViewModel> AllSupervisors()
        {
            //String user_id = System.Web.HttpContext.Current.User.Identity.GetUserId();
            //ApplicationUser user = userRepository.FindById(user_id);
            ApplicationUser user = userRepository.FindByEmail("StoreClerk1@email.com");
            List<ApplicationUser> supervisors = userService.FindSupervisorsByDepartment(user.Department);
            List<SupervisorViewModel> sv = new List<SupervisorViewModel>();
            foreach (ApplicationUser a in supervisors)
            {
                sv.Add(new SupervisorViewModel
                { Name = a.FirstName + " " + a.LastName });

            }
            return sv;
        }



        [Route("api/manager/all")]
        [HttpGet]
        public IEnumerable<ApplicationUser> AllManagers()
        {
            return null;
        }

    }
}
