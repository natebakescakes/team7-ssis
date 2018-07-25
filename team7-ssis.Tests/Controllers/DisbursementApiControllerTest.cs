using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Controllers;
using team7_ssis.Models;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class DisbursementApiControllerTest
    {
        private ApplicationDbContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
        }

        [Ignore]
        [TestMethod]
        public void GetAllDisbursements_ContainsResult()
        {
            // Arrange
            var expectedId = "TEST";
            var expectedQuantity = 1;
            var controller = new DisbursementAPIController()
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration(),
            };

            // Act
            IHttpActionResult actionResult = controller.GetAllDisbursements();
            var contentResult = actionResult as OkNegotiatedContentResult<IEnumerable<DisbursementMobileViewModel>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.IsTrue(contentResult.Content.Select(d => d.DisbursementId).Contains(expectedId));
            Assert.IsTrue(contentResult.Content.Select(d => d.DisbursementDetails.Select(dd => dd.Qty)).FirstOrDefault().Contains(expectedQuantity));
        }
    }
}
