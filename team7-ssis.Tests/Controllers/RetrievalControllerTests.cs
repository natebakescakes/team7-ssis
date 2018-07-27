using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using team7_ssis.Controllers;
using team7_ssis.Models;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class RetrievalControllerTests
    {
        ApplicationDbContext context;
        RetrievalController retrievalController;

        public RetrievalControllerTests()
        {
            context = new ApplicationDbContext();
            retrievalController = new RetrievalController();
        }

        /// <summary>
        /// Tests thhat RetrievalDetails view renders when a valid Retrieval ID AND Item Code is passed in
        /// </summary>
        [TestMethod]
        public void RetrievalDetailsTest()
        {
            // ARRANGE

            // ACT
            ActionResult result = retrievalController.RetrievalDetails("TEST2", "E032");

            // ASSERT
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
        }
    }
    
}
