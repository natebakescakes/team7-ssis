using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using team7_ssis.Models;
using team7_ssis.Repositories;

namespace team7_ssis.Services
{
    public class CollectionPointService
    {
        ApplicationDbContext context;
        CollectionPointRepository collectionPointRepository;
        public CollectionPointService(ApplicationDbContext context)
        {
            this.context = context;
            this.collectionPointRepository = new CollectionPointRepository(context);
        }
        public List<CollectionPoint> FindAllCollectionPoints()
        {
            return collectionPointRepository.FindAll().ToList();
        }
    }
}