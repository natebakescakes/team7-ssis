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

        /// <summary>
        /// Saves new or existing model to DbContext
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Model that was saved to DbContext</returns>
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

        /// <summary>
        /// Finds model by respective id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Model that was found</returns>
        public virtual TModel FindById(TId id)
        {
            return entity.Find(id);
        }

        /// <summary>
        /// Returns DbSet of entity
        /// </summary>
        /// <returns>Returns DbSet of entity</returns>
        public IQueryable<TModel> FindAll()
        {
            return entity;
        }

        /// <summary>
        /// Returns no. of rows in DbSet
        /// </summary>
        /// <returns>Returns no. of rows in DbSet</returns>
        public int Count()
        {
            return entity.Count();
        }

        /// <summary>
        /// Deletes model that was passed in
        /// </summary>
        /// <param name="model"></param>
        public void Delete(TModel model)
        {
            entity.Remove(model);
            context.SaveChanges();
        }

        /// <summary>
        /// Checks if model exists by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean value of model existing</returns>
        public bool ExistsById(TId id)
        {
            return entity.Find(id) != null;
        }
    }
}