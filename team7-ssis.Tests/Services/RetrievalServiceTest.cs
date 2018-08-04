using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Tests.Services
{

    [TestClass]
    public class RetrievalServiceTest
    {
        ApplicationDbContext context;
        RetrievalService retrievalService;
        ItemService itemService;
        DisbursementService disbursementService;

        RetrievalRepository retrievalRepository;
        DisbursementRepository disbursementRepository;
        DisbursementDetailRepository disbursementdetailRepository;
        StockMovementRepository stockmovementRepository;
        StatusRepository statusRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();

            retrievalService = new RetrievalService(context);
            itemService = new ItemService(context);
            disbursementService = new DisbursementService(context);

            retrievalRepository = new RetrievalRepository(context);
            disbursementRepository = new DisbursementRepository(context);
            disbursementdetailRepository = new DisbursementDetailRepository(context);
            stockmovementRepository = new StockMovementRepository(context);
            statusRepository = new StatusRepository(context);

            Retrieval retrieval = new Retrieval();
            if (retrievalRepository.FindById("TEST") == null)
            {
                //save retrieval object into db

                retrieval.RetrievalId = "TEST";
                retrieval.CreatedDateTime = DateTime.Now;
                retrievalRepository.Save(retrieval);
                retrieval.Status = statusRepository.FindById(19);
            }
            else retrieval = retrievalRepository.FindById("TEST");

            Disbursement disbursement = new Disbursement();
            if (disbursementRepository.FindById("TEST") == null)
            {
                disbursement.DisbursementId = "TEST";
                disbursement.CreatedDateTime = DateTime.Now;
                disbursement.Retrieval = retrieval;

            }
            else disbursement = disbursementRepository.FindById("TEST");

            disbursementRepository.Save(disbursement);

            //save disbursement detail object into db
            DisbursementDetail detail = new DisbursementDetail()
            {
                Disbursement = disbursement,
                Item = context.Item.Where(x => x.ItemCode == "C003").First(),
                PlanQuantity = 5,
                ActualQuantity = 5

            };
            disbursementdetailRepository.Save(detail);
        }

        [TestMethod]
        public void FindAllRetrievalsTest()
        {
            //Arrange
            int expected = context.Retrieval.ToList().Count;

            //Act
            var result = retrievalService.FindAllRetrievals();

            //Assert
            Assert.AreEqual(expected, result.Count);

        }

        [TestMethod]
        public void FindRetrievalByIdTest()
        {
            //Arrange
            string expected = "TEST";

            //Act
            var result = retrievalService.FindRetrievalById(expected);

            //Assert
            Assert.AreEqual(expected, result.RetrievalId);

        }

        [TestMethod]
        public void SaveTest()
        {
            //Arrange
            Retrieval newRetrieval = new Retrieval();
            newRetrieval.RetrievalId = IdService.GetNewRetrievalId(context);
            newRetrieval.CreatedDateTime = DateTime.Now;
            string expected = newRetrieval.RetrievalId;

            //Act
            var result = retrievalService.Save(newRetrieval);
            retrievalRepository.Delete(newRetrieval);

            //Assert
            Assert.AreEqual(expected, result.RetrievalId);



        }

        [TestMethod]
        public void SaveEditTest()
        {
            //Arrange
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();
            Status status = context.Status.Where(x => x.StatusId == 1).First();
            retrieval.Status = status;

            //Act
            var result = retrievalService.Save(retrieval);

            //Assert
            Assert.AreEqual(status.Name, result.Status.Name);
        }

        [TestMethod]
        public void RetrieveItemsTest()
        {
            //Arrange
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();
            //quantity of the first disbursement detail in the first disbursement in retrieval
            int before = retrieval.Disbursements.First().DisbursementDetails.First().Item.Inventory.Quantity;
            int quantity = retrieval.Disbursements.First().DisbursementDetails.First().ActualQuantity;
            //Act
            var result = retrievalService.RetrieveItems(retrieval);

            //Assert
            //compare item quantity
            int after = retrieval.Disbursements.First().DisbursementDetails.First().Item.Inventory.Quantity;
            Assert.AreEqual(quantity, before - after);
            //find the stockmovement
            StockMovement sm = context.StockMovement.Where(x => x.DisbursementId == "TEST").First();
            Assert.IsNotNull(sm);

        }

        [TestMethod]
        public void RetrieveItem_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        ItemCode = "E030",
                        ActualQuantity = 1,
                        Status = new StatusService(context).FindStatusByStatusId(17),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });
            var expected = new StatusService(context).FindStatusByStatusId(18);

            // Act
            new RetrievalService(context).RetrieveItem(retrieval.RetrievalId, "StoreClerk1@email.com", "E030");

            // Assert
            Assert.AreEqual(expected.StatusId, new DisbursementDetailRepository(context).FindById(disbursement.DisbursementId, "E030").Status.StatusId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RetrieveItem_DoesNotExist_ThrowsException()
        {
            // Arrange

            // Act
            new RetrievalService(context).RetrieveItem("RSERVICETEST", "StoreClerk1@email.com", "E030");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void RetrieveItem_AlreadyRetrieved_ThrowsException()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        ActualQuantity = 1,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            new RetrievalService(context).RetrieveItem(retrieval.RetrievalId, "StoreClerk1@email.com", "E030");

            // Assert
        }

        [TestMethod]
        public void ConfirmRetrieval_NoOutstanding_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Quantity = 20,
                        Status = statusRepository.FindById(7),
                    }
                }
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(19),
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 20,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var expectedStatus = new StatusService(context).FindStatusByStatusId(20);
            var expectedRemarks = "Automatically generated by system based on outstanding quantity - RSERVICETEST";

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(expectedStatus.StatusId, new RetrievalService(context).FindRetrievalById("RSERVICETEST").Status.StatusId);
            Assert.IsFalse(new RequisitionService(context).FindAllRequisitions().Select(r => r.EmployeeRemarks).Contains(expectedRemarks));
        }

        [TestMethod]
        public void ConfirmRetrieval_Outstanding_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Quantity = 30,
                        Status = statusRepository.FindById(7),
                    }
                }
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(19),
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 30,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var expectedStatus = new StatusService(context).FindStatusByStatusId(20);
            var expectedRemarks = "Automatically generated by system based on outstanding quantity - RSERVICETEST";

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(expectedStatus.StatusId, new RetrievalService(context).FindRetrievalById("RSERVICETEST").Status.StatusId);
            Assert.IsTrue(new RequisitionService(context).FindAllRequisitions().Select(r => r.EmployeeRemarks).Contains(expectedRemarks));
            var result = new RequisitionService(context).FindAllRequisitions().Where(r => r.EmployeeRemarks == expectedRemarks).FirstOrDefault();
            Assert.AreEqual(10, result.RequisitionDetails.Sum(rr => rr.Quantity));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfirmRetrieval_DoesNotExist_ThrowsException()
        {
            // Arrange

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfirmRetrieval_AlreadyRetrieved_ThrowsException()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Quantity = 30,
                    }
                }
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(20),
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 30,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var expectedStatus = new StatusService(context).FindStatusByStatusId(20);

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");
            
            // Assert
        }

        [TestMethod]
        public void ConfirmRetrieval_MoreThanOneRequisitionSameDeptOutstanding_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition1 = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST1",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST1",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Quantity = 30,
                        Status = statusRepository.FindById(7),
                    }
                }
            });
            var requisition2 = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST2",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST2",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Quantity = 30,
                        Status = statusRepository.FindById(7),
                    }
                }
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition1, requisition2 },
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(19),
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 60,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var expectedStatus = new StatusService(context).FindStatusByStatusId(20);
            var expectedRemarks = "Automatically generated by system based on outstanding quantity - RSERVICETEST";

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(expectedStatus.StatusId, new RetrievalService(context).FindRetrievalById("RSERVICETEST").Status.StatusId);
            Assert.IsTrue(new RequisitionService(context).FindAllRequisitions().Select(r => r.EmployeeRemarks).Contains(expectedRemarks));
            var result = new RequisitionService(context).FindAllRequisitions().Where(r => r.EmployeeRemarks == expectedRemarks).FirstOrDefault();
            Assert.AreEqual(40, result.RequisitionDetails.Sum(rr => rr.Quantity));
        }

        [TestMethod]
        public void ConfirmRetrieval_MoreThanOneRequisitionDiffDeptOutstanding_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition1 = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST1",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST1",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Status = statusRepository.FindById(7),
                        Quantity = 30,
                    }
                }
            });
            var requisition2 = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST2",
                CollectionPoint = departmentRepository.FindById("COMM").CollectionPoint,
                Department = departmentRepository.FindById("COMM"),
                CreatedDateTime = DateTime.Now,
                Status = statusRepository.FindById(7),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "RSERVICETEST2",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        Status = statusRepository.FindById(7),
                        Quantity = 30,
                    }
                }
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition1, requisition2 },
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(19),
            });
            var disbursement1 = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST1",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST1",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 30,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    },
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var disbursement2 = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST2",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST2",
                        ItemCode = "E030",
                        Item = new ItemService(context).FindItemByItemCode("E030"),
                        PlanQuantity = 30,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    },
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(7),
                CreatedDateTime = DateTime.Now,
            });
            var expectedStatus = new StatusService(context).FindStatusByStatusId(20);
            var expectedRemarks = "Automatically generated by system based on outstanding quantity - RSERVICETEST";

            // Act
            new RetrievalService(context).ConfirmRetrieval("RSERVICETEST", "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(expectedStatus.StatusId, new RetrievalService(context).FindRetrievalById("RSERVICETEST").Status.StatusId);
            Assert.IsTrue(new RequisitionService(context).FindAllRequisitions().Select(r => r.EmployeeRemarks).Contains(expectedRemarks));
            var result = new RequisitionService(context).FindAllRequisitions().Where(r => r.EmployeeRemarks == expectedRemarks).FirstOrDefault();
            Assert.AreEqual(10, result.RequisitionDetails.Sum(rr => rr.Quantity));
        }

        [TestMethod]
        public void UpdateActualQuantity_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "RSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "RSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "RSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "RSERVICETEST",
                        ItemCode = "E030",
                        PlanQuantity = 20,
                        ActualQuantity = 20,
                        Status = new StatusService(context).FindStatusByStatusId(17),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            new RetrievalService(context).UpdateActualQuantity("RSERVICETEST", "StoreClerk1@email.com", "E030", new List<BreakdownByDepartment>()
            {
                new BreakdownByDepartment()
                {
                    DeptId = "ENGL",
                    Actual = 10,
                }
            });

            // Assert
            Assert.AreEqual(10, new DisbursementDetailRepository(context).FindById("RSERVICETEST", "E030").ActualQuantity);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();

            List<StockMovement> smlist = context.StockMovement.Where(x => x.DisbursementId == "TEST").ToList();
            foreach (StockMovement sm in smlist)
            {
                stockmovementRepository.Delete(sm);

            }


            List<Disbursement> list = disbursementService.FindDisbursementsByRetrievalId(retrieval.RetrievalId);
            foreach (Disbursement d in list)
            {
                //Delete dummy disbursement test objects
                disbursementRepository.Delete(d);
            }

            //delete retrieval objects
            retrievalRepository.Delete(retrieval);

            var requisitionRepository = new RequisitionRepository(context);

            if (disbursementRepository.ExistsById("RSERVICETEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("RSERVICETEST"));
            if (disbursementRepository.ExistsById("RSERVICETEST1"))
                disbursementRepository.Delete(disbursementRepository.FindById("RSERVICETEST1"));
            if (disbursementRepository.ExistsById("RSERVICETEST2"))
                disbursementRepository.Delete(disbursementRepository.FindById("RSERVICETEST2"));
            if (requisitionRepository.ExistsById("RSERVICETEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("RSERVICETEST"));
            if(requisitionRepository.ExistsById("RSERVICETEST1"))
                requisitionRepository.Delete(requisitionRepository.FindById("RSERVICETEST1"));
            if(requisitionRepository.ExistsById("RSERVICETEST2"))
                requisitionRepository.Delete(requisitionRepository.FindById("RSERVICETEST2"));
            if (retrievalRepository.ExistsById("RSERVICETEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("RSERVICETEST"));
            if (requisitionRepository.FindAll().Where(r => r.EmployeeRemarks.Contains("RSERVICETEST")).Count() > 0)
                requisitionRepository.Delete(requisitionRepository.FindAll().Where(r => r.EmployeeRemarks.Contains("RSERVICETEST")).FirstOrDefault());
        }
    }
}