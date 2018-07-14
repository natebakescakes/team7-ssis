using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Tests.Repositories
{
    [TestClass()]
    public class ItemPriceRepositoryTests
    {
        ApplicationDbContext context;
        ItemPriceRepository itemPriceRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Arrange
            context = new ApplicationDbContext();
            itemPriceRepository = new ItemPriceRepository(context);
        }

        [TestMethod]
        public void CountTestNotNull()
        {
            // Act
            int result = itemPriceRepository.Count();

            // Assert
            Assert.IsTrue(result >= 0, "Unable to count properly");
        }

        [TestMethod]
        public void FindAllTestNotNull()
        {
            // Act
            int result = itemPriceRepository.FindAll().Count;

            // Assert
            Assert.IsTrue(result >= 0, "Unable to find all properly");
        }

        [TestMethod]
        public void FindByIdTestNotNull()
        {
            // Act
            var result = itemPriceRepository.FindById("T003", "OMEG");

            // Assert
            Assert.IsInstanceOfType(result, typeof(ItemPrice));
        }

        [TestMethod]
        public void ExistsByIdTestIsTrue()
        {
            // Act
            var result = itemPriceRepository.ExistsById("T003", "OMEG");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveTestExistingChangeStatus()
        {
            // Arrange
            var status = new StatusRepository(context).FindById(14);
            var itemPrice = itemPriceRepository.FindById("T023", "ALPA");
            var original = itemPrice.Status;
            itemPrice.Status = status;

            // Act
            var result = itemPriceRepository.Save(itemPrice);

            // Assert
            Assert.AreEqual(status, result.Status);

            // Tear Down
            itemPrice.Status = original;
            itemPriceRepository.Save(itemPrice);
        }

        [TestMethod]
        public void SaveAndDeleteTestNew()
        {
            // Save new object into DB
            // Arrange
            var itemPrice = new ItemPrice
            {
                ItemCode = "E030",
                SupplierCode = "CHEP",
                CreatedDateTime = DateTime.Now
            };

            // Act
            var saveResult = itemPriceRepository.Save(itemPrice);

            // Assert
            Assert.IsInstanceOfType(saveResult, typeof(ItemPrice));

            // Delete saved object from DB
            // Act
            itemPriceRepository.Delete(saveResult);

            // Assert
            Assert.IsNull(itemPriceRepository.FindById("E030", "CHEP"));
        }
    }
}