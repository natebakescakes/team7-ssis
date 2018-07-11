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
    public class StatusRepositoryTests
    {
        [TestMethod()]
        public void CountTestValid()
        {
            // Arrange
            var context = new ApplicationDbContext();
            var statusRepository = new StatusRepository();

            // Act
            int result = statusRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }
    }
}