using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using team7_ssis.Models;
using team7_ssis.Repositories;
using team7_ssis.Services;

namespace team7_ssis.Tests.Services
{
    [TestClass]
    class ItemServiceTest
    {
        ApplicationDbContext context;
        ItemService itemService;
        ItemRepository itemRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            itemRepository = new ItemRepository(context);

        }

        [TestMethod]
        public void FindAllItemsTest()
        {
            //test
        }

        
    }
}
