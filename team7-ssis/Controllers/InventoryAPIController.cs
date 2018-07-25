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
        StockMovementService stkMovementService;
        ItemPriceService itemPriceService;

        public InventoryApiController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            stkMovementService = new StockMovementService(context);
            itemPriceService = new ItemPriceService(context);
        }

        [Route("api/manage/stockhistory/{itemCode}")]
        [HttpGet]
        public IEnumerable<StockHistoryViewModel> FindStockHistoryByItem(string itemCode)
        {
            //System.Diagnostics.Debug.WriteLine("FindStockHistoryByItem",itemCode);
            List<StockMovement> list = stkMovementService.FindStockMovementByItemCode(itemCode);
            List<StockHistoryViewModel> items = new List<StockHistoryViewModel>();

            foreach (StockMovement i in list)
            {
                items.Add(new StockHistoryViewModel
                {
                    theDate = i.CreatedDateTime,
                    host = (i.DeliveryOrderDetailItemCode != null ? i.DeliveryOrderDetail.DeliveryOrder.Supplier.Name : i.DisbursementDetailItemCode != null ? i.DisbursementDetail.Disbursement.Department.Name : i.StockAdjustmentDetail.StockAdjustmentId),
                    qty = i.AfterQuantity - i.OriginalQuantity,
                    balance = i.AfterQuantity
                });
            }
            return items;
        }

        [Route("api/manage/items")]
        [HttpGet]
        public IEnumerable<ItemViewModel> FindAllItems()
        {
            Console.WriteLine("Find All Items API");
            //List<Item> list = itemService.FindAllItems();
            List<Item> list = itemService.FindAllActiveItems();
            List<ItemViewModel> items = new List<ItemViewModel>();

            foreach (Item i in list)
            {
                items.Add(new ItemViewModel
                {
                    ItemCode = i.ItemCode,
                    ItemCategoryName = i.ItemCategory.Name,
                    Description = i.Description,
                    ReorderLevel = i.ReorderLevel,
                    ReorderQuantity = i.ReorderQuantity,
                    Uom = i.Uom,
                    Quantity = i.Inventory.Quantity
                });
            }
            return items;
        }

        [Route("api/manage/singleitem/{itemCode}")]
        [HttpGet]
        public ItemDetailModel FindItemSingle(string itemCode)
        {
            Item item = itemService.FindItemByItemCode(itemCode);
            List<ItemPrice> itemPrice = itemPriceService.FindItemPriceByItemCode(itemCode);
            List<ItemDetailsSupplierInfoViewModel> idSupp = new List<ItemDetailsSupplierInfoViewModel>();
            foreach(ItemPrice i in itemPrice)
            {
                idSupp.Add(new ItemDetailsSupplierInfoViewModel()
                {
                    SupplierName = i.Supplier.Name,
                    SupplierUnitPrice = (double)i.Price
                });
            }

            return new ItemDetailModel()
            {
                ItemCode = item.ItemCode,
                Description = item.Description,
                ItemCategoryName = item.ItemCategory.Name,
                Bin = item.Bin,
                Uom = item.Uom,
                Quantity = item.Inventory.Quantity,
                Status = item.Status.Name,
                SupplierInfo = idSupp
            };

        }

        [Route("api/delete/items")]
        [HttpPost]
        public IHttpActionResult DeleteItems([FromBody]string[] itemCodes)
        {
            Console.WriteLine("In API Controller" + itemCodes.Length);
            List<Item> list = new List<Item>();
            foreach (string i in itemCodes)
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
            } catch (Exception e) {
                Console.WriteLine("In API Controller error" + e);
                return BadRequest();

            }

            Console.WriteLine("In API Controller OK");
            return Ok();

        }
    }
}
