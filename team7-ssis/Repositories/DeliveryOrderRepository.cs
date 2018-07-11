using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class DeliveryOrderRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public DeliveryOrder Save(DeliveryOrder deliveryOrder)
        {
            throw new NotImplementedException();
        }

        public DeliveryOrder FindById(string deliveryOrderNo)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryOrder> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.DeliveryOrder.Count();
        }

        public void Delete(DeliveryOrder deliveryOrder)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string deliveryOrderNo)
        {
            throw new NotImplementedException();
        }
    }
}
