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
    public class ItemCategoryAPIController : ApiController
    {
        ApplicationDbContext context;
        ItemCategoryService itemcategoryService;

        public ItemCategoryAPIController()
        {
            context = new ApplicationDbContext();
            itemcategoryService = new ItemCategoryService(context);

        }

        [Route("api/itemcategory/all")]
        [HttpGet]
        public IHttpActionResult ItemCategories()
        {
            var itemcategories = itemcategoryService.FindAllItemCategory();

            if (itemcategories.Count == 0) return NotFound();

            return Ok( itemcategories.Select(x => new ItemCategoryViewModel()
            {
                ItemCategoryId = x.ItemCategoryId,
                Name = x.Name,
                Description = x.Description,
                StatusName = x.Status.Name
                
 
            }).ToList());
        }

        public ItemCategoryViewModel GetItemCategory(string id)
        {
            ItemCategory itemcategory = itemcategoryService.FindItemCategoryByItemCategoryId(int.Parse(id));
            return new ItemCategoryViewModel()
            {
                ItemCategoryId = itemcategory.ItemCategoryId,
                Name = itemcategory.Name,
                Description = itemcategory.Description,
                Status = itemcategory.Status.StatusId
            };

        }
    }
}
