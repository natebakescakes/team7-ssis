using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class StockMovementRepository : CrudRepository<StockMovement, int>
    {
        public StockMovementRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.StockMovement;
        }

        public IQueryable<StockMovement> FindByItemCode(string itemCode)
        {
            return context.StockMovement.Where(x => x.Item.ItemCode == itemCode).OrderBy(x => x.CreatedDateTime);
        }

        public IQueryable<StockMovement> FindByDisbursementId(string id)
        {
            return context.StockMovement.Where(x => x.DisbursementId==id);
        }

        public IQueryable<StockMovement> FindByStockAdjustmentId(string id)
        {
            return context.StockMovement.Where(x => x.StockAdjustmentId == id);
        }
    }
}
