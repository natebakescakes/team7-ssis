using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class CollectionPointRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public CollectionPoint Save(CollectionPoint collectionPoint)
        {
            throw new NotImplementedException();
        }

        public CollectionPoint FindById(int collectionPointId)
        {
            throw new NotImplementedException();
        }

        public List<CollectionPoint> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.CollectionPoint.Count();
        }

        public void Delete(CollectionPoint collectionPoint)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int collectionPointId)
        {
            throw new NotImplementedException();
        }
    }
}
