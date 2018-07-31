﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Repositories;
using Microsoft.AspNet.Identity;

namespace team7_ssis.Tests.Services
{
    [TestClass()]
    public class RequisitionServiceTests
    {
        static ApplicationDbContext context;
        static RequisitionService requisitionService;
        static StatusService statusService;
        static RequisitionRepository requisitionRepository;
        ItemRepository itemRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            requisitionService = new RequisitionService(context);
            statusService = new StatusService(context);
            requisitionRepository = new RequisitionRepository(context);
            itemRepository = new ItemRepository(context);

            //// Populate Data (if necessary)
            populateRequisitions();
        }

        [TestMethod]
        public void FindByDepartmentTest()
        {
            // Arrange
            requisitionService.Save(new Requisition()
            {
                RequisitionId = "RQSERVTEST",
                Department = new DepartmentRepository(context).FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            var result = requisitionService.FindRequisitionsByDepartment(new DepartmentRepository(context).FindById("ENGL"));

            // Assert
            result.ToList().ForEach(r => Assert.AreEqual("ENGL", r.Department.DepartmentCode));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (requisitionRepository.ExistsById("TEST1"))
                requisitionRepository.Delete(requisitionRepository.FindById("TEST1"));
            if (requisitionRepository.ExistsById("GAB1"))
                requisitionRepository.Delete(requisitionRepository.FindById("GAB1"));
            if (requisitionRepository.ExistsById("GAB2"))
                requisitionRepository.Delete(requisitionRepository.FindById("GAB2"));
            if (requisitionRepository.ExistsById("GAB3"))
                requisitionRepository.Delete(requisitionRepository.FindById("GAB3"));
            if (requisitionRepository.ExistsById("RQSERVTEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("RQSERVTEST"));
            if (requisitionRepository.ExistsById("APPROVETEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("APPROVETEST"));
            if (requisitionRepository.ExistsById("REQ-201807-004"))
                requisitionRepository.Delete(requisitionRepository.FindById("REQ-201807-004"));
            if (requisitionRepository.ExistsById("REQ-201807-005"))
                requisitionRepository.Delete(requisitionRepository.FindById("REQ-201807-005"));



        }

        [TestMethod]
        public void FindRequisitionsByStatusTest()
        {
            // Arrange
            List<Status> statusList = new List<Status>();
            statusList.Add(context.Status.Where(x => x.StatusId == 8).First());
            statusList.Add(context.Status.Where(x => x.StatusId == 9).First());
            statusList.Add(context.Status.Where(x => x.StatusId == 10).First());

            // Act
            List<Requisition> result = requisitionService.FindRequisitionsByStatus(statusList);

            // Assert - All Requisitions in the list should have a Status which is in the statusList
            foreach (Requisition req in result)
            {
                Assert.IsTrue(statusList.Contains(req.Status));
            }
        }
        [TestMethod]
        public void GetRequisitionDetailsTest()
        {
            // Arrange
            string reqId = "GAB1";

            // Act
            List<RequisitionDetail> reqList = requisitionService.GetRequisitionDetails(reqId);

            // Assert - Should return a List<Requistion> where Requisition.RequisitionId equals the one passed in
            foreach (RequisitionDetail rd in reqList)
            {
                Assert.AreEqual(rd.RequisitionId, reqId);
            }
        }

        [TestMethod()]
        public void ProcessRequisitionsTest_CreatesRetrieval()
        {
            // Arrange
            List<Requisition> reqList = new List<Requisition>();
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB1").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB2").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB3").ToList().First());

            // Act
            string retrievalId = requisitionService.ProcessRequisitions(reqList);

            // Assert - the Retrieval ID that ProcessRequisitions returns should return a Requisition
            Retrieval result = context.Retrieval.Where(x => x.RetrievalId == retrievalId).ToList().First();
            Assert.IsNotNull(result);

            // Cleanup
            var disb = context.Disbursement.Where(x => x.Retrieval.RetrievalId == result.RetrievalId).AsEnumerable();
            context.Disbursement.RemoveRange(disb);

            context.Retrieval.Remove(result);
            context.SaveChanges();
        }

        [TestMethod]
        public void ProcessRequisitionsTest_AddQuantity()
        {
            // ARRANGE
            List<Requisition> reqList = new List<Requisition>();
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB1").ToList().First()); // department is COMM

            Requisition r1 = new Requisition();
            r1.RequisitionId = "TEST1";
            r1.Department = context.Department.Where(x => x.DepartmentCode == "COMM").ToList().First();
            r1.RequisitionDetails = new List<RequisitionDetail>();
            r1.EmployeeRemarks = "From AddDisbursementDetailsFromEachDepartmentTest_AddQuantity";
            r1.CreatedDateTime = DateTime.Now;

            RequisitionDetail rd1 = new RequisitionDetail();
            rd1.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd1.ItemCode = "C001";
            rd1.Quantity = 20;

            r1.RequisitionDetails.Add(rd1);

            reqList.Add(r1);

            //// ACT
            string retrievalId = requisitionService.ProcessRequisitions(reqList);

            //// ASSERT: RequisitionDetail & Department should result in a single DisbursementDetail with 30 items if C001 has enough inventory
            
            var query = context.Disbursement.Where(x => x.Retrieval.RetrievalId == retrievalId);
            var inventoryLevel = new ItemService(context).FindInventoryByItemCode("C001").Quantity;

            if (inventoryLevel >= 30)
            {
                Assert.IsTrue(query.First().DisbursementDetails.Count() == 1); // single DisbursementDetail for 1 Department?
                var dd = query.First().DisbursementDetails.First();
                Assert.IsTrue(dd.ItemCode == "C001"); // DisbursementDetail's ItemCode is "C001"
                Assert.IsTrue(dd.PlanQuantity == 30); // DisbursementDetail's PlanQuantity is 30

                // remove Disbursement which has the generated DisbursementId
                // will remove DisbursementDetails as well
                var d = context.Disbursement.Where(x => x.DisbursementId == dd.DisbursementId);
                context.Disbursement.Remove(d.First());
            }
            else if (inventoryLevel > 0)
            {
                Assert.IsTrue(query.First().DisbursementDetails.Count() == 1); // single DisbursementDetail for 1 Department?
                var dd = query.First().DisbursementDetails.First();
                Assert.IsTrue(dd.ItemCode == "C001"); // DisbursementDetail's ItemCode is "C001"
                Assert.IsTrue(dd.PlanQuantity > 0); // DisbursementDetail's PlanQuantity is 30

                // remove Disbursement which has the generated DisbursementId
                // will remove DisbursementDetails as well
                var d = context.Disbursement.Where(x => x.DisbursementId == dd.DisbursementId);
                context.Disbursement.Remove(d.First());
            }
            else
            {
                Assert.IsTrue(context.Disbursement.Where(x => x.Retrieval.RetrievalId == retrievalId).Count() == 0);
            }

            //// CLEANUP
            // remove the Retrieval with the generated retrievalId
            context.Retrieval.Remove(context.Retrieval.Where(x => x.RetrievalId == retrievalId).First());
            context.SaveChanges();
        }

        [TestMethod()]
        public void AddDisbursementDetailsForEachDepartmentTest_CorrectDepts()
        {
            // Arrange
            List<Requisition> reqList = new List<Requisition>();

            // Add Requisitions to List<Requisition>
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB1").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB2").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB3").ToList().First());

            // Act
            List<Disbursement> disbList = requisitionService.CreateDisbursementForEachDepartment(reqList);

            //// Assert - There should be 3 Disbursements created, one for each department
            Assert.AreEqual(disbList.Count, 3);
            Assert.IsTrue(new HashSet<string>(disbList.Select(x => x.Department.DepartmentCode).ToList())
                            .SetEquals(new HashSet<string> { "COMM", "CPSC", "ENGL" }));
        }

        [TestMethod()]
        public void CreateDisbursementForEachDepartmentTest()
        {
            // Arrange

            List<Requisition> reqList = new List<Requisition>();
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB1").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB2").ToList().First());
            reqList.Add(context.Requisition.Where(x => x.RequisitionId == "GAB3").ToList().First());

            //// Act
            HashSet<Department> expected = new HashSet<Department>
            {
                context.Department.Where(x => x.DepartmentCode == "COMM").First(),
                context.Department.Where(x => x.DepartmentCode == "CPSC").First(),
                context.Department.Where(x => x.DepartmentCode == "ENGL").First()
            };

            List<Disbursement> result = requisitionService.CreateDisbursementForEachDepartment(reqList);

            //// Assert
            HashSet<Department> depts = new HashSet<Department>(result.Select(x => x.Department));

            Assert.IsTrue(depts.SetEquals(expected));
        }

        [TestMethod]
        public void ApproveRequisition_Valid()
        {
            // Arrange
            requisitionService.Save(new Requisition()
            {
                RequisitionId = "APPROVETEST",
                Status = statusService.FindStatusByStatusId(4),
                CreatedDateTime = DateTime.Now,
            });
            var expected = statusService.FindStatusByStatusId(6);

            // Act
            requisitionService.ApproveRequisition("APPROVETEST", "root@admin.com", "I APPROVE THIS");

            // Assert
            Assert.AreEqual(expected.StatusId, requisitionRepository.FindById("APPROVETEST").Status.StatusId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ApproveRequisition_AlreadyApproved_ThrowsException()
        {
            // Arrange
            requisitionService.Save(new Requisition()
            {
                RequisitionId = "APPROVETEST",
                Status = statusService.FindStatusByStatusId(6),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            requisitionService.ApproveRequisition("APPROVETEST", "root@admin.com", "I APPROVE THIS");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ApproveRequisition_Invalid_ThrowException()
        {
            // Arrange

            // Act
            requisitionService.ApproveRequisition("APPROVETEST", "root@admin.com", "I APPROVE THIS");
        }

        [TestMethod]
        public void RejectRequisition_Valid()
        {
            // Arrange
            requisitionService.Save(new Requisition()
            {
                RequisitionId = "APPROVETEST",
                Status = statusService.FindStatusByStatusId(4),
                CreatedDateTime = DateTime.Now,
            });
            var expected = statusService.FindStatusByStatusId(5);

            // Act
            requisitionService.RejectRequisition("APPROVETEST", "root@admin.com", "I REJECT THIS");

            // Assert
            Assert.AreEqual(expected.StatusId, requisitionRepository.FindById("APPROVETEST").Status.StatusId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RejectRequisition_AlreadyRejected_ThrowsException()
        {
            // Arrange
            requisitionService.Save(new Requisition()
            {
                RequisitionId = "APPROVETEST",
                Status = statusService.FindStatusByStatusId(6),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            requisitionService.RejectRequisition("APPROVETEST", "root@admin.com", "I REJECT THIS");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RejectRequisition_Invalid_ThrowException()
        {
            // Arrange

            // Act
            requisitionService.RejectRequisition("APPROVETEST", "root@admin.com", "I REJECT THIS");
        }

        [TestMethod]
        public void UpdateRequisitionStatus_Test()
        {
            // ARRANGE
            Requisition r = requisitionRepository.FindById("GAB1");
            ;
            // ACT
            // Update StatusId to 6 ("Approved")
            requisitionService.UpdateRequisitionStatus("GAB1", 6 , "");

            // ASSERT
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Assert.AreEqual(r.Status.StatusId, 6);
            }
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
            r1.RequisitionId = "GAB1";
            r1.Department = context.Department.Where(x => x.DepartmentCode == "COMM").ToList().First();
            r1.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r1.RequisitionDetails = new List<RequisitionDetail>();
            r1.Retrieval = null;
            r1.EmployeeRemarks = "Test by Gabriel";
            r1.HeadRemarks = "Test by Gabriel Boss";
            r1.Status = context.Status.Where(x => x.StatusId == 8).ToList().First(); ; // Pending Approval
            r1.CreatedBy = null;
            r1.CreatedDateTime = DateTime.Now;

            r1.RequisitionDetails.Add(rd1);

            requisitionService.Save(r1);

            Requisition r2 = new Requisition();
            r2.RequisitionId = "GAB2";
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
            r3.RequisitionId = "GAB3";
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


        [TestMethod]
        public void FindUnfulfilledQuantityRequestedTest()
        {
            //arrange
            Requisition r1 = new Requisition();
            r1.RequisitionId = "REQ-201807-004";
            r1.Department = context.Department.Where(x => x.DepartmentCode == "COMM").ToList().First();
            r1.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r1.RequisitionDetails = new List<RequisitionDetail>();
            r1.Retrieval = null;
            r1.EmployeeRemarks = "Test by Gabriel";
            r1.HeadRemarks = "Test by Gabriel Boss";
            r1.Status = context.Status.Where(x => x.StatusId == 6).ToList().First(); ; // Approved
            r1.CreatedBy = null;
            r1.CreatedDateTime = DateTime.Now;

            RequisitionDetail rd1 = new RequisitionDetail();
            rd1.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd1.Quantity = 10;

            r1.RequisitionDetails.Add(rd1);
            requisitionService.Save(r1);

            Requisition r2 = new Requisition();
            r2.RequisitionId = "REQ-201807-005";
            r2.Department = context.Department.Where(x => x.DepartmentCode == "CPSC").ToList().First();
            r2.CollectionPoint = context.CollectionPoint.Where(x => x.CollectionPointId == 1).ToList().First();
            r2.RequisitionDetails = new List<RequisitionDetail>();
            r2.Retrieval = null;
            r2.EmployeeRemarks = "Test by Gabriel";
            r2.HeadRemarks = "Test by Gabriel Boss";
            r2.Status = context.Status.Where(x => x.StatusId == 7).ToList().First(); ; // Req Processed
            r2.CreatedBy = null;
            r2.CreatedDateTime = DateTime.Now;

            RequisitionDetail rd2 = new RequisitionDetail();
            rd2.Item = context.Item.Where(x => x.ItemCode == "C001").ToList().First();
            rd2.Quantity = 15;

            r2.RequisitionDetails.Add(rd2);
            requisitionService.Save(r2);

            //Act
            var result = requisitionService.FindUnfulfilledQuantityRequested(itemRepository.FindById("C001"));

            //Assert
            Assert.AreEqual(result, 25);
        }
    }
}