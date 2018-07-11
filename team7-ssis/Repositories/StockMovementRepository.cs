using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class StockMovementRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public StockMovement Save(StockMovement stockMovement)
        {
            throw new NotImplementedException();
        }

        public StockMovement FindById(int StockMovementId)
        {
            throw new NotImplementedException();
        }

        public List<StockMovement> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.StockMovement.Count();
        }

        public void Delete(StockMovement StockMovement)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int stockMovementId)
        {
            throw new NotImplementedException();
        }
    }
}
