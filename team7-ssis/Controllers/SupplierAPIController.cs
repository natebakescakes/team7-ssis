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
    public class SupplierAPIController : ApiController
    {
        static ApplicationDbContext context = new ApplicationDbContext();
        SupplierService supplierService = new SupplierService(context);

        [Route("api/supplier/all")]
        [HttpGet]
        public IEnumerable<SupplierViewModel> Suppliers()
        {
            List<Supplier> list = supplierService.FindAllSuppliers();
            List<SupplierViewModel> suppliers = new List<SupplierViewModel>();

            foreach (Supplier s in list)
            {
                suppliers.Add(new SupplierViewModel
                {
                    SupplierCode = s.SupplierCode,
                    Address = s.Address,
                    PhoneNumber = s.PhoneNumber,
                    ContactName = s.ContactName,
                    FaxNumber = s.FaxNumber,
                    Name = s.Name

                });
            }

            return suppliers;
        }
        
    }
}
