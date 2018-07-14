using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class SupplierRepository : CrudRepository<Supplier, String>
    {
        public SupplierRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Supplier;
        }
    }
}
