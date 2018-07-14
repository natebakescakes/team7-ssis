using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class TitleRepository : CrudRepository<Title, int>
    {
        public TitleRepository(ApplicationDbContext context)
        {
            this.context = context;
            this.entity = context.Title;
        }
    }
}