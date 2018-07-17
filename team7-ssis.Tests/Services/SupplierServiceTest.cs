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
    public class SupplierServiceTest
    {
        ApplicationDbContext context;
        SupplierService supplierService;
        SupplierRepository supplierRepository; 
        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            supplierService = new SupplierService(context);
            supplierRepository = new SupplierRepository(context);
        }

        [TestMethod]
        public void FindSupplierByIdTest()
        {
            //Arrange
            string expected = "BANE";

            //Act
            var result = supplierService.FindSupplierById(expected);

            //Assert
            Assert.AreEqual(expected, result.SupplierCode);
            
        }

        [TestMethod]
        public void FindAllSuppliersTest()
        {
            //Arrange
            int expected = context.Supplier.Count();
            //Act
            var result = supplierService.FindAllSuppliers().Count();
            //Assert
            Assert.AreEqual(expected, result);
           
        }

        [TestMethod]
        public void SaveTest()
        {
            //Arrange
            Supplier supplier = new Supplier();
            supplier.SupplierCode = "YYYY";
            supplier.CreatedDateTime = DateTime.Now;
            //Act
            var result = supplierService.Save(supplier);
            //Assert
            Assert.AreEqual("YYYY", result.SupplierCode);
            Assert.IsNotNull(context.Supplier.Where(x => x.SupplierCode == "YYYY").First());
            supplierRepository.Delete(result);
        }

    }
}
