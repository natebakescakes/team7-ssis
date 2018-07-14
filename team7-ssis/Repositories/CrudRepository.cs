using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting;
using team7_ssis.Models;

namespace team7_ssis.Repositories
{
    public class CrudRepository<TModel, TId>
        where TModel : class
    {
        protected DbSet<TModel> entity;
        protected ApplicationDbContext context;

        public TModel Save(TModel model)
        {
            TModel result;
            if (entity.AsEnumerable().Contains(model))
            {
                result = model;
            }
            else
            {
                result = entity.Add(model);
            }

            context.SaveChanges();
            return result;
        }

        public virtual TModel FindById(TId id)
        {
            return entity.Find(id);
        }

        public List<TModel> FindAll()
        {
            return entity.ToList();
        }

        public int Count()
        {
            return entity.Count();
        }

        public void Delete(TModel model)
        {
            entity.Remove(model);
            context.SaveChanges();
        }

        public bool ExistsById(TId id)
        {
            return entity.Find(id) != null;
        }
    }
}