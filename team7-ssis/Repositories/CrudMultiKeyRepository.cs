using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class CrudMultiKeyRepository<TModel, TId1, TId2> : CrudRepository<TModel, TId1>
        where TModel : class
    { 

        public TModel FindById(TId1 id1, TId2 id2)
        {
            return entity.Find(id1, id2);
        }

        public bool ExistsById(TId1 id1, TId2 id2)
        {
            return entity.Find(id1, id2) != null;
        }
    }
}