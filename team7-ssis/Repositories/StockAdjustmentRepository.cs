using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using team7_ssis.Models;

namespace team7_ssis.Repository
{
    public class StockAdjustmentRepository
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public StockAdjustment Save(StockAdjustment stockAdjustment)
        {
            throw new NotImplementedException();
        }

        public StockAdjustment FindById(string stockAdjustmentId)
        {
            throw new NotImplementedException();
        }

        public List<StockAdjustment> FindAll()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            return context.StockAdjustment.Count();
        }

        public void Delete(StockAdjustment stockAdjustment)
        {
            throw new NotImplementedException();
        }

        bool ExistsById(int stockAdjustmentId)
        {
            throw new NotImplementedException();
        }
    }
}
