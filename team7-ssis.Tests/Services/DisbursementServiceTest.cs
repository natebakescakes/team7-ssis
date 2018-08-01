using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class DisbursementServiceTest
    {
        ApplicationDbContext context;
        DisbursementService disbursementService;
        DisbursementRepository disbursementRepository;
        ItemRepository itemRepository;
        RetrievalRepository retrievalRepository;
        DisbursementDetailRepository disbursementdetailRepository;
        RequisitionRepository requisitionRepository;
        RequisitionDetailRepository requisitiondetailRepository;
        DepartmentRepository departmentRepository;


        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            disbursementRepository = new DisbursementRepository(context);
            disbursementdetailRepository = new DisbursementDetailRepository(context);
            disbursementService = new DisbursementService(context);
            itemRepository = new ItemRepository(context);
            retrievalRepository = new RetrievalRepository(context);
            requisitionRepository = new RequisitionRepository(context);
            requisitiondetailRepository = new RequisitionDetailRepository(context);
            departmentRepository = new DepartmentRepository(context);

            Retrieval retrieval = new Retrieval();

            if (retrievalRepository.FindById("TEST") == null)
            {
                //save retrieval object into db

                retrieval.RetrievalId = "TEST";
                retrieval.CreatedDateTime = DateTime.Now;
                retrievalRepository.Save(retrieval);
            }
            else retrieval = retrievalRepository.FindById("TEST");

            //save disbursement object into db

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
                Item = context.Item.First(),
                PlanQuantity = 2,
                ActualQuantity = 2

            };
            disbursementdetailRepository.Save(detail);

            Requisition requisition = new Requisition()
            {
                RequisitionId = "TEST",
                Retrieval = retrieval,
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(8),
                RequisitionDetails = new List<RequisitionDetail>()
                {
                    new RequisitionDetail()
                    {
                        RequisitionId = "TEST",
                        ItemCode = "E030",
                        Status = new StatusService(context).FindStatusByStatusId(8),
                    }
                }

            };
            requisitionRepository.Save(requisition);

        }
        [TestMethod]
        public void FindAllDisbursementsTest()
        {
            //Arrange
            int expected = context.Disbursement.Count();
            //Act
            var result = disbursementService.FindAllDisbursements().Count;

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FindDisbursementByIdTest()
        {
            //Arrange
            
            string expected = "TEST";

            //Act
            var result = disbursementService.FindDisbursementById(expected);
            

            //Assert
            Assert.AreEqual(expected, result.DisbursementId);

        }

        [TestMethod]
        public void SaveTest()
        {
            //Arrange
            Disbursement newDisbursement = new Disbursement();
            newDisbursement.DisbursementId = IdService.GetNewDisbursementId(context);
            newDisbursement.CreatedDateTime = DateTime.Now;
            string expected = newDisbursement.DisbursementId;

            //Act
            var result = disbursementService.Save(newDisbursement);
            disbursementRepository.Delete(newDisbursement);
            //Assert
            Assert.AreEqual(expected, result.DisbursementId);


        }



        [TestMethod]
        public void ConfirmCollectionTest()
        {
            int expected = 10; //status id of Items Collected

            //get retrieval object
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();

            //create disbursement and save it into database
            Disbursement a = new Disbursement();
            a.DisbursementId = IdService.GetNewDisbursementId(context);
            a.Department = departmentRepository.FindById("COMM");
            retrieval.Requisitions.Add(context.Requisition.First());
            a.Retrieval = retrieval;
            a.CreatedDateTime = DateTime.Now;
            disbursementService.Save(a);

            //include Disbursement detail object and save it
            DisbursementDetail detail = new DisbursementDetail()
            {
                Disbursement = a,
                Item = context.Item.First(),
                PlanQuantity = 1,
                ActualQuantity = 1
                        
            };
            disbursementdetailRepository.Save(detail);

            Disbursement result = disbursementService.ConfirmCollection(a.DisbursementId);
            

            //Asert
            Assert.AreEqual(expected, result.Status.StatusId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConfirmCollection_ThrowsError()
        {
            // Arrange
            disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "COLLECTIONTEST",
                CreatedDateTime = DateTime.Now,
                Status = new StatusService(context).FindStatusByStatusId(10),
            });

            // Act
            disbursementService.ConfirmCollection("COLLECTIONTEST");

            // Assert
        }

        [TestMethod]
        public void FindDisbursementsByRetrievalIdTest()
        {
            //Arrange
            //get retrival object
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();

            //save 2 disbursement objects
            Disbursement a = new Disbursement();
            a.DisbursementId = IdService.GetNewDisbursementId(context);
            a.Retrieval = retrieval;
            a.CreatedDateTime = DateTime.Now;
            disbursementService.Save(a);

            Disbursement b = new Disbursement();
            b.DisbursementId = IdService.GetNewDisbursementId(context);
            b.Retrieval = retrieval;
            b.CreatedDateTime = DateTime.Now;
            disbursementService.Save(b);

            //find any existing data in disbursement where RetrievalId = TESTER and add 2 more
            int expected = context.Disbursement.Where(x => x.Retrieval.RetrievalId == "TEST").Count();

            //Act
            var result = disbursementService.FindDisbursementsByRetrievalId(retrieval.RetrievalId).Count;

            //Assert
            Assert.AreEqual(expected, result);

            //Delete dummy test objects in TestCleanUp

        }


        [TestMethod]
        public void UpdateActualQuantityForDisbursementDetailTest()
        {
            //Arrange
            //create disbursement object
            Disbursement newDisbursement = context.Disbursement.Where(x => x.DisbursementId == "TEST").First();

            //assign disbursement detail object to disbursement
            DisbursementDetail disbursementDetail = context.DisbursementDetail
                .Where(x => x.DisbursementId == newDisbursement.DisbursementId).First();

            int expected = 1;

            //Act
            var result = disbursementService.UpdateActualQuantityForDisbursementDetail(newDisbursement.DisbursementId, disbursementDetail.ItemCode, expected);
            disbursementService.UpdateActualQuantityForDisbursementDetail(newDisbursement.DisbursementId, disbursementDetail.ItemCode, disbursementDetail.ActualQuantity);


            //Assert
            Assert.AreEqual(expected, result.DisbursementDetails.First().ActualQuantity);


        }

        [TestMethod]
        public void UpdateRequisitionStatusTest_SingleRequisition()
        {
            //Act
            //Get test requisition object from db
            Disbursement disbursement = context.Disbursement.Where(x => x.DisbursementId == "TEST").First();
            Requisition requisition = context.Requisition.Where(x => x.RequisitionId == "TEST").First();

            Item item = context.Item.First();
            

            //make and save 2 multiple requisition detail objects
            RequisitionDetail rd1 = new RequisitionDetail()
            {
                Requisition = requisition,

                Item = item,
                Quantity = 1
            };
            requisitiondetailRepository.Save(rd1);
   

            RequisitionDetail rd2 = new RequisitionDetail()
            {
                Requisition = requisition,
                Item = itemRepository.FindById("C003"),
                Quantity = 2
            };

            requisitiondetailRepository.Save(rd2);


            //Arrange
            var result = disbursementService.UpdateRequisitionStatus(disbursement);

            //Assert
            //disbursement detail disburses 2, so rd1 should be statusID(10), 
            Assert.AreEqual(result.First().RequisitionDetails.First().Status.StatusId, 10);
            Assert.AreEqual(result.First().Status.StatusId, 9);


        }

        [TestMethod]
        public void UpdateRequisitionStatusTest_MultipleRequisition()
        {
            //Act
            //Get test requisition object from db
            Disbursement disbursement = context.Disbursement.Where(x => x.DisbursementId == "TEST").First();
            Requisition requisition1 = context.Requisition.Where(x => x.RequisitionId == "TEST").First();
            

            Item item = context.Item.First();

            //Make 2nd Requsition test object
            Requisition requisition2 = new Requisition()
            {
                RequisitionId = "TEST2",
                Retrieval = requisition1.Retrieval,
                CreatedDateTime = DateTime.Now
            };
            requisitionRepository.Save(requisition2);


            //make and save  1 requisition detail objects to each requisition
            RequisitionDetail rd1 = new RequisitionDetail()
            {
                Requisition = requisition1,

                Item = item,
                Quantity = 1
            };
            requisitiondetailRepository.Save(rd1);


            RequisitionDetail rd2 = new RequisitionDetail()
            {
                Requisition = requisition2,
                Item = item,
                Quantity = 5

            };

            requisitiondetailRepository.Save(rd2);

            //Arrange
            var result = disbursementService.UpdateRequisitionStatus(disbursement);

            //Assert
            RequisitionDetail result1 = result.Find(x => x.RequisitionId == "TEST").RequisitionDetails.First();
            RequisitionDetail result2 = result.Find(x => x.RequisitionId == "TEST2").RequisitionDetails.First();
            Assert.AreEqual(result1.Status.StatusId, 10);
            Assert.AreEqual(result2.Status.StatusId, 9);
            Assert.AreEqual(result.First().Status.StatusId, 10);
        }

        [TestMethod]
        public void UpdateActualQuantityForDisbursementDetail_Valid()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "DSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "DSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "DSERVICETEST",
                        ItemCode = "E030",
                        PlanQuantity = 30,
                        ActualQuantity = 30,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            new DisbursementService(context).UpdateActualQuantityForDisbursementDetail("DSERVICETEST", "E030", 20, "StoreClerk1@email.com");

            // Assert
            Assert.AreEqual(20, new DisbursementDetailRepository(context).FindById("DSERVICETEST", "E030").ActualQuantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateActualQuantityForDisbursementDetail_DoesNotExist_ThrowsException()
        {
            // Arrange

            // Act
            new DisbursementService(context).UpdateActualQuantityForDisbursementDetail("FAKETEST", "E030", 2, "StoreClerk1@email.com");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateActualQuantityForDisbursementDetail_ActualQuantityMore_ThrowsException()
        {
            // Arrange
            var requisitionRepository = new RequisitionRepository(context);
            var departmentRepository = new DepartmentRepository(context);

            var requisition = requisitionRepository.Save(new Requisition()
            {
                RequisitionId = "DSERVICETEST",
                CollectionPoint = departmentRepository.FindById("ENGL").CollectionPoint,
                Department = departmentRepository.FindById("ENGL"),
                CreatedDateTime = DateTime.Now,
            });
            var retrieval = retrievalRepository.Save(new Retrieval()
            {
                RetrievalId = "DSERVICETEST",
                Requisitions = new List<Requisition>() { requisition },
                CreatedDateTime = DateTime.Now,
            });
            var disbursement = disbursementRepository.Save(new Disbursement()
            {
                DisbursementId = "DSERVICETEST",
                Department = departmentRepository.FindById("ENGL"),
                DisbursementDetails = new List<DisbursementDetail>()
                {
                    new DisbursementDetail()
                    {
                        DisbursementId = "DSERVICETEST",
                        ItemCode = "E030",
                        PlanQuantity = 30,
                        ActualQuantity = 30,
                        Status = new StatusService(context).FindStatusByStatusId(18),
                    }
                },
                Retrieval = retrieval,
                Status = new StatusService(context).FindStatusByStatusId(1),
                CreatedDateTime = DateTime.Now,
            });

            // Act
            new DisbursementService(context).UpdateActualQuantityForDisbursementDetail("DSERVICETEST", "E030", 40, "StoreClerk1@email.com");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Retrieval retrieval = context.Retrieval.Where(x => x.RetrievalId == "TEST").First();

            List<Disbursement> list = disbursementService.FindDisbursementsByRetrievalId(retrieval.RetrievalId);
            foreach (Disbursement d in list)
            {
                //Delete dummy disbursement test objects
                disbursementRepository.Delete(d);

            }

            //have to delete requisitions before retrievals
            List<Requisition> requisitionlist = context.Requisition.Where(x => x.RequisitionId == "TEST").ToList();
            foreach(Requisition r in requisitionlist)
            {
                //delete dummy requisition test objects
                requisitionRepository.Delete(r);
            }

            List<Requisition> requisitionlist2 = context.Requisition.Where(x => x.RequisitionId == "TEST2").ToList();
            foreach (Requisition r in requisitionlist2)
            {
                //delete dummy requisition test objects
                requisitionRepository.Delete(r);
            }

            //delete retrieval objects
            retrievalRepository.Delete(retrieval);
            //}

            if (disbursementRepository.ExistsById("COLLECTIONTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("COLLECTIONTEST"));
            if (disbursementRepository.ExistsById("DSERVICETEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("DSERVICETEST"));
            if (requisitionRepository.ExistsById("DSERVICETEST"))
                requisitionRepository.Delete(requisitionRepository.FindById("DSERVICETEST"));
            if (retrievalRepository.ExistsById("DSERVICETEST"))
                retrievalRepository.Delete(retrievalRepository.FindById("DSERVICETEST"));
        }

    }
}
