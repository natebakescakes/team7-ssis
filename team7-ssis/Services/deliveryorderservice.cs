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
        
        public DeliveryOrder FindDeliveryOrderById(string deliveryorderno)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryOrder> FindAllDeliveryOrders(string query)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryOrder> FindDeliveryOrderByPurchaseOrderNo(string purchaseorderno)
        {
            throw new NotImplementedException();
        }

        public DeliveryOrder FindDeliveryOrderBySupplier(string suppliercode)
        {
            throw new NotImplementedException();
        }

       
        public void Save(DeliveryOrder DeliveryOrder)
        {
            throw new NotImplementedException();
        }
       
        public void SavePOFileToDeliveryOrder(string filepath)
        {
            throw new NotImplementedException();
        }
                                 
    }
}