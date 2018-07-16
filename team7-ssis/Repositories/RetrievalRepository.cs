using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class RetrievalRepository : CrudRepository<Retrieval, String>
    {
        public RetrievalRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Retrieval;
        }

        /// <summary>
        /// Find Retrieval objects that match start and end date range inclusive by CreatedDateTime
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of Retrieval objects</returns>
        public IQueryable<Retrieval> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.Retrieval
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }
    }
}