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
    }
}