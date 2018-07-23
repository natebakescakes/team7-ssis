﻿using System;
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
        public List<SupplierViewModel> Suppliers()
        {
            return supplierService.FindAllSuppliers().Select(supplier => new SupplierViewModel()
            {
                SupplierCode = supplier.SupplierCode,
                Address = supplier.Address,
                PhoneNumber = supplier.PhoneNumber,
                ContactName = supplier.ContactName,
                FaxNumber = supplier.FaxNumber,
                Name = supplier.Name
            }).ToList();
        }


        public SupplierViewModel GetSupplier(string id)
        {
            Supplier supplier = supplierService.FindSupplierById(id);
            return new SupplierViewModel()
            {
                SupplierCode = supplier.SupplierCode,
                Address = supplier.Address,
                PhoneNumber = supplier.PhoneNumber,
                ContactName = supplier.ContactName,
                FaxNumber = supplier.FaxNumber,
                Name = supplier.Name,
                GSTNumber = supplier.GstRegistrationNo,
                Status = supplier.Status.StatusId
            };
            
        }

    }
}