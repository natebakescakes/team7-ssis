using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using team7_ssis.Models;
using team7_ssis.Services;
using team7_ssis.ViewModels;

using System.IO;

namespace team7_ssis.Controllers
{
    public class InventoryApiController : ApiController
    {
        private ApplicationDbContext context;
        private ItemService itemService;
        private ItemPriceService itemPriceService;
        private RequisitionService requisitionService;
        private StockMovementService stkMovementService;
        private PurchaseOrderService purchaseOrderService;
        

        public InventoryApiController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            stkMovementService = new StockMovementService(context);
            itemPriceService = new ItemPriceService(context);
            requisitionService = new RequisitionService(context);
            purchaseOrderService = new PurchaseOrderService(context);
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
                    qty = (i.OriginalQuantity < i.AfterQuantity ? (i.AfterQuantity - i.OriginalQuantity) : (i.OriginalQuantity - i.AfterQuantity)),
                    qtyString= (i.OriginalQuantity == i.AfterQuantity ?"0":(i.OriginalQuantity < i.AfterQuantity ? "+"+ (i.AfterQuantity - i.OriginalQuantity) :"-"+ (i.OriginalQuantity - i.AfterQuantity))),
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
                    ItemCategoryName = i.ItemCategory != null ? i.ItemCategory.Name : "",
                    Description = i.Description,
                    ReorderLevel = i.ReorderLevel,
                    ReorderQuantity = i.ReorderQuantity,
                    Uom = i.Uom,
                    Quantity = i.Inventory.Quantity,
                    UnitPrice = itemPriceService.GetDefaultPrice(i, 1),
                    
                    ImagePath = (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath("/Images/" + i.ItemCode.ToString() + ".JPG"))) ? i.ItemCode : "default"
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
                ReorderLevel= item.ReorderLevel,
                ReorderQuantity = item.ReorderQuantity,
                Quantity = item.Inventory.Quantity,
                Status = item.Status.StatusId
            };

        }

        [Route("api/manage/quantity/{itemCode}")]
        [HttpGet]
        public MessageViewModel FindItemQuantity(string itemCode)
        {
            Item item = itemService.FindItemByItemCode(itemCode);

            return new MessageViewModel()
            {
                Message = item.Inventory.Quantity.ToString()
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

        [Route("api/manage/itempricelist")]
        [HttpGet]
        public List<ItemPricesListViewModel> FindAllItemPriceList()
        {
            List<Item> iList = itemService.FindAllActiveItems();
            List<ItemPricesListViewModel> ip = new List<ItemPricesListViewModel>();
            ItemPricesListViewModel myModel = new ItemPricesListViewModel();
            foreach (Item i in iList)
            {
                ip.Add(new ItemPricesListViewModel()
                {
                    ItemCode = i.ItemCode,
                    Description = i.Description,
                    Code1 = itemPriceService.FindOneByItemAndSequence(i, 1).SupplierCode,
                    Code2 = itemPriceService.FindOneByItemAndSequence(i, 2).SupplierCode,
                    Code3 = itemPriceService.FindOneByItemAndSequence(i, 3).SupplierCode,
                    Price1 = itemPriceService.FindOneByItemAndSequence(i, 1).Price,
                    Price2 = itemPriceService.FindOneByItemAndSequence(i, 2).Price,
                    Price3 = itemPriceService.FindOneByItemAndSequence(i, 3).Price

            });
            }
            return ip;
        }


        [Route("api/delete/items")]
        [HttpPost]
        public Boolean DeleteItems([FromBody]string[] itemCodes)
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
                return false;
            }

            Console.WriteLine("In API Controller OK");
            return true;

        }

        [Route("api/inventory/shortfall")]
        [HttpGet]
        public List<ItemViewModel> Shortfall()
        {
            //FindInventoryShortfallItems
            return purchaseOrderService.FindInventoryShortfallItems().Select(item => new ItemViewModel()
            {
                ItemCode = item.ItemCode,
                Description = item.Description,
                Quantity = item.Inventory.Quantity,
                ReorderLevel = item.ReorderLevel,
                ReorderQuantity = item.ReorderQuantity,
                Uom = item.Uom,
                UnfulfilledQuantity = requisitionService.FindUnfulfilledQuantityRequested(item),
                AmountToReorder = (item.Inventory.Quantity - requisitionService.FindUnfulfilledQuantityRequested(item) < item.ReorderLevel) ?
                                Math.Max(requisitionService.FindUnfulfilledQuantityRequested(item) + item.ReorderQuantity, item.ReorderLevel-item.Inventory.Quantity+ requisitionService.FindUnfulfilledQuantityRequested(item)) : 0
            
            }).ToList();

        }

        [Route("api/inventory/items")]
        [HttpPost]
        public List<ItemViewModel> GeneratePO([FromBody]string itemNums)
        { 
            List<ItemViewModel> itemView = new List<ItemViewModel>();
            if (itemNums == null) {

                return itemView;
            }

            else
            {
                string[] itemArray = itemNums.Split(',');
                List<Item> items = new List<Item>();

                foreach (string i in itemArray)
                {
                    Item item =itemService.FindItemByItemCode(i);
                    items.Add(item);
                }

                return items.Select(item => new ItemViewModel()
                {
                    ItemCode = item.ItemCode,
                    Description = item.Description,
                    UnitPriceDecimal=itemPriceService.FindOneByItemAndSequence(item,1).Price,
                    ItemCategoryName=item.ItemCode,
                    Quantity = (item.Inventory.Quantity - requisitionService.FindUnfulfilledQuantityRequested(item) < item.ReorderLevel) ?
                                Math.Max(requisitionService.FindUnfulfilledQuantityRequested(item) + item.ReorderQuantity, item.ReorderLevel - item.Inventory.Quantity + requisitionService.FindUnfulfilledQuantityRequested(item)) : 0,

                    TotalPrice = ((item.Inventory.Quantity - requisitionService.FindUnfulfilledQuantityRequested(item) < item.ReorderLevel) ?
                                Math.Max(requisitionService.FindUnfulfilledQuantityRequested(item) + item.ReorderQuantity, item.ReorderLevel - item.Inventory.Quantity + requisitionService.FindUnfulfilledQuantityRequested(item)) : 0) *
                                itemPriceService.FindOneByItemAndSequence(item, 1).Price
                }).ToList();
            }

            
        }


    }
}
