using System;
using Microsoft.AspNet.Identity;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using team7_ssis.Services;
using team7_ssis.Models;
using team7_ssis.ViewModels;
using System.IO;
using Rotativa;

namespace team7_ssis.Controllers
{
    public class InventoryController : Controller
    {
        public ApplicationDbContext context;
        ItemService itemService;
        StatusService statusService;
        SupplierService supplierService;
        ItemCategoryService categoryService;
        UserService userService;
        ItemPriceService itemPriceService;

        public InventoryController()
        {
            context = new ApplicationDbContext();
            itemService = new ItemService(context);
            statusService = new StatusService(context);
            supplierService = new SupplierService(context);
            categoryService = new ItemCategoryService(context);
            userService = new UserService(context);
            itemPriceService = new ItemPriceService(context);
        }
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        //GET:New Item
        public ActionResult Create()
        {

            //get data for Status dropdownlist
            List<Status> list = new List<Status>();
            list.Add(statusService.FindStatusByStatusId(0));
            list.Add(statusService.FindStatusByStatusId(1));
            //get data for Supplier dropdownlist
            List<Supplier> list2 = new List<Supplier>();
            List<Supplier> sAllList = supplierService.FindAllSuppliers();
            //get data for Category dropdownlist
            List<ItemCategory> list3 = new List<ItemCategory>();
            List<ItemCategory> cAllList = categoryService.FindAllItemCategory();
            foreach(ItemCategory i in cAllList)
            {
                list3.Add(i);
            }
            foreach (Supplier i in sAllList)
            {
                list2.Add(i);
            }

            return View(new EditItemFinalViewModel
            {
                Statuses = new SelectList(
                    list.Select(x => new { Value = x.StatusId, Text = x.Name }),
                     "Value",
                    "Text"
                ),
                SupplierName = new SelectList(
                    list2.Select(x => new { Value = x.SupplierCode, Text = x.Name }),
                    "Value",
                    "Text"
                ),
                Categories= new SelectList(
                    list3.Select(x=>new { Value= x.ItemCategoryId, Text= x.Name }),
                    "Value",
                    "Text"
                )
            });
        }

        

        public ActionResult GeneratePrice()
        {
            //ViewBag.myFlag = flag;
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
            return View(ip);
        }

        
        public ActionResult PrintView()
        {
            //ViewBag.myFlag = flag;
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
            return View(ip);
        }

        public ActionResult PrintAllPrices()
        {
            var a = new ActionAsPdf("PrintView") { FileName = "ItemPrices.pdf" };
            a.Cookies = Request.Cookies.AllKeys.ToDictionary(k => k, k => Request.Cookies[k].Value);
            a.FormsAuthenticationCookieName = System.Web.Security.FormsAuthentication.FormsCookieName;
            a.CustomSwitches = "--load-error-handling ignore";
            return a;
        }



        //GET: Inventory Detail
        public ActionResult Details(string itemCode)
        {
            //get itemCode from findAll page
            ViewBag.VB = itemCode;
            //get data for Status dropdownlist
            List<Status> list = new List<Status>();
            list.Add(statusService.FindStatusByStatusId(0));
            list.Add(statusService.FindStatusByStatusId(1));
            //get data for Supplier dropdownlist
            List<Supplier> list2 = new List<Supplier>();
            List<Supplier> sAllList = supplierService.FindAllSuppliers();
            foreach (Supplier i in sAllList)
            {
                list2.Add(i);
            }

            //image
            string path;
            if (System.IO.File.Exists(Server.MapPath("~/Images/" + itemCode.ToString() + ".JPG")))
            {
                path = "~/Images/" + itemCode.ToString() + ".JPG";
            }
            else
            {
                path = "~/Images/default"+".JPG";
            }

            return View(new EditItemFinalViewModel
            {
                Statuses = new SelectList(
                    list.Select(x => new { Value = x.StatusId, Text = x.Name }),
                     "Value",
                    "Text"
                ),
                SupplierName = new SelectList(
                    list2.Select(x => new { Value = x.SupplierCode, Text = x.Name }),
                    "Value",
                    "Text"
                ),
                ImagePath= path

            });
        }



        public ActionResult DeleteImage(string filename)
        {
            Console.WriteLine(filename);
            if (System.IO.File.Exists(Server.MapPath("~/Images/" + filename + ".JPG")))
            {
                System.IO.File.Delete(Server.MapPath("~/Images/" + filename + ".JPG"));
            }
            return RedirectToAction("Details", new { itemCode = filename });
        }

        public void SaveImage(EditItemFinalViewModel model)
        {
            if (model.ImageFile != null)
            {
                Console.WriteLine(model.ImageFile);
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = model.ItemCode.ToString() + ".JPG";
                model.ImagePath = "~/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Images/") + fileName);
                model.ImageFile.SaveAs(fileName);
            }

        }

        //Save new or update existing ItemCategory
        [HttpPost]
        public ActionResult Save(EditItemFinalViewModel model)
        {
            Console.WriteLine(model);
            SaveImage(model);
            string error = "";
            Item newItem = new Item();
            ItemPrice newItemPrice = new ItemPrice();
            if (model.ItemCode != null)
            {
                if (itemService.FindItemByItemCode(model.ItemCode) == null)
                {
                    //for inventory
                    int quantity = 0;
                    //new item
                    newItem.ItemCode = model.ItemCode;
                    newItem.CreatedDateTime = DateTime.Now;
                    newItem.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    newItem.Name = model.ItemName;
                    newItem.Description = model.Description;
                    newItem.Uom = model.Uom;
                    newItem.ItemCategory=categoryService.FindItemCategoryByItemCategoryId(model.CategoryId);
                    newItem.Bin = model.Bin;
                    newItem.ReorderLevel = model.ReorderLevel;
                    newItem.ReorderQuantity = model.ReorderQuantity;
                    newItem.Status = new StatusService(context).FindStatusByStatusId(1);
                    try
                    {
                        itemService.Save(newItem, quantity);
                        ProcessItemPrice(model);
                    }
                    catch(Exception e)
                    {
                        error = "Error in item save";
                        Console.WriteLine("An error occurred in Item Save: '{0}'", e);
                    }
                }
                else
                {
                    //update exiting item
                    Item current = itemService.FindItemByItemCode(model.ItemCode);
                    List<ItemPrice> k = itemPriceService.FindItemPriceByItemCode(current.ItemCode);
                    foreach(ItemPrice i in k)
                    {
                        itemPriceService.DeleteItemPrice(i);
                    }
                    current.Description = model.Description;
                    current.ReorderLevel = model.ReorderLevel;
                    current.ReorderQuantity = model.ReorderQuantity;
                    current.Bin = model.Bin;
                    current.Uom = model.Uom;
                    current.Status = new StatusService(context).FindStatusByStatusId(1);
                    current.UpdatedBy= userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    current.UpdatedDateTime = DateTime.Now;
                    int quantity = current.Inventory.Quantity;
                    try
                    {
                        itemService.Save(current, quantity);
                        if (!ProcessItemPrice(model))
                        {
                            //write error case
                        }
                    }
                    catch(Exception e)
                    {
                        error = "Error in item update";
                        Console.WriteLine("An error occurred in Item Update: '{0}'", e);
                    }
                }
            }
            else
            {
                //show erro bcuz no item code
                error = "Item code comes as null";
            }

            return RedirectToAction("Index", "Inventory");
            //return new JsonResult { };
        }
        
        public Boolean ProcessItemPrice(EditItemFinalViewModel model)
        {
            try
            {
                if (model.SupplierName1 != null && model.SupplierName1!="0" && model.SupplierUnitPrice1!=0)
                {
                    ItemPrice p = new ItemPrice();
                    p.ItemCode = itemService.FindItemByItemCode(model.ItemCode).ItemCode;
                    p.SupplierCode = model.SupplierName1;
                    p.PrioritySequence = 1;
                    p.Price = model.SupplierUnitPrice1;
                    p.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    p.CreatedDateTime = DateTime.Now;
                    itemPriceService.Save(p);
                }
                if (model.SupplierName2 != null && model.SupplierName2 != "0" && model.SupplierUnitPrice2!=0)
                {
                    ItemPrice p = new ItemPrice();
                    p.ItemCode = itemService.FindItemByItemCode(model.ItemCode).ItemCode;
                    p.SupplierCode = model.SupplierName2;
                    p.PrioritySequence = 2;
                    p.Price = model.SupplierUnitPrice2;
                    p.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    p.CreatedDateTime = DateTime.Now;
                    itemPriceService.Save(p);
                }
                if (model.SupplierName3 != null && model.SupplierName3 != "0" && model.SupplierUnitPrice3!=0)
                {
                    ItemPrice p = new ItemPrice();
                    p.ItemCode = itemService.FindItemByItemCode(model.ItemCode).ItemCode;
                    p.SupplierCode = model.SupplierName3;
                    p.PrioritySequence = 3;
                    p.Price = model.SupplierUnitPrice3;
                    p.CreatedBy = userService.FindUserByEmail(System.Web.HttpContext.Current.User.Identity.GetUserName());
                    p.CreatedDateTime = DateTime.Now;
                    itemPriceService.Save(p);
                }
            }
            catch(Exception e)
            {
                string error = "error in item price save";
                Console.WriteLine("An error occurred in Item Price Save: '{0}'", e);
                return false;
            }
            return true;
        }


        public ActionResult Shortfall()
        {
            return View();
        }
    }


}