using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services.Tests
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
        public void FindRetrievalByIdTest()
        {

        }

        [TestMethod()]
        public void UpdateActualQuantitiesTest()
        {

        }

        [TestMethod()]
        public void ContinueToDisbursementTest()
        {

        }

        [TestMethod()]
        public void FindDisbursementsByRetrievalIdTest()
        {

        }

        [TestMethod()]
        public void ViewDisbursementDetailsTest()
        {

        }

        [TestMethod()]
        public void GetDisbursementByIdTest()
        {

        }

        [TestMethod()]
        public void ConfirmDeliveryTest()
        {

        }

        [TestMethod()]
        public void CreateNewRequisitionTest()
        {

        }

        [TestMethod()]
        public void AddItemPopUpTest()
        {

        }

        [TestMethod()]
        public void AddToRequisitionsTest()
        {

        }

        [TestMethod()]
        public void CreateNewRequisitionTest1()
        {

        }

        [TestMethod()]
        public void ShowRequisitionTest()
        {

        }

        [TestMethod()]
        public void EditRequisitionTest()
        {

        }

        [TestMethod()]
        public void AddItemTest()
        {

        }

        [TestMethod()]
        public void CancelRequisitionTest()
        {

        }

        [TestMethod()]
        public void ApproveTest()
        {

        }

        [TestMethod()]
        public void ViewRequisitionDetailsTest()
        {

        }

        [TestMethod()]
        public void ApproveAllTest()
        {

        }
    }
}