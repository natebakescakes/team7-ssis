using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

namespace team7_ssis.Controllers
{
    public class InventoryApiController : ApiController
    {
        public static ApplicationDbContext context = new ApplicationDbContext();
        ItemService itemService = new ItemService(context);

        [Route("api/manage/items")]
        [HttpGet]
        public IEnumerable<ItemViewModel> FindAllItems()
        {
            List<Item> list = itemService.FindAllItems();
            List<ItemViewModel> items = new List<ItemViewModel>();

            foreach(Item i in list)
            {
                items.Add(new ItemViewModel
                {
                    ItemCode = i.ItemCode,
                    ItemCategoryName = i.ItemCategory != null ? i.ItemCategory.Name: "",
                    Description = i.Description,
                    ReorderLevel = i.ReorderLevel,
                    ReorderQuantity = i.ReorderQuantity,
                    Uom = i.Uom,
                    Quantity= i.Inventory.Quantity
                });
            }
            return items;
        }
    }
}
