using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class DisbursementRepository : CrudRepository<Disbursement, String>
    {
        public DisbursementRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Disbursement;
        }

        /// <summary>
        /// Find Disbursement objects that match start and end date range inclusive by CreatedDateTime
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of Disbursement objects</returns>
        public IQueryable<Disbursement> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.Disbursement
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }
    }
}
