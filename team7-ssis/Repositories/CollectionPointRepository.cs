using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class CollectionPointRepository : CrudRepository<CollectionPoint, int>
    {
        public CollectionPointRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.CollectionPoint;
        }
        public CollectionPoint FindByDepartment(Department department)
        {
            return context.CollectionPoint.Where(x => x.CollectionPointId == department.CollectionPoint.CollectionPointId).FirstOrDefault();
        }
    }
}
