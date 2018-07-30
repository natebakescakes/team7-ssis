using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;

namespace team7_ssis.Tests.Controllers
{
    [TestClass]
    public class StockAdjustmentControllerTest
    {

        ApplicationDbContext context;




        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();


        }




        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
