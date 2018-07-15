using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class SupplierRepositoryTests
    {
        ApplicationDbContext context;
        SupplierRepository supplierRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            supplierRepository = new SupplierRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = supplierRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = supplierRepository.FindAll().Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = supplierRepository.FindById("CHEP");

            // Assert
            Assert.IsInstanceOfType(result, typeof(Supplier));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = supplierRepository.ExistsById("CHEP");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeContactName()
        {
            // Arrange
            var supplier = supplierRepository.FindById("CHEP");
            var original = supplier.ContactName;
            supplier.ContactName = "TEST";

            // Act
            var result = supplierRepository.Save(supplier);

            // Assert
            Assert.AreEqual("TEST", result.ContactName);

            // Tear Down
            supplier.ContactName = original;
            supplierRepository.Save(supplier);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var supplier = new Supplier
            {
                SupplierCode = "XXXX",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = supplierRepository.Save(supplier);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(Supplier));

            // Delete saved object from DB
            // Act
            supplierRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(supplierRepository.FindById("XXXX"));
        }
    }
}