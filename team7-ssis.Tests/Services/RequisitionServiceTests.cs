using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Services
{
    [TestClass()]
    public class RequisitionServiceTests
    {
        ApplicationDbContext context;
        TitleRepository titleRepository;
        RequisitionService requisitionService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            titleRepository = new TitleRepository(context);
        }

        [TestMethod()]
        public void FindRequisitionsByStatusTest()
        {

        }

        [TestMethod()]
        public void GetRequisitionDetailsTest()
        {

        }

        [TestMethod()]
        public void ProcessRequisitionsTest()
        {
            // Arrange


            // Act
            // Assert
        }

        [TestMethod()]
        public void AddItemsToRequisitionTest()
        {

        }

        [TestMethod()]
        public void CreateRequisitionTest()
        {

        }

        [TestMethod()]
        public void AddItemsToRequisitionTest1()
        {

        }

        [TestMethod()]
        public void CancelRequisitionTest()
        {

        }

        [TestMethod()]
        public void ApproveRequisitionsTest()
        {

        }

        [TestMethod()]
        public void ApproveRequisitionsTest1()
        {

        }
    }
}