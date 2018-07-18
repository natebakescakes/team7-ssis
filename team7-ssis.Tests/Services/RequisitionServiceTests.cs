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
        RequisitionService requisitionService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
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
            //// Arrange
            //string expected = "A123";

            ////Act
            //// create list of requests
            //List<Requisition> reqList = new List<Requisition>();
            //var result = requisitionService.ProcessRequisitions(reqList);


            ////Assert
            //Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void createDisbursementsByDepartmentTest()
        {
            // Arrange
            HashSet<string> expected = new HashSet<string> { "a", "b" };

            Department d1 = new Department();
            d1.Name = "a";
            Department d2 = new Department();
            d2.Name = "b";

            List<Requisition> reqList = new List<Requisition>();
            Requisition r1 = new Requisition();
            r1.Department = d1;
            Requisition r2 = new Requisition();
            r2.Department = d2;
            Requisition r3 = new Requisition();
            r3.Department = d1;
            reqList.Add(r1);
            reqList.Add(r2);
            reqList.Add(r3);

            // Act
            List<Disbursement> disbursementList = requisitionService.CreateDisbursementsByDepartment(reqList);

            // Assert
            HashSet<string> depts = new HashSet<string>(reqList.Select(x => x.Department.Name).Distinct());

            Assert.IsTrue(depts.SetEquals(expected));
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