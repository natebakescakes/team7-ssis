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
        private ApplicationDbContext context;
        ItemService itemService;

        public InventoryApiController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
        }

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
                    ItemCategoryName = i.ItemCategory.Name,
                    Description = i.Description,
                    ReorderLevel = i.ReorderLevel,
                    ReorderQuantity = i.ReorderQuantity,
                    Uom = i.Uom,
                    Quantity= i.Inventory.Quantity
                });
            }
            return items;
        }

        [Route("api/delete/items")]
        [HttpPost]
        public HttpResponseMessage DeleteItems(string[] itemCodes)
        {
            List<Item> list = new List<Item>();
            for (int i=0;i< itemCodes.Length; i++)
            {
                list.Add(itemService.FindItemByItemCode(itemCodes[i]));
            }
            Console.WriteLine("Number of Items to be deleted:" + list.Count);
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    itemService.DeleteItem(list[i]);
                }
            }catch(Exception e) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            return Request.CreateResponse(HttpStatusCode.OK); ;
        }
    }
}
