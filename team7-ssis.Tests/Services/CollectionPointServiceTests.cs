using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    public class CollectionPointServiceTests
    {
        ApplicationDbContext context;
        CollectionPointService collectionPointService;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            collectionPointService = new CollectionPointService(context);
        }
        [TestMethod]
        public void FindAllCollectionPointsTest()
        {
            //Arrange
            int expected = context.CollectionPoint.Count();
            //Act
            int result = collectionPointService.FindAllCollectionPoints().Count();
            //Assert
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void FindAllCollectionPointsUniqueTest()
        {
            //Assert
            CollectionAssert.AllItemsAreUnique(collectionPointService.FindAllCollectionPoints());
        }
        [TestMethod]
        public void FindAllCollectionPointsObjectTest()
        {
            //Act
            var result = collectionPointService.FindAllCollectionPoints();
            //Assert
            CollectionAssert.AllItemsAreInstancesOfType(result, typeof(CollectionPoint));
        }
        [TestMethod]
        public void FindAllCollectionPointsNotNullTest()
        {
            //Assert
            CollectionAssert.AllItemsAreNotNull(collectionPointService.FindAllCollectionPoints());
        }
    }
}
