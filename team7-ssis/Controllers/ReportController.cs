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
        private string _url = "http://localhost:3000";
        private string _secret = "b4f77ab392e805ca1ed01b9d7148c9af7d7c9607afb785ca82d91c38fb559c13";

        // GET: Data
        public ActionResult Index()
        {
            var payload = new Dictionary<string, object>
            {
                {"resource", new { question = 2 } },
                {"params", new {} }
            };

            var urlTokenViewModel = new UrlTokenViewModel() { Url = _url, Token = Jose.JWT.Encode(payload, Encoding.ASCII.GetBytes(_secret), Jose.JwsAlgorithm.HS256) };

            return View(urlTokenViewModel);
        }
    }
}