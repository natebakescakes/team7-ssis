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
                PlanQuantity = 1,
                ActualQuantity = 0

            };
            disbursementdetailRepository.Save(detail);

            Requisition requisition = new Requisition()
            {
                RequisitionId = "TEST",
                Retrieval = retrieval,
                CreatedDateTime = DateTime.Now

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
            retrieval.Requisitions.Add(context.Requisition.First());
            a.Retrieval = retrieval;
            a.CreatedDateTime = DateTime.Now;
            disbursementService.Save(a);

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

            //delete retrieval objects
            retrievalRepository.Delete(retrieval);
            //}

            if (disbursementRepository.ExistsById("COLLECTIONTEST"))
                disbursementRepository.Delete(disbursementRepository.FindById("COLLECTIONTEST"));
        }

    }
}
