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
    public class DeliveryOrderApiController : ApiController
    {
        private ApplicationDbContext context;
        private DeliveryOrderService deliveryOrderService;
        private ItemService itemService;

        public DeliveryOrderApiController()
        {
            context = new ApplicationDbContext();
            deliveryOrderService = new DeliveryOrderService(context);
            itemService = new ItemService(context);
        }

        [Route("api/receivegoods/all")]
        [HttpGet]
        public List<DeliveryOrderViewModel> DeliveryOrders()
        {
            return deliveryOrderService.FindAllDeliveryOrders().Select(x => new DeliveryOrderViewModel()
                {
                DeliveryOrderNo = x.DeliveryOrderNo,

                PurchaseOrder_PurchaseOrderNo = x.PurchaseOrder.PurchaseOrderNo,

                 Supplier_SupplierCode = x.Supplier.SupplierCode,
                 
                }).ToList();
         }
    }
}