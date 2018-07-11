using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository.Tests
{
    [TestClass()]
    public class StockMovementRepositoryTests
    {
        [TestMethod()]
        public void CountTestsNotNull()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var stockMovementRepository = new StockMovementRepository();

            // Act
            int result = stockMovementRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }
    }
}