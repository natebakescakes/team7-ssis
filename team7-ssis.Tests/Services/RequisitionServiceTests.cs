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

        [TestMethod()]
        public void AddDisbursementDetailsForEachDepartmentTest()
        {
            //// Arrange
            List<Requisition> reqList = new List<Requisition>();

            // Create 5 Requisition Details
            RequisitionDetail rd1 = new RequisitionDetail();
            RequisitionDetail rd2 = new RequisitionDetail();
            RequisitionDetail rd3 = new RequisitionDetail();
            RequisitionDetail rd4 = new RequisitionDetail();
            RequisitionDetail rd5 = new RequisitionDetail();
            rd1.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd2.Item = context.Item.Where(x => x.ItemCode == "C002").ToList().First();
            rd3.Item = context.Item.Where(x => x.ItemCode == "C003").ToList().First();
            rd4.Item = context.Item.Where(x => x.ItemCode == "C004").ToList().First();
            rd5.Item = context.Item.Where(x => x.ItemCode == "C005").ToList().First();
            
            // Create 3 Requisitions
            Requisition r1 = new Requisition(); // Commerce Dept ordered 1 item
            r1.Department = context.Department.Where(x => x.DepartmentCode == "COMM").ToList().First();
            r1.RequisitionDetails = new List<RequisitionDetail>();
            r1.RequisitionDetails.Add(rd1);
            r1.CreatedDateTime = DateTime.Now;

            Requisition r2 = new Requisition(); // Computer Science ordered 2 items
            r2.Department = context.Department.Where(x => x.DepartmentCode == "CPSC").ToList().First();
            r2.RequisitionDetails = new List<RequisitionDetail>();
            r2.RequisitionDetails.Add(rd2);
            r2.RequisitionDetails.Add(rd3);
            r2.CreatedDateTime = DateTime.Now;
            Requisition r3 = new Requisition(); // English Dept ordered 2 items
            r3.Department = context.Department.Where(x => x.DepartmentCode == "ENGL").ToList().First();
            r3.RequisitionDetails = new List<RequisitionDetail>();
            r3.RequisitionDetails.Add(rd4);
            r3.RequisitionDetails.Add(rd5);
            r3.CreatedDateTime = DateTime.Now;

            // Add Requisitions to List<Requisition>
            reqList.Add(r1);
            reqList.Add(r2);
            reqList.Add(r3);

            // Act
            List<Disbursement> disbList = requisitionService.CreateDisbursementForEachDepartment(reqList);

            //// Assert

            // There should be 3 Disbursements created, one for each department
            Assert.AreEqual(disbList.Count, 3);
            Assert.IsTrue(new HashSet<string>(disbList.Select(x => x.Department.DepartmentCode).ToList())
                            .SetEquals(new HashSet<string> { "COMM", "CPSC", "ENGL" }));

            // TODO: Write a case which checks that the correct items are in DisbursementDetails 

            // TODO: Write a case which tests same department, multiple requisitions

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