using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public  class ItemService
    {
        ApplicationDbContext context;
        ItemRepository itemRepository;
        StatusRepository statusRepository;
        InventoryRepository inventoryRepository;

        public ItemService(ApplicationDbContext context)
        {
            this.context = context;
            itemRepository = new ItemRepository(context);
            statusRepository = new StatusRepository(context);
            inventoryRepository = new InventoryRepository(context);
        }

        public Item FindItemByItemCode(string itemCode)
        {
            Console.WriteLine("In findbyitemcode item" + itemCode.ToString());

            return itemRepository.FindById(itemCode);
        }

        public  List<Item> FindAllItems()
        {
            return itemRepository.FindAll().ToList();
        }

        public List<Item> FindAllActiveItems()
        {
            return itemRepository.FindAllActive().ToList();
        }


        public List<Item> FindItemsByCategory(ItemCategory itemCategory)
        {
            return itemRepository.FindByCategory(itemCategory).ToList();
        }
        
        public Item Save(Item item,int quantity)
        {
            Item result=itemRepository.Save(item);
            SaveInventory(result,quantity);
            return result;
        }

        public List<Item> FindItemQuantityLessThanReorderLevel()
        {
            return itemRepository.FindQuantity().ToList();
        }

        public Inventory SaveInventory(Item item,int quantity)
        {
            Inventory iv = new Inventory();
            iv.ItemCode = item.ItemCode;
            iv.Quantity = quantity;
            return inventoryRepository.Save(iv);
        }

        public Inventory UpdateQuantity(Item item,int quantity)
        {
            Inventory iv = inventoryRepository.FindById(item.ItemCode);
            iv.Quantity = quantity;
            return inventoryRepository.Save(iv);
        }

        public Inventory AddQuantity(Item item,int quantity)
        {
            Inventory iv = inventoryRepository.FindById(item.ItemCode);
            iv.Quantity = iv.Quantity + quantity;
            return inventoryRepository.Save(iv);
        }

        public Item DeleteItem(Item item)
        {
            Console.WriteLine("In delete item" + item.ItemCode.ToString());
            Item a = itemRepository.FindById(item.ItemCode);
            a.Status= statusRepository.FindById(0);
            return itemRepository.Save(a);
        }
        public int UploadItemImage(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string path = HttpContext.Current.Server.MapPath("~/Uploads/");
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + System.IO.Path.GetFileName(file.FileName));
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}