using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class DeliveryOrderService
    {
        ApplicationDbContext context;
        DeliveryOrderRepository deliveryOrderRepository;

        public DeliveryOrderService(ApplicationDbContext context,DeliveryOrderRepository deliveryOrderRepository)
        {
            this.context = context;
            this.deliveryOrderRepository = deliveryOrderRepository;
        }

        public List<DeliveryOrder> FindAllDeliveryOrders()
        {
            return deliveryOrderRepository.FindAll().ToList();
        }

        public DeliveryOrder FindDeliveryOrderById(string deliveryOrderNo)
        {
            return deliveryOrderRepository.FindById(deliveryOrderNo);
        }


        public DeliveryOrder FindDeliveryOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            return deliveryOrderRepository.FindDeliveryOrderByPurchaseOrderNo(purchaseOrderNo);
        }

        public DeliveryOrder FindDeliveryOrderBySupplier(string supplierCode)
        {
            return deliveryOrderRepository.FindDeliveryOrderBySupplier(supplierCode);
        }
  
        public DeliveryOrder Save(DeliveryOrder DeliveryOrder)
        {
            return deliveryOrderRepository.Save(DeliveryOrder);
        }
       
        public void SaveDOFileToDeliveryOrder(string Filepath)
        {
            throw new NotImplementedException();
            // deliveryOrderRepository.SaveInvoiceFileToDeliveryOrder(Filepath);
            //Need to send to Finance department
        }

        public void SaveInvoiceFileToDeliveryOrder(string Filepath)
        {
            throw new NotImplementedException();
          //  deliveryOrderRepository.SaveInvoiceFileToDeliveryOrder(Filepath);
            //Need to send to Finance department
        }

    }
}