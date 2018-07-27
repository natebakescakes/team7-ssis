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
                    ItemCategoryName = i.ItemCategory != null ? i.ItemCategory.Name: "",
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
            return new ItemDetailModel()
            {
                ItemCode = item.ItemCode,
                Description = item.Description,
                ItemCategoryName = item.ItemCategory.Name,
                Bin = item.Bin,
                Uom = item.Uom,
                Quantity = item.Inventory.Quantity,
                Status = item.Status.StatusId
            };

        }

        [Route("api/manage/supplierInfo/{itemCode}")]
        [HttpGet]
        public List<ItemDetailsSupplierInfoViewModel> FindSupplierInfo(string itemCode)
        {
            int count = 0;
            List<ItemPrice> itemPrices = itemPriceService.FindAllItemPriceByOrder(itemCode);
            List<ItemDetailsSupplierInfoViewModel> list = new List<ItemDetailsSupplierInfoViewModel>();
            foreach(ItemPrice i in itemPrices)
            {
                count++;
                list.Add(new ItemDetailsSupplierInfoViewModel()
                {
                    Number= count,
                    SupplierName = i.Supplier.Name,
                    SupplierUnitPrice = (double)i.Price
                });
            }
            return list;
        }

        [Route("api/manage/supplierInfoAll")]
        [HttpGet]
        public List<ItemDetailsSupplierInfoViewModel> FindAllSupplierInfo()
        {
            int count = 0;
            List<Supplier> slist = new SupplierService(context).FindAllSuppliers();
            List<ItemDetailsSupplierInfoViewModel> list = new List<ItemDetailsSupplierInfoViewModel>();
            foreach (Supplier i in slist)
            {
                count++;
                list.Add(new ItemDetailsSupplierInfoViewModel()
                {
                    Number = count,
                    SupplierName = i.Name
                });
            }
            return list;
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
