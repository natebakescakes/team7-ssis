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
    }
}