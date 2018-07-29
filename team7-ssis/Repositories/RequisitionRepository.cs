using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class RequisitionRepository : CrudRepository<Requisition, String>
    {
        public RequisitionRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Requisition;
        }

        /// <summary>
        /// Find Requisition objects that match start and end date range inclusive by CreatedDateTIme
        /// </summary>
        /// <param name="startDateRange"></param>
        /// <param name="endDateRange"></param>
        /// <returns>IQueryable of Requisition objects</returns>
        public IQueryable<Requisition> FindByCreatedDateTime(DateTime startDateRange, DateTime endDateRange)
        {
            return context.Requisition
                .Where(x => x.CreatedDateTime.CompareTo(startDateRange) >= 0 &&
                    x.CreatedDateTime.CompareTo(endDateRange) <= 0);
        }

        public IQueryable<Requisition> FindRequisitionsByStatus(List<Status> statusList)
        {

            var statusIdList = statusList.Select(x => x.StatusId);

            return context.Requisition.Where(requisition => statusIdList.Contains(requisition.Status.StatusId));
        }
        public IQueryable<RequisitionDetail> FindRequisitionDetails(string requisitionId)
        {
            return context.RequisitionDetail.Where(x => x.RequisitionId == requisitionId);
        }

        public IQueryable<Requisition> FindByDepartment(Department department)
        {
            return context.Requisition.Where(r => r.Department.DepartmentCode == department.DepartmentCode);
        }
    }
}
