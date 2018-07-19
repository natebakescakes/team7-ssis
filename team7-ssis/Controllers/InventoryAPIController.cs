using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;

namespace team7_ssis.Controllers
{
    public class InventoryAPIController : ApiController
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        ItemService itemService = new ItemService(context);
        public IEnumerable<Item> GetAllItem()
        {
            var k = itemService.FindAllItems();
            return k;
        }
    }
}
