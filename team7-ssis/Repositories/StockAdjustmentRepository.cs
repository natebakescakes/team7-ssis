using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class StockAdjustmentRepository : CrudRepository<StockAdjustment, String>
    {
        public StockAdjustmentRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.StockAdjustment;
        }

        /// <summary>
        /// Find StockAdjustment objects that match start and end date range inclusive by CreatedDateTime
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of StockAdjustment objects</returns>
        public IQueryable<StockAdjustment> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.StockAdjustment
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }
    }
}
