using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class DelegationRepository : CrudRepository<Delegation, int>
    {
        public DelegationRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Delegation;
        }

        /// <summary>
        /// Find DeliveryOrder objects that match start and end date range inclusive by CreatedDateTime
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of DeliveryOrder objects</returns>
        public IQueryable<Delegation> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.Delegation
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }
        public IQueryable<Delegation> FindByDepartment(ApplicationUser user)
        {
            return context.Delegation
                 .Where(x => x.Receipient.Department.DepartmentCode == user.Department.DepartmentCode);
        }
    }
}