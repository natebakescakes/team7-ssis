using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Services
{
    public class CollectionPointService
    {
        ApplicationDbContext context;
        public CollectionPointService(ApplicationDbContext context)
        {
            this.context = context;
        }
        public List<CollectionPoint> FindAllCollectionPoints()
        {
            throw new NotImplementedException();
        }
    }
}