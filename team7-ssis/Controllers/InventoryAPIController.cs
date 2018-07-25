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
        ItemPriceService itemPriceService;

        public InventoryApiController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            itemPriceService = new ItemPriceService(context);
        }

        [Route("api/manage/items")]
        [HttpGet]
        public IEnumerable<ItemViewModel> FindAllItems()
        {
            Console.WriteLine("Find All Items API");
            List<Item> list = itemService.FindAllItems();
            List<ItemViewModel> viewModel = new List<ItemViewModel>();

            foreach(Item i in list)
            {
                ItemViewModel ivm = new ItemViewModel();
                ivm.ItemCode = i.ItemCode;
                ivm.ItemCategoryName = i.ItemCategory != null ? i.ItemCategory.Name : "";
                ivm.Description = i.Description;
                ivm.ReorderLevel = i.ReorderLevel;
                ivm.ReorderQuantity = i.ReorderQuantity;
                ivm.Uom = i.Uom;
                ivm.Quantity = i.Inventory.Quantity;
                try
                {
                    ivm.UnitPrice = itemPriceService.GetDefaultPrice(i, 1);
                } catch
                {
                    ivm.UnitPrice = "";
                }

                viewModel.Add(ivm);
            }
            return viewModel;
        }

        [Route("api/delete/items")]
        [HttpPost]
        public HttpResponseMessage DeleteItems([FromBody]string[] itemCodes)
        {
            Console.WriteLine("In API Controller" + itemCodes.Length);
            List<Item> list = new List<Item>();
            foreach(string i in itemCodes)
            {
                list.Add(itemService.FindItemByItemCode(i));
            }
            Console.WriteLine("Number of Items to be deleted:" + list.Count);
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    itemService.DeleteItem(list[i]);
                }
            }catch(Exception e) {
                Console.WriteLine("In API Controller error"+e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

            Console.WriteLine("In API Controller OK");
            return Request.CreateResponse(HttpStatusCode.OK); ;
        }
    }
}
