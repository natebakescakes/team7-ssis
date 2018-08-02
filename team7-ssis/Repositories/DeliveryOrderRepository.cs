using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class DeliveryOrderRepository : CrudRepository<DeliveryOrder, String>
    {
        public DeliveryOrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.DeliveryOrder;
        }

        /// <summary>
        /// Find DeliveryOrder objects that match start and end date range inclusive by CreatedDateTime
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of DeliveryOrder objects</returns>
        public IQueryable<DeliveryOrder> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.DeliveryOrder
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }

        public IQueryable<DeliveryOrder> FindDeliveryOrderByPurchaseOrderNo(string purchaseOrderNo)
        {
            return context.DeliveryOrder
                .Where(x => x.PurchaseOrder.PurchaseOrderNo == purchaseOrderNo);
        }

        public IQueryable<DeliveryOrder> FindDeliveryOrderBySupplier(string supplierCode)
        {
            return context.DeliveryOrder
                .Where(x => x.Supplier.SupplierCode == supplierCode);
        }

        public IQueryable<PurchaseOrderDetail> FindPurchaseOrderDetailbyPurchaseOrderNumber(string purchaseordernumber)
        {
            return context.PurchaseOrderDetail.Where(x => x.PurchaseOrderNo==purchaseordernumber);
        }
    }
}
