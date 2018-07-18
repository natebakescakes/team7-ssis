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
            List<Requisition> reqList = new List<Requisition>();

            // Act
            string id = requisitionService.ProcessRequisitions(reqList);

            // Assert - the Requisition ID that ProcessRequisitions returns should return a Requisition
            Retrieval result = context.Retrieval.Where(x => x.RetrievalId == id).ToList().First();
            Assert.IsNotNull(result);

            context.Retrieval.Remove(result);
            context.SaveChanges();
        }

        public void AddDisbursementDetailsForEachDisbursementTest()
        {
            // Arrange


            // Act
            
            
            // Assert - RequisitionDetail should go in the correct Disbursement.DisbursementDetail
        }

        [TestMethod()]
        public void CreateDisbursementForEachDepartmentTest()
        {
            //// Arrange

            // get Departments
            Department d1 = context.Department.ToList()[0];
            Department d2 = context.Department.ToList()[1];

            // Create expected result
            HashSet<Department> expected = new HashSet<Department> { d1, d2 };

            // Create mock Requisition list
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

            //// Act
            List<Disbursement> result = requisitionService.CreateDisbursementForEachDepartment(reqList);

            //// Assert
            HashSet<Department> depts = new HashSet<Department>(result.Select(x => x.Department));

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