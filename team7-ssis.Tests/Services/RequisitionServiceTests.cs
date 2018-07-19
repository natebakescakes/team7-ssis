using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;
using Microsoft.AspNet.Identity;

namespace team7_ssis.Services.Tests
{
    [TestClass()]
    public class RequisitionServiceTests
    {
        ApplicationDbContext context;
        RequisitionService requisitionService;
        string userId;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);

            //// Populate Data (if necessary)
            populateRequisitions();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            removeRequisitions();
        }

        private void removeRequisitions()
        {
            Requisition r1 = context.Requisition.Where(x => x.RequisitionId == "REQ-201807001").ToList().First();
            Requisition r2 = context.Requisition.Where(x => x.RequisitionId == "REQ-201807002").ToList().First();
            Requisition r3 = context.Requisition.Where(x => x.RequisitionId == "REQ-201807003").ToList().First();
            context.Requisition.Remove(r1);
            context.Requisition.Remove(r2);
            context.Requisition.Remove(r3);
            context.SaveChanges();
        }

        private void populateRequisitions()
        {
            //// Create Requisition Details

            RequisitionDetail rd1 = new RequisitionDetail();
            rd1.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd1.Quantity = 10;

            RequisitionDetail rd2 = new RequisitionDetail();
            rd2.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd2.Quantity = 10;

            RequisitionDetail rd3 = new RequisitionDetail();
            rd3.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd3.Quantity = 10;

            RequisitionDetail rd4 = new RequisitionDetail();
            rd4.Item = context.Item.Where(x => x.ItemCode == "C002").ToList().First();
            rd4.Quantity = 10;

            RequisitionDetail rd5 = new RequisitionDetail();
            rd5.Item = context.Item.Where(x => x.ItemCode == "C003").ToList().First();
            rd5.Quantity = 10;

            //// Create Requisitions

            Requisition r1 = new Requisition();
            r1.RequisitionId = "REQ-201807001";
            r1.Department = context.Department.Where(x => x.DepartmentCode == "COMM").ToList().First();
            r1.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r1.RequisitionDetails = new List<RequisitionDetail>();
            r1.Retrieval = null;
            r1.EmployeeRemarks = "Test by Gabriel";
            r1.HeadRemarks = "Test by Gabriel Boss";
            r1.Status = context.Status.Where(x => x.StatusId == 4).ToList().First(); ; // Pending Approval
            r1.CreatedBy = null;
            r1.CreatedDateTime = DateTime.Now;

            r1.RequisitionDetails.Add(rd1);

            requisitionService.Save(r1);

            Requisition r2 = new Requisition();
            r2.RequisitionId = "REQ-201807002";
            r2.Department = context.Department.Where(x => x.DepartmentCode == "CPSC").ToList().First();
            r2.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r2.RequisitionDetails = new List<RequisitionDetail>();
            r2.Retrieval = null;
            r2.EmployeeRemarks = "Test by Gabriel";
            r2.HeadRemarks = "Test by Gabriel Boss";
            r2.Status = context.Status.Where(x => x.StatusId == 4).ToList().First(); ; // Pending Approval
            r2.CreatedBy = null;
            r2.CreatedDateTime = DateTime.Now;

            r2.RequisitionDetails.Add(rd2);

            requisitionService.Save(r2);

            Requisition r3 = new Requisition();
            r3.RequisitionId = "REQ-201807003";
            r3.Department = context.Department.Where(x => x.DepartmentCode == "ENGL").ToList().First();
            r3.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r3.RequisitionDetails = new List<RequisitionDetail>();
            r3.Retrieval = null;
            r3.EmployeeRemarks = "Test by Gabriel";
            r3.HeadRemarks = "Test by Gabriel Boss";
            r3.Status = context.Status.Where(x => x.StatusId == 4).ToList().First(); ; // Pending Approval
            r3.CreatedBy = null;
            r3.CreatedDateTime = DateTime.Now;

            r3.RequisitionDetails.Add(rd3);
            r3.RequisitionDetails.Add(rd4);
            r3.RequisitionDetails.Add(rd5);

            requisitionService.Save(r3);
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
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807001").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807002").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807003").ToList().First());

            // Act
            string retrievalId = requisitionService.ProcessRequisitions(reqList);

            // Assert - the Retrieval ID that ProcessRequisitions returns should return a Requisition
            Retrieval result = context.Retrieval.Where(x => x.RetrievalId == retrievalId).ToList().First();
            Assert.IsNotNull(result);

            context.Retrieval.Remove(result);
            context.SaveChanges();
        }

        [TestMethod()]
        public void AddDisbursementDetailsForEachDepartmentTest()
        {
            //// Arrange
            List<Requisition> reqList = new List<Requisition>();

            // Add Requisitions to List<Requisition>
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807001").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807002").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "REQ-201807003").ToList().First());

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