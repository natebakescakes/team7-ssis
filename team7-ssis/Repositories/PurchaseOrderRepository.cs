using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class PurchaseOrderRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public PurchaseOrder Save(PurchaseOrder purchaseOrder)
        {
            throw new NotImplementedException();
        }

        public PurchaseOrder FindById(string purchaseOrderNo)
        {
            throw new NotImplementedException();
        }

        public List<PurchaseOrder> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.PurchaseOrder.Count();
        }

        public void Delete(PurchaseOrder purchaseOrder)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(string purchaseOrderNo)
        {
            throw new NotImplementedException();
        }
    }
}
