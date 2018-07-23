using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class ReportController : Controller
    {
        private string _url = "http://13.76.131.177:3000";
        private string _secret = "f8c1e9d377bbedaa14613882e579b4653c27e9010162b11b74d61616441ef85a";

        // GET: Report/DepartmentUsage
        public ActionResult DepartmentUsage()
        {
            var quantityByDepartmentAPayload = new Dictionary<string, object>
            {
                {"resource", new { dashboard = 1 } },
                {"params", new {} }
            };

            return View("DepartmentUsage", "_LayoutFluid", new ReportViewModel()
            {
                DashboardConfig = new UrlTokenViewModel()
                {
                    Url = _url,
                    Token = Jose.JWT.Encode(quantityByDepartmentAPayload, Encoding.ASCII.GetBytes(_secret), Jose.JwsAlgorithm.HS256)
                }
            });
        }

        // GET: Report/StoreOperations
        public ActionResult StoreOperations()
        {
            var quantityByDepartmentAPayload = new Dictionary<string, object>
            {
                {"resource", new { dashboard = 3 } },
                {"params", new {} }
            };

            return View("StoreOperations", "_LayoutFluid", new ReportViewModel()
            {
                DashboardConfig = new UrlTokenViewModel()
                {
                    Url = _url,
                    Token = Jose.JWT.Encode(quantityByDepartmentAPayload, Encoding.ASCII.GetBytes(_secret), Jose.JwsAlgorithm.HS256)
                }
            });
        }
    }
}