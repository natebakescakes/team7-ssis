using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class DeliveryorderService
    {
        ApplicationDbContext context;
        public DeliveryorderService(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        public DeliveryOrder FindDeliveryOrderById(string deliveryOrderNo)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryOrder> FindAllDeliveryOrders()
        {
            throw new NotImplementedException();
        }

        public List<DeliveryOrder> FindDeliveryOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            throw new NotImplementedException();
        }

        public DeliveryOrder FindDeliveryOrderBySupplier(string supplierCode)
        {
            throw new NotImplementedException();
        }

       
        public DeliveryOrder Save(DeliveryOrder DeliveryOrder)
        {
            throw new NotImplementedException();
        }
       
        public void SaveDOFileToDeliveryOrder(string filepath)
        {
            throw new NotImplementedException();
        }

        public void SaveInvoiceFileToDeliveryOrder(string filepath)
        {
            throw new NotImplementedException();
        }

    }
}